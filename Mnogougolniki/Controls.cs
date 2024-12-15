using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Mnogougolniki.Figures;

namespace Mnogougolniki;

public partial class Controls : UserControl
{
    public override void Render(DrawingContext context)
    {
        List<Shape> shapes =
        [
            new Circle(200, 200, Colors.Blue),
            new Triangle(300, 300, Colors.Chocolate),
            new Circle(200, 150, Colors.DarkRed),
            new Square(600, 450, Colors.Cyan),
        ];
        foreach (var s in shapes)
        {
            s.Draw(context);
        }
        Console.WriteLine("Drawing in process...");
    }
}