using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6v9
{
    // Власний делегат для арифметичних операцій
    // delegate double BinaryOperation(double x, double y);
    public delegate double BinaryOperation(double x, double y);

    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ================== 1. Власний делегат + анонімні методи ==================

            // Анонімний метод (delegate) для додавання
            // BinaryOperation
            BinaryOperation add = delegate (double a, double b)
            {
                return a + b;
            };

            // Лямбда-вираз для множення
            // BinaryOperation
            BinaryOperation multiply = (a, b) => a * b;

            Console.WriteLine("=== Власний делегат BinaryOperation ===");
            Console.WriteLine($"5 + 7 = {add(5, 7)}");
            Console.WriteLine($"3 * 4 = {multiply(3, 4)}");
            Console.WriteLine();

            // ================== 2. Модель предметної області ==================

            var cars = new List<Car>
            {
                new Car("Toyota Corolla",   85000,   6.5),
                new Car("Volkswagen Passat",135000,  7.2),
                new Car("BMW 320d",         210000,  5.8),
                new Car("Skoda Octavia",    120000,  6.0),
                new Car("Honda Civic",      95000,   6.1),
            };

            // Func<Car, double> – проєкція автомобіля в числову метрику (витрата)
            Func<Car, double> getConsumption = c => c.FuelConsumption;

            // Predicate<Car> – перевірка для відбору за пробігом
            Predicate<Car> highMileage = c => c.Mileage > 100000;

            // Action<Car> – дія над елементом, без повернення значення
            Action<Car> printCar = c =>
                Console.WriteLine($"{c.Model,-20} Пробіг: {c.Mileage,7} км  Витрата: {c.FuelConsumption:F1} л/100км");

            Console.WriteLine("=== Вихідний список автомобілів ===");
            cars.ForEach(printCar);
            Console.WriteLine();

            // ================== 3. LINQ + лямбда-вирази ==================

            // 3.1. Середня витрата пального (Average + Func<Car,double>)
            double averageConsumption = cars.Average(getConsumption);
            Console.WriteLine($"Середня витрата пального: {averageConsumption:F2} л/100км");
            Console.WriteLine();

            // 3.2. Автомобіль з мінімальною витратою (OrderBy + лямбда)
            Car minConsumptionCar = cars
                .OrderBy(c => c.FuelConsumption)
                .First();

            Console.WriteLine("Автомобіль з мінімальною витратою:");
            printCar(minConsumptionCar);
            Console.WriteLine();

            // 3.3. Відбір авто з пробігом > 100000 (Where + Predicate<Car>)
            var highMileageCars = cars
                .Where(c => highMileage(c))
                .ToList();

            Console.WriteLine("Автомобілі з пробігом > 100000 км:");
            highMileageCars.ForEach(printCar);
            Console.WriteLine();

            // 3.4. Використання Select для проєкції в анонімний тип
            var shortInfo = cars
                .Select(c => new
                {
                    c.Model,
                    Category = c.Mileage > 150000 ? "Великий пробіг" : "Нормальний пробіг"
                });

            Console.WriteLine("Коротка інформація (Select + лямбда):");
            foreach (var info in shortInfo)
            {
                Console.WriteLine($"{info.Model,-20} -> {info.Category}");
            }
            Console.WriteLine();

            // 3.5. Aggregate – сумарна витрата (демонстрація використання лямбда-виразу як агрегатора)
            // Func<double, Car, double>
            double totalConsumption = cars.Aggregate(
                0.0,
                (sum, car) => sum + car.FuelConsumption);

            Console.WriteLine($"Сумарна \"витрата\" (сума значень витрати): {totalConsumption:F2}");
        }
    }

    // Клас Car (модель, пробіг, витрата пального)
    public class Car
    {
        public string Model { get; }
        public int Mileage { get; }            // пробіг у км
        public double FuelConsumption { get; } // л/100км

        public Car(string model, int mileage, double fuelConsumption)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            if (mileage < 0) throw new ArgumentOutOfRangeException(nameof(mileage));
            if (fuelConsumption <= 0) throw new ArgumentOutOfRangeException(nameof(fuelConsumption));

            Mileage = mileage;
            FuelConsumption = fuelConsumption;
        }

        public override string ToString() =>
            $"{Model}: {Mileage} км, {FuelConsumption:F1} л/100км";
    }
}
