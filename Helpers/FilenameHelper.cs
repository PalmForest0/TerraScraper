using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TerraScraper.Utility;

public static class FilenameHelper
{
    public static string ValidateFilename(string name, string replacement = " ")
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var builder = new StringBuilder(name.Length);

        foreach (char c in name)
        {
            if (invalidChars.Contains(c))
                builder.Append(replacement);
            else
                builder.Append(c);
        }

        return builder.ToString();
    }
}