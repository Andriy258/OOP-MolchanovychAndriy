using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab22;
public class ShapeAreaCalculator
{
    public void PrintArea(IShape shape)
    {
        System.Console.WriteLine($"Площа форми: {shape.GetArea()}");
    }
}

public class RectangleShape : IShape
{
    private int width;
    private int height;

    public RectangleShape(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public int GetArea() => width * height;
}

public class SquareShape : IShape
{
    private int size;

    public SquareShape(int size)
    {
        this.size = size;
    }

    public int GetArea() => size * size;
}