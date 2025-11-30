using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5v9
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var test = new Test(
                id: "T1",
                title: "C# Basics",
                questions: new[]
                {
                    "Який тип для цілих чисел?",
                    "Ключове слово для оголошення класу?",
                    "Який простір імен містить Console?"
                },
                correctAnswers: new[]
                {
                    "int",
                    "class",
                    "System"
                });

            IRepository<Student> studentsRepository = new Repository<Student>();

            var s1 = new Student("S1", "Іван Іванов", "KP-11");
            var s2 = new Student("S2", "Марія Петренко", "KP-11");
            var s3 = new Student("S3", "Петро Сидоренко", "KP-11");

            s1.AddAnswers(test.Id, new[]
            {
                new Answer(0, "int"),
                new Answer(1, "class"),
                new Answer(2, "System")
            });

            s2.AddAnswers(test.Id, new[]
            {
                new Answer(0, "int"),
                new Answer(1, "struct"),
                new Answer(2, "System")
            });

            s3.AddAnswers(test.Id, new[]
            {
                new Answer(0, "int"),
                new Answer(5, "class")
            });

            studentsRepository.Add(s1);
            studentsRepository.Add(s2);
            studentsRepository.Add(s3);

            var scores = new List<TestScore>();

            try
            {
                foreach (var student in studentsRepository.All())
                {
                    var score = TestEvaluator.EvaluateStudentTest(test, student);
                    scores.Add(score);

                    Console.WriteLine(
                        $"{student.Name}: {score.CorrectAnswers}/{score.TotalQuestions} ({score.Percent:F2}%)");
                }

                var groupAverage = TestEvaluator.GroupAveragePercent(scores);
                Console.WriteLine();
                Console.WriteLine($"Середній відсоток групи: {groupAverage:F2}%");

                var bestScore = scores.MaxBy(s => s.Percent);
                Console.WriteLine(
                    $"Найкращий результат: {bestScore.StudentName} - {bestScore.Percent:F2}%");
            }
            catch (InvalidAnswerException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Сталася помилка у відповідях:");
                Console.WriteLine($"Повідомлення: {ex.Message}");
                Console.WriteLine($"Індекс питання: {ex.QuestionIndex}");
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Помилка пошуку в репозиторії:");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Непередбачена помилка:");
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Test
    {
        public string Id { get; }
        public string Title { get; }
        public IReadOnlyList<string> Questions { get; }
        public IReadOnlyList<string> CorrectAnswers { get; }

        public Test(
            string id,
            string title,
            IEnumerable<string> questions,
            IEnumerable<string> correctAnswers)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));

            var qList = questions?.ToList() ?? throw new ArgumentNullException(nameof(questions));
            var aList = correctAnswers?.ToList() ?? throw new ArgumentNullException(nameof(correctAnswers));

            if (qList.Count == 0)
                throw new ArgumentException("Test must contain at least one question.", nameof(questions));

            if (qList.Count != aList.Count)
                throw new ArgumentException("Questions count must match correct answers count.");

            Questions = qList;
            CorrectAnswers = aList;
        }

        public int QuestionsCount => Questions.Count;
    }


    public class Answer
    {
        public int QuestionIndex { get; }
        public string Value { get; }

        public Answer(int questionIndex, string value)
        {
            if (questionIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(questionIndex));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            QuestionIndex = questionIndex;
            Value = value;
        }
    }


    public class Student
    {
        public string Id { get; }
        public string Name { get; }
        public string Group { get; }

        public Dictionary<string, List<Answer>> AnswersByTestId { get; } = new();

        public Student(string id, string name, string group)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public void AddAnswers(string testId, IEnumerable<Answer> answers)
            => AnswersByTestId[testId] = answers.ToList();
    }


    public class InvalidAnswerException : Exception
    {
        public int QuestionIndex { get; }

        public InvalidAnswerException(string message, int index)
            : base(message)
        {
            QuestionIndex = index;
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message) { }
    }

    public interface IRepository<T>
    {
        void Add(T item);
        IEnumerable<T> All();
    }

    public class Repository<T> : IRepository<T>
    {
        private readonly List<T> _data = new();

        public void Add(T item) => _data.Add(item);

        public IEnumerable<T> All() => _data;
    }

    public static class EnumerableExtensions
    {
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext())
                throw new InvalidOperationException();

            var maxElem = e.Current;
            var maxKey = selector(maxElem);
            var cmp = Comparer<TKey>.Default;

            while (e.MoveNext())
            {
                var cand = e.Current;
                var candKey = selector(cand);

                if (cmp.Compare(candKey, maxKey) > 0)
                {
                    maxKey = candKey;
                    maxElem = cand;
                }
            }

            return maxElem;
        }
    }

    public record TestScore(
        string StudentId,
        string StudentName,
        string Group,
        string TestId,
        string TestTitle,
        int CorrectAnswers,
        int TotalQuestions,
        double Percent);

    public static class TestEvaluator
    {
        public static TestScore EvaluateStudentTest(Test test, Student student)
        {
            if (!student.AnswersByTestId.TryGetValue(test.Id, out var answers))
            {
                return new TestScore(student.Id, student.Name, student.Group,
                    test.Id, test.Title, 0, test.QuestionsCount, 0);
            }

            var correct = 0;

            foreach (var ans in answers)
            {
                if (ans.QuestionIndex < 0 || ans.QuestionIndex >= test.QuestionsCount)
                    throw new InvalidAnswerException("Недійсний індекс питання", ans.QuestionIndex);

                if (test.CorrectAnswers[ans.QuestionIndex].Equals(ans.Value, StringComparison.OrdinalIgnoreCase))
                    correct++;
            }

            var percent = (double)correct / test.QuestionsCount * 100;

            return new TestScore(student.Id, student.Name, student.Group,
                test.Id, test.Title, correct, test.QuestionsCount, Math.Round(percent, 2));
        }

        public static double GroupAveragePercent(IEnumerable<TestScore> scores)
            => scores.Any() ? scores.Average(x => x.Percent) : 0;
    }
}
