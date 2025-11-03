using System;
using System.Collections.Generic;
using System.Linq;

public class InvalidAnswerException : Exception
{
    public InvalidAnswerException(string message) : base(message) { }
}

public class Answer
{
    public int QuestionNumber { get; set; }
    public char GivenAnswer { get; set; }

    public Answer(int questionNumber, char givenAnswer)
    {
        if (questionNumber <= 0)
            throw new InvalidAnswerException($"Номер питання {questionNumber} є недійсним!");

        QuestionNumber = questionNumber;
        GivenAnswer = char.ToUpper(givenAnswer);
    }
}

public class Test
{
    public List<char> CorrectAnswers { get; set; }

    public Test(List<char> correctAnswers)
    {
        if (correctAnswers == null || correctAnswers.Count == 0)
            throw new ArgumentException("Ключ тесту не може бути порожнім.");
        CorrectAnswers = correctAnswers.Select(char.ToUpper).ToList();
    }

    public int GetScore(List<Answer> studentAnswers)
    {
        int score = 0;
        foreach (var ans in studentAnswers)
        {
            if (ans.QuestionNumber > CorrectAnswers.Count)
                throw new InvalidAnswerException($"Питання №{ans.QuestionNumber} не існує!");
            if (ans.GivenAnswer == CorrectAnswers[ans.QuestionNumber - 1])
                score++;
        }
        return score;
    }

    public double GetPercentage(List<Answer> studentAnswers)
        => (double)GetScore(studentAnswers) / CorrectAnswers.Count * 100.0;
}

public class Student
{
    public string Name { get; set; }
    public List<Answer> Answers { get; set; }

    public Student(string name, List<Answer> answers)
    {
        Name = name;
        Answers = answers;
    }

    public override string ToString() => Name;
}

public class Repository<T>
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> All() => _items;

    public T MaxBy(Func<T, double> selector)
    {
        if (!_items.Any())
            throw new InvalidOperationException("Колекція порожня — немає елементів для порівняння.");

        T best = _items[0];
        double maxValue = selector(best);

        foreach (var item in _items.Skip(1))
        {
            double val = selector(item);
            if (val > maxValue)
            {
                best = item;
                maxValue = val;
            }
        }

        return best;
    }

    public T MinBy(Func<T, double> selector)
    {
        if (!_items.Any())
            throw new InvalidOperationException("Колекція порожня — немає елементів для порівняння.");

        T worst = _items[0];
        double minValue = selector(worst);

        foreach (var item in _items.Skip(1))
        {
            double val = selector(item);
            if (val < minValue)
            {
                worst = item;
                minValue = val;
            }
        }

        return worst;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            var correct = new List<char> { 'A', 'B', 'C', 'D', 'A' };
            var test = new Test(correct);

            var s1 = new Student("Андрій", new List<Answer> {
                new Answer(1,'A'), new Answer(2,'B'),
                new Answer(3,'C'), new Answer(4,'D'), new Answer(5,'A')
            });

            var s2 = new Student("Марія", new List<Answer> {
                new Answer(1,'B'), new Answer(2,'B'),
                new Answer(3,'A'), new Answer(4,'C'), new Answer(5,'A')
            });

            var s3 = new Student("Ігор", new List<Answer> {
                new Answer(1,'A'), new Answer(2,'C'),
                new Answer(3,'B'), new Answer(4,'D'), new Answer(5,'B')
            });

            var repo = new Repository<Student>();
            repo.Add(s1);
            repo.Add(s2);
            repo.Add(s3);

            Console.WriteLine("=== РЕЗУЛЬТАТИ ТЕСТУ ===");
            foreach (var s in repo.All())
            {
                int score = test.GetScore(s.Answers);
                double percent = test.GetPercentage(s.Answers);
                Console.WriteLine($"{s.Name,-10}: {score}/{test.CorrectAnswers.Count} ({percent:F1}%)");
            }

            var best = repo.MaxBy(s => test.GetPercentage(s.Answers));
            var worst = repo.MinBy(s => test.GetPercentage(s.Answers));

            Console.WriteLine($"\n Найкращий: {best.Name}");
            Console.WriteLine($" Найгірший: {worst.Name}");

        }
        catch (InvalidAnswerException ex)
        {
            Console.WriteLine($" Помилка відповіді: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($" Помилка: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Невідома помилка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\n Перевірку завершено.");
        }
    }
}
