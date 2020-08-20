using System;
using System.IO;
using System.Reflection;

namespace split_2_folders
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || args.Length > 2)
            {
                Console.WriteLine("Usages:");
                Console.WriteLine("");
                string name = Assembly.GetEntryAssembly()?.GetName().Name;
                Console.WriteLine($"{name}.exe *.*      - splits by creation date");
                Console.WriteLine($"{name}.exe -c *.*   - splits by creation date");
                Console.WriteLine($"{name}.exe -m *.*   - splits by modification date");
                return;
            }

            string mask;
            string param;
            if (args.Length > 1)
            {
                mask = args[1];
                param = args[0];
            }
            else
            {
                mask = args[0];
                param = "-c";
            }

            bool splitByModificationDate = param?.ToLowerInvariant() == "-m";

            var dir = new DirectoryInfo(".");
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
                    ? $"{file.LastWriteTime.Year}.{file.LastWriteTime.Month}"
                    : $"{file.CreationTime.Year}.{file.CreationTime.Month}";
                if (!Directory.Exists(name))
                {
                    Directory.CreateDirectory(name);
                }
                File.Move(file.FullName, $"{name}\\{file.Name}");
            }

            Console.WriteLine("Done");
        }
    }
}
