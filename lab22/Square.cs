using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab22;

// Похідний клас, який порушує LSP
public class Square : Rectangle
{
    public Square(int size) : base(size, size) { }

    public override int Width
    {
        get => base.Width;
        set
        {
            base.Width = value;
            base.Height = value; // автоматично змінює висоту
        }
    }

    public override int Height
    {
        get => base.Height;
        set
        {
            base.Height = value;
            base.Width = value; // автоматично змінює ширину
        }
    }
}