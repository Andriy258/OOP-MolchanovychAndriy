using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SR12
{
    internal static class Program
    {
        private static bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            int limit = (int)Math.Sqrt(n);
            for (int i = 3; i <= limit; i += 2)
                if (n % i == 0) return false;

            return true;
        }

        private static List<int> GenerateRandomData(int size, int minValue, int maxValue)
        {
            var rnd = new Random(42);
            var list = new List<int>(size);

            for (int i = 0; i < size; i++)
                list.Add(rnd.Next(minValue, maxValue));

            return list;
        }

        private static void DemonstrateSideEffects()
        {
            var data = Enumerable.Range(1, 1_000_000).ToList();

            int unsafeSum = 0;

            data
                .AsParallel()
                .ForAll(x =>
                {
                    unsafeSum += x % 10;
                });

            Console.WriteLine($"unsafeSum (без синхронізації): {unsafeSum}");

            int safeSumLock = 0;
            object locker = new();

            data
                .AsParallel()
                .ForAll(x =>
                {
                    int v = x % 10;
                    lock (locker)
                    {
                        safeSumLock += v;
                    }
                });

            Console.WriteLine($"safeSumLock (lock): {safeSumLock}");

            int safeSumInterlocked = 0;

            data
                .AsParallel()
                .ForAll(x =>
                {
                    int v = x % 10;
                    Interlocked.Add(ref safeSumInterlocked, v);
                });

            Console.WriteLine($"safeSumInterlocked (Interlocked): {safeSumInterlocked}");
        }

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

            foreach (int size in sizes)
            {
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"Розмір: {size:N0}");

                var data = GenerateRandomData(size, 1, 5_000_000);

                var sw = Stopwatch.StartNew();

                var r1 = data
                    .Where(IsPrime)
                    .Select(x => x * 2)
                    .ToList();

                sw.Stop();
                long linqMs = sw.ElapsedMilliseconds;

                sw.Restart();

                var r2 = data
                    .AsParallel()
                    .Where(IsPrime)
                    .Select(x => x * 2)
                    .ToList();

                sw.Stop();
                long plinqMs = sw.ElapsedMilliseconds;

                Console.WriteLine($"LINQ:  {linqMs} ms");
                Console.WriteLine($"PLINQ: {plinqMs} ms");
                Console.WriteLine($"Count LINQ:  {r1.Count:N0}");
                Console.WriteLine($"Count PLINQ: {r2.Count:N0}");
            }

            Console.WriteLine(new string('=', 50));
            DemonstrateSideEffects();
        }
    }
}
