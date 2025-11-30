using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace lab7v9
{
    public class FileProcessor
    {
        private int counter = 0;

        public string ReadFile(string path)
        {
            counter++;

            if (path == "missing.txt")
                throw new FileNotFoundException("Файл не знайдено", path);

            if (counter < 3)
                throw new IOException("Тимчасова IO помилка читання файлу");

            return $"Файл \"{path}\" успішно прочитано";
        }
    }

    public class NetworkClient
    {
        private int attempts = 0;

        public string SendRequest()
        {
            attempts++;

            if (attempts < 3)
                throw new HttpRequestException("Мережева помилка (імітація тимчасової помилки)");

            return "Успішна відповідь від сервера";
        }
    }

    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool>? shouldRetry = null)
        {
            if (initialDelay == default)
                initialDelay = TimeSpan.FromMilliseconds(300);

            int attempt = 0;

            while (true)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"Виконання спроби #{attempt}...");
                    return operation();
                }
                catch (Exception ex)
                {
                    bool canRetry = shouldRetry?.Invoke(ex) ?? true;

                    Console.WriteLine($"Спроба #{attempt} не вдалася: {ex.GetType().Name}: {ex.Message}");

                    if (!canRetry)
                    {
                        Console.WriteLine("shouldRetry забороняє подальші спроби.");
                        throw;
                    }

                    if (attempt >= retryCount)
                    {
                        Console.WriteLine("Досягнуто максимальної кількості спроб.");
                        throw;
                    }

                    var delay = TimeSpan.FromMilliseconds(
                        initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

                    Console.WriteLine($"Очікування {delay.TotalMilliseconds} мс перед наступною спробою...");
                    Thread.Sleep(delay);
                }
            }
        }
    }

    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота №7: IO / Network помилки та Retry ===");

            var fileProcessor = new FileProcessor();
            var networkClient = new NetworkClient();

            Console.WriteLine("\n=== Тест 1: FileProcessor з тимчасовими IO-помилками ===");

            try
            {
                string fileResult = RetryHelper.ExecuteWithRetry(
                    () => fileProcessor.ReadFile("data.txt"),
                    retryCount: 5,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: ex => ex is IOException
                );

                Console.WriteLine($"Результат: {fileResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Файлова операція остаточно провалилась: {ex.GetType().Name}: {ex.Message}");
            }

            Console.WriteLine("\n=== Тест 2: NetworkClient з тимчасовими мережевими помилками ===");

            try
            {
                string response = RetryHelper.ExecuteWithRetry(
                    () => networkClient.SendRequest(),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromMilliseconds(300),
                    shouldRetry: ex => ex is HttpRequestException
                );

                Console.WriteLine($"Відповідь сервера: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Мережевий запит остаточно провалився: {ex.GetType().Name}: {ex.Message}");
            }

            Console.WriteLine("\n=== Тест 3: FileNotFoundException без повторів (shouldRetry блокує) ===");

            try
            {
                string missingResult = RetryHelper.ExecuteWithRetry(
                    () => fileProcessor.ReadFile("missing.txt"),
                    retryCount: 5,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: ex => ex is IOException && ex is not FileNotFoundException
                );

                Console.WriteLine($"Результат: {missingResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Очікувана зупинка без повторів: {ex.GetType().Name}: {ex.Message}");
            }

            Console.WriteLine("\n=== Кінець роботи ===");
        }
    }
}
