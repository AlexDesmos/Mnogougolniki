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
    private readonly List<Shape> shapes = [
    new Circle(100, 100, Colors.OrangeRed),
    new Square(150,200, Colors.Chartreuse),
    new Triangle(300,300, Colors.Fuchsia)
    ];
    private int prevX, prevY, prevC;
    private int shapeType;

    public override void Render(DrawingContext context)
    {
        foreach (var shape in shapes)
        {
            shape.Draw(context);
        }

        if (shapes.Count >= 3)
        {
            DrawConvexHullByDef(context);
        }
    }

    public void LeftClick(int newX, int newY, Avalonia.Input.PointerPoint point)
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
                    shapes.Add(new Circle(newX, newY, Colors.OrangeRed));
                    break;
                case 1:
                    shapes.Add(new Triangle(newX, newY, Colors.Chartreuse));
                    break;
                case 2:
                    shapes.Add(new Square(newX, newY, Colors.Cyan));
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

    private void RemoveShapesInsideHull()
    {
        foreach (var shape in shapes.ToList().Where(shape => !shape.IsInConvexHull))
        {
            shapes.Remove(shape);
        }

        InvalidateVisual();
    }

    private void DrawConvexHullByDef(DrawingContext context)
    {
        foreach (var shape in shapes)
        {
            shape.IsInConvexHull = false;
        }

        int i = 0;
        foreach (var s1 in shapes)
        {
            if (i == shapes.Count - 1)
            {
                break;
            }

            int j = 0;
            foreach (var s2 in shapes)
            {
                if (j <= i)
                {
                    j++;
                    continue;
                }

                int l = 0;
                bool upper = false, lower = false;
                double k = (double)(s2.Y - s1.Y) / (s2.X - s1.X);
                double b = s2.Y - k * s2.X;
                foreach (var s3 in shapes)
                {
                    if ((s3.X == s2.X && s3.Y == s2.Y) || (s3.X == s1.X && s3.Y == s1.Y))
                    {
                        l++;
                        continue;
                    }
                    if (l != i && l != j)
                    {
                        if (s1.X != s2.X)
                        {
                            if (s3.X * k + b > s3.Y)
                            {
                                lower = true;
                            }
                            else if (s3.X * k + b < s3.Y)
                            {
                                upper = true;
                            }
                        }
                        else
                        {
                            if (s2.X > s3.X)
                            {
                                lower = true;
                            }
                            else if (s2.X <= s3.X)
                            {
                                upper = true;
                            }
                        }
                    }

                    l++;
                }

                if (upper != lower)
                {
                       
                    Brush lineBrush = new SolidColorBrush(Colors.Blue);
                    Pen pen = new(lineBrush, lineCap: PenLineCap.Square);
                    var point1 = new Point(s1.X, s1.Y);
                    var point2 = new Point(s2.X, s2.Y);
                    context.DrawLine(pen, point1, point2);
                }

                j++;
            }

            i++;
        }
    }
    private void UpdatePointsInConvexHull()
    {
        foreach (var shape in shapes)
        {
            shape.IsInConvexHull = false;
        }

        int i = 0;
        foreach (var s1 in shapes)
        {
            if (i == shapes.Count - 1)
            {
                break;
            }

            int j = 0;
            foreach (var s2 in shapes)
            {
                if (j <= i)
                {
                    j++;
                    continue;
                }

                int l = 0;
                bool upper = false, lower = false;
                double k = (double)(s2.Y - s1.Y) / (s2.X - s1.X);
                double b = s2.Y - k * s2.X;
                foreach (var s3 in shapes)
                {
                    if (l != i && l != j)
                    {
                        if (s1.X != s2.X)
                        {
                            if (s3.X * k + b > s3.Y)
                            {
                                lower = true;
                            }
                            else if (s3.X * k + b < s3.Y)
                            {
                                upper = true;
                            }
                        }
                        else
                        {
                            if (s2.X > s3.X)
                            {
                                lower = true;
                            }
                            else if (s2.X < s3.X)
                            {
                                upper = true;
                            }
                        }
                    }

                    l++;
                }

                if (upper != lower || (upper == false && lower == false))
                {
                    s1.IsInConvexHull = true;
                    s2.IsInConvexHull = true;
                }

                j++;
            }

            i++;
        }
    }
}   