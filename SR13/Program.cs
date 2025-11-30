using System;
using System.Net.Http;
using System.Threading;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace SR13
{
    internal class Program
    {
        private static int _apiAttempts = 0;
        private static int _dbAttempts = 0;
        private static int _workAttempts = 0;

        private static string CallExternalApi(string url)
        {
            _apiAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [API] Attempt {_apiAttempts}: Calling {url}...");

            if (_apiAttempts <= 2)
                throw new HttpRequestException($"API call failed for {url}");

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [API] Successful.");
            return "Data from API";
        }

        private static string QueryDatabase(string sql)
        {
            _dbAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Attempt {_dbAttempts}: Executing '{sql}'...");

            if (_dbAttempts <= 3)
                throw new InvalidOperationException("Transient DB failure");

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Query OK.");
            return "DB result";
        }

        private static string LongRunningOperation()
        {
            _workAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [WORK] Attempt {_workAttempts}: Starting...");

            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [WORK] Done.");

            return "Work completed";
        }

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            RunScenario1();
            Console.WriteLine();
            RunScenario2();
            Console.WriteLine();
            RunScenario3();
        }

        private static void RunScenario1()
        {
            Console.WriteLine("=== Scenario 1: External API Retry ===");

            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetry(
                    retryCount: 3,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    (ex, ts, rc, ctx) =>
                    {
                        Console.WriteLine(
                            $"[{DateTime.Now:HH:mm:ss}] [API] Retry {rc} after {ts.TotalSeconds}s due to: {ex.Message}");
                    });

            try
            {
                string result = retryPolicy.Execute(
                    () => CallExternalApi("https://api.example.com/data"));

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [API] Final: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [API] Failed: {ex.Message}");
            }

            Console.WriteLine("--- End Scenario 1 ---");
        }

        private static void RunScenario2()
        {
            Console.WriteLine("=== Scenario 2: DB Retry + CircuitBreaker ===");

            var circuitBreaker = Policy
                .Handle<InvalidOperationException>()
                .CircuitBreaker(
                    2,
                    TimeSpan.FromSeconds(10),
                    (ex, delay) =>
                    {
                        Console.WriteLine(
                            $"[{DateTime.Now:HH:mm:ss}] [DB] Break {delay.TotalSeconds}s: {ex.Message}");
                    },
                    () => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Reset"),
                    () => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Half-open"));

            var retryPolicy = Policy
                .Handle<InvalidOperationException>()
                .WaitAndRetry(
                    3,
                    attempt => TimeSpan.FromMilliseconds(500 * attempt),
                    (ex, ts, rc, ctx) =>
                    {
                        Console.WriteLine(
                            $"[{DateTime.Now:HH:mm:ss}] [DB] Retry {rc} after {ts.TotalMilliseconds}ms: {ex.Message}");
                    });

            var combined = retryPolicy.Wrap(circuitBreaker);

            try
            {
                string result = combined.Execute(
                    () => QueryDatabase("SELECT * FROM Users"));

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Final: {result}");
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Circuit open: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [DB] Failed: {ex.Message}");
            }

            Console.WriteLine("--- End Scenario 2 ---");
        }

        private static void RunScenario3()
        {
            Console.WriteLine("=== Scenario 3: Timeout + Fallback ===");

            var timeout = Policy
                .Timeout(
                    TimeSpan.FromSeconds(2),
                    TimeoutStrategy.Pessimistic,
                    (ctx, ts, task, ex) =>
                    {
                        Console.WriteLine(
                            $"[{DateTime.Now:HH:mm:ss}] [WORK] Timeout after {ts.TotalSeconds}s");
                    });

            var fallback = Policy<string>
                .Handle<TimeoutRejectedException>()
                .Fallback(
                    "Fallback: operation timed out.",
                    (ex, ctx) =>
                    {
                        Console.WriteLine(
                            $"[{DateTime.Now:HH:mm:ss}] [WORK] Fallback due to: {ex.Exception.Message}");
                    });

            var combined = fallback.Wrap(timeout);

            try
            {
                string result = combined.Execute(() => LongRunningOperation());
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [WORK] Final: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [WORK] Failed: {ex.Message}");
            }

            Console.WriteLine("--- End Scenario 3 ---");
        }
    }
}
