using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using lab29v9.Services;

namespace lab29v9.Services;
    public class Program
    {
        static async Task Main()
    {
        string filePath = "data.csv";
        string filteredPath = "filtered.csv";

        var sw = new Stopwatch();

        // Генерація файлу
        Console.WriteLine("Генерація файлу...");
        sw.Start();
        await FileGenerator.GenerateAsync(filePath, 1_000_000);
        sw.Stop();
        Console.WriteLine($"Генерація: {sw.ElapsedMilliseconds} ms");

        // Асинхронне читання
        sw.Restart();
        var stats = await FileReaderAsync.ReadAndAnalyzeAsync(filePath);
        sw.Stop();

        Console.WriteLine("\nСтатистика:");
        foreach (var item in stats)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

        Console.WriteLine($"Async читання: {sw.ElapsedMilliseconds} ms");

        // Фільтрація gmail
        sw.Restart();
        await FileFilter.FilterAsync(filePath, filteredPath, "gmail.com");
        sw.Stop();

        Console.WriteLine($"Фільтрація gmail: {sw.ElapsedMilliseconds} ms");

        Console.WriteLine("\nГотово ");
    }
        
    }