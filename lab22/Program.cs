namespace lab22;

class Program
{
    static void Main()
    {
        System.Console.WriteLine("=== Порушення LSP ===");
        Rectangle rect = new Rectangle(5, 10);
        Square sq = new Square(5);

        System.Console.WriteLine($"Rectangle Area: {rect.GetArea()}"); // 50
        sq.Width = 10; // очікуємо 50, але отримаємо 100
        System.Console.WriteLine($"Square Area: {sq.GetArea()}"); // 100

        System.Console.WriteLine("\n=== Альтернатива LSP через композицію ===");
        ShapeAreaCalculator calculator = new ShapeAreaCalculator();

        IShape rectShape = new RectangleShape(5, 10);
        IShape squareShape = new SquareShape(5);

        calculator.PrintArea(rectShape);    // 50
        calculator.PrintArea(squareShape);  // 25
    }
}