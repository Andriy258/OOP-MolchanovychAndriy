using System;
using Lab2;

class Program
{
    static void Main(string[] args)
    {
        Inventory inventory = new Inventory();

        inventory += "Молоко";
        inventory += "Хліб";
        inventory += "Сир";

        Console.WriteLine(inventory);

        Console.WriteLine("Товар з індексом 1: " + inventory[1]);

        inventory[1] = "Булка";
        Console.WriteLine("Після зміни: " + inventory);

        string searchItem = "Сир";
        int index = inventory[searchItem];
        Console.WriteLine($"Товар \"{searchItem}\" має індекс: {index}");

        Console.WriteLine("Загальна кількість товарів: " + inventory.Count);
    }
}
