using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace lab29v9.Services;
    public class FileGenerator
    {
        public static async Task GenerateAsync(string path, int lines)
    {
        using var writer = new StreamWriter(path, false, Encoding.UTF8);

        await writer.WriteLineAsync("id,email");

        await writer.WriteLineAsync("1,andriy1@gmail.com");
        await writer.WriteLineAsync("2,andriy2@gmail.com");

        for (int i = 3; i <= lines; i++)
        {
            int mod = i % 3;

            string domain = mod switch
            {
                0 => "gmail.com",
                1 => "yahoo.com",
                _ => "outlook.com"
            };

            string email = $"user{i}@{domain}";
            await writer.WriteLineAsync($"{i},{email}");
            }
        }
    }