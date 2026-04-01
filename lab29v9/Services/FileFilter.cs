using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab29v9.Services;
    public class FileFilter
    {
        public static async Task FilterAsync(string input, string output, string domainFilter)
        {
            using var reader = new StreamReader(input);
            using var writer = new StreamWriter(output);

            string? header = await reader.ReadLineAsync();
            await writer.WriteLineAsync(header);

            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var email = line.Split(',')[1];

                if (email.EndsWith(domainFilter))
                {
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
