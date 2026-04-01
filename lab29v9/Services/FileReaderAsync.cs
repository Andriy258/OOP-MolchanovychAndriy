using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab29v9.Services;
    public class FileReaderAsync
    {
        public static async Task<Dictionary<string, int>> ReadAndAnalyzeAsync(string path)
        {
            var stats = new Dictionary<string, int>();

            using var reader = new StreamReader(path);

            string? line;

            await reader.ReadLineAsync(); // пропустити заголовок

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var email = line.Split(',')[1];
                var domain = email.Split('@')[1];

                if (!stats.ContainsKey(domain))
                    stats[domain] = 0;

                stats[domain]++;
            }

            return stats;
        }
    }