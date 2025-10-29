using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4v9
{
    /// <summary>
    /// Інтерфейс оцінювання студента
    /// </summary>
    public interface IGrade
    {
        double GetScore();
        string GetDescription();
    }

    /// <summary>
    /// Абстрактний клас для спільних властивостей оцінок
    /// </summary>
    public abstract class Grade : IGrade
    {
        public string Name { get; set; }

        protected Grade(string name)
        {
            Name = name;
        }

        public abstract double GetScore();
        public abstract string GetDescription();
    }

    /// <summary>
    /// Клас для представлення оцінки за екзамен
    /// </summary>
    public class Exam : Grade
    {
        public double Score { get; set; }

        public Exam(string subject, double score) : base(subject)
        {
            Score = score;
        }

        public override double GetScore()
        {
            return Score;
        }

        public override string GetDescription()
        {
            return $"Екзамен з предмету: {Name}";
        }
    }

    /// <summary>
    /// Клас для представлення оцінки за проєкт
    /// </summary>
    public class Project : Grade
    {
        public double Score { get; set; }
        public double Difficulty { get; set; }

        public Project(string title, double score, double difficulty) : base(title)
        {
            Score = score;
            Difficulty = difficulty;
        }

        public override double GetScore()
        {
            // оцінка враховує коефіцієнт складності
            return Score * Difficulty;
        }

        public override string GetDescription()
        {
            return $"Проєкт: {Name} (складність: {Difficulty})";
        }
    }

    /// <summary>
    /// Клас, який зберігає оцінки студента (композиція)
    /// </summary>
    public class Student
    {
        public string Name { get; set; }
        public List<IGrade> Grades { get; set; } = new List<IGrade>();

        public Student(string name)
        {
            Name = name;
        }

        public void AddGrade(IGrade grade)
        {
            Grades.Add(grade);
        }

        /// <summary>
        /// Підсумковий середній бал студента
        /// </summary>
        public double GetFinalScore()
        {
            if (!Grades.Any()) return 0;
            return Grades.Average(g => g.GetScore());
        }
    }

    /// <summary>
    /// Основна програма
    /// </summary>
    public class Program
    {
        static double AverageGroupScore(List<Student> students)
        {
            if (students.Count == 0) return 0;
            return students.Average(s => s.GetFinalScore());
        }

        public static void Main(string[] args)
        {
            // створення студентів
            var s1 = new Student("Іваненко Іван");
            var s2 = new Student("Петренко Олена");

            // додавання оцінок
            s1.AddGrade(new Exam("Математика", 85));
            s1.AddGrade(new Project("Система оцінювання", 90, 1.2));

            s2.AddGrade(new Exam("Математика", 70));
            s2.AddGrade(new Project("Калькулятор", 75, 1.0));

            var students = new List<Student> { s1, s2 };

            // вивід результатів
            foreach (var s in students)
            {
                Console.WriteLine($"{s.Name}: підсумковий бал = {s.GetFinalScore():F2}");
            }

            Console.WriteLine($"\nСередній бал групи: {AverageGroupScore(students):F2}");
        }
    }
}
