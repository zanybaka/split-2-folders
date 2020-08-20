## Description

<p>
Scans the current folder with the specified file mask and splits all the found files
by creation or modification date into separate folders like "YYYY.MM"
</p>

## Usage

<pre>
split2folders.exe [option] &lt;mask&gt;
</pre>

## Examples

<pre>
split2folders.exe *.*      - splits by creation date
split2folders.exe -c *.*   - splits by creation date
split2folders.exe -m *.*   - splits by modification date
</pre>

### Before
![alt](Example.png))

### After
![alt](Example2.png))