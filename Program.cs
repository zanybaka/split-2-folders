using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace split_2_folders
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || args.Length > 4)
            {
                PrintUsage();
                return;
            }

            List<string> @params     = args.ToList();
            string       mode        = "-c";
            string       modePattern = @params.FirstOrDefault(x => x.Equals("-c", StringComparison.InvariantCultureIgnoreCase));
            if (modePattern != null)
            {
                @params.Remove(modePattern);
                mode = "-c";
            }
            else
            {
                modePattern = @params.FirstOrDefault(x => x.Equals("-m", StringComparison.InvariantCultureIgnoreCase));
                if (modePattern != null)
                {
                    @params.Remove(modePattern);
                    mode = "-m";
                }
            }

            string format = "yyyy.MM";
            var formatPattern = @params
                .Select((x, index) => new
                {
                    isFormat = x.Equals("-f", StringComparison.InvariantCultureIgnoreCase),
                    index,
                    value = x
                })
                .FirstOrDefault(x => x.isFormat);
            if (formatPattern != null)
            {
                if (formatPattern.index + 1 >= @params.Count)
                {
                    PrintUsage();
                    return;
                }

                format = @params[formatPattern.index + 1];
                @params.Remove(formatPattern.value);
                @params.Remove(format);
            }

            if (@params.Count != 1)
            {
                PrintUsage();
                return;
            }

            string mask = @params[0];

            bool splitByModificationDate = mode == "-m";

            Console.WriteLine("Parameters:");
            Console.WriteLine($"Mode:   {mode}");
            Console.WriteLine($"Format: {format}");
            Console.WriteLine($"Mask:   {mask}");
            Console.WriteLine();

            var dir   = new DirectoryInfo(".");
            var files = dir.GetFiles(mask);
            if (files.Length == 0)
            {
                Console.WriteLine($"No files to process. The specified mask is '{mask}'.");
                return;
            }

            Console.WriteLine($"Splitting {files.Length} files by {(splitByModificationDate ? "modification" : "creation")} date...");
            foreach (var file in files)
            {
                string name = splitByModificationDate
                    ? file.LastWriteTime.ToString(format)
                    : file.CreationTime.ToString(format);
                if (!Directory.Exists(name))
                {
                    Directory.CreateDirectory(name);
                }

                File.Move(file.FullName, $"{name}\\{file.Name}");
            }

            Console.WriteLine("Done");
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            string name = Assembly.GetEntryAssembly()?.GetName().Name;
            Console.WriteLine($"{name}.exe *.*                     - splits by creation date");
            Console.WriteLine($"{name}.exe -c *.*                  - splits by creation date");
            Console.WriteLine($"{name}.exe -f \"yyyy MM dd\" *.*     - splits by creation date with the specified format. Default is yyyy.MM");
            Console.WriteLine($"{name}.exe -m *.*                  - splits by modification date");
            Console.WriteLine($"{name}.exe -m -f \"yyyy.MM\" *.*     - splits by modification date with the specified format. Default is yyyy.MM");
        }
    }
}