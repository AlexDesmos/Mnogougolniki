using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Mnogougolniki.Figures;

namespace Mnogougolniki;

public partial class Controls : UserControl
{
    private readonly List<Shape> shapes = [];
    private int prevX, prevY;
    private int shapeType;

    public override void Render(DrawingContext context)
    {
        foreach (var shape in shapes)
        {
            shape.Draw(context);
        }

        Console.WriteLine("Drawing in process...");
    }

    public void LeftClick(int newX, int newY)
    {
        bool inside = false;
        foreach (var shape in shapes.Where(shape => shape.InSide(newX, newY)))
        {
            Console.WriteLine("Click");
            prevX = newX;
            prevY = newY;
            shape.Moving = true;
            inside = true;
        }

        if (!inside)
        {
            switch (shapeType)
            {
                case 0:
                    shapes.Add(new Circle(newX, newY));
                    break;
                case 1:
                    shapes.Add(new Triangle(newX, newY));
                    break;
                case 2:
                    shapes.Add(new Square(newX, newY));
                    break;
            }
        }

        InvalidateVisual();
    }

    public void RightClick(int newX, int newY)
    {
        foreach (var shape in shapes.Where(shape => shape.InSide(newX, newY)).Reverse())
        {
            prevX = newX;
            prevY = newY;
            shapes.Remove(shape);
            break;
        }

        InvalidateVisual();
    }
    public void Move(int newX, int newY)
    {
        foreach (var shape in shapes.Where(shape => shape.Moving))
        {
            Console.WriteLine("Move");
            shape.X += newX - prevX;
            shape.Y += newY - prevY;
        }

        prevX = newX;
        prevY = newY;
        InvalidateVisual();
    }
    public void Release(int newX, int newY)
    {
        foreach (var shape in shapes.Where(shape => shape.Moving))
        {
            Console.WriteLine("Release");
            shape.X += newX - prevX;
            shape.Y += newY - prevY;
            shape.Moving = false;
        }

        prevX = newX;
        prevY = newY;
        InvalidateVisual();
    }

    public void ChangeType(int type)
    {
        shapeType = type;
    }
}    