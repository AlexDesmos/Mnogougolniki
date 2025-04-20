using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Mnogougolniki.Figures;
using Point = Avalonia.Point;
using System.Drawing;
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
    private int shapeType, _algorithmType;
    
    
    public override void Render(DrawingContext context)
    {
        foreach (var shape in shapes)
        {
            shape.Draw(context);
        }

        if (shapes.Count >= 3)
        {
            switch (_algorithmType)
            {
                case 0:
                    DrawConvexHullByDef(context);
                    break;
                case 1:
                    DrawConvexHullJarvis(context);
                    break;
            }
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

            if (shapes.Count >= 3)
            {
                switch (_algorithmType)
                {
                    case 0:
                        UpdatePointsInConvexHull();
                        break;
                    case 1:
                        UpdateConvexHullJarvis();
                        break;
                }
                var drag = false;
                if (!shapes.Last().IsInConvexHull)
                {
                    drag = true;
                    shapes.Remove(shapes.Last());
                    foreach (var shape in shapes)
                    {
                        shape.Moving = true;
                    }
                    prevX = newX;
                    prevY = newY;
                }

                if (!drag)
                {
                    RemoveShapesInsideHull();
                }
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
        RemoveShapesInsideHull();
        InvalidateVisual();
        
    }

    public void ChangeType(int type)
    {
        shapeType = type;
    }

    public void ChangeAlgorithmType(int type)
    {
        _algorithmType = type;
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
                    s1.IsInConvexHull = true;
                    s2.IsInConvexHull = true;
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
    private static double GetCoors(Shape a, Shape b, Shape c)
    {
        var ba = (a.X - b.X, a.Y - b.Y);
        var bc = (c.X - b.X, c.Y - b.Y);
        var scalarProduct = ba.Item1 * bc.Item1 + ba.Item2 * bc.Item2;
        var baLen = Math.Sqrt(ba.Item1 * ba.Item1 + ba.Item2 * ba.Item2);
        var bcLen = Math.Sqrt(bc.Item1 * bc.Item1 + bc.Item2 * bc.Item2);
        double coors = scalarProduct / (baLen * bcLen);
        return coors;
    }
    private void DrawConvexHullJarvis(DrawingContext context)
    {
        foreach (var shape in shapes)
        {
            shape.IsInConvexHull = false;
        }

        Brush lineBrush = new SolidColorBrush(Colors.Fuchsia);
        Pen pen = new(lineBrush, lineCap: PenLineCap.Square);
        double minX = Int32.MaxValue;
        double minY = Int32.MinValue;
        Shape first = new Circle(0, 0, Colors.Blue);
        foreach (var s in shapes)
        {
            if (s.Y > minY)
            {
                minY = s.Y;
                minX = s.X;
                first = s;
            }
            else if (Math.Abs(s.Y - minY) < 1e-4)
            {
                if (s.X < minX)
                {
                    minY = s.Y;
                    minX = s.X;
                    first = s;
                }
            }
        }

        shapes.Find(s => s == first)!.IsInConvexHull = true;
        Shape mid = new Circle(first.X - 0.1, first.Y, Colors.Blue);
        Shape end = mid;
        double maxCoors = -2;
        foreach (var s in shapes)
        {
            if (s == mid || s == first) continue;
            if (maxCoors < GetCoors(first, mid, s))
            {
                end = s;
                maxCoors = GetCoors(first, mid, s);
            }
        }

        mid = end;
        shapes.Find(i => i == end)!.IsInConvexHull = true;
        var p1 = new Point(first.X, first.Y);
        var p2 = new Point(mid.X, mid.Y);
        context.DrawLine(pen, p1, p2);
        var start = first;
        while (true)
        {
            double minCoors = 2;
            foreach (var s in shapes)
            {
                if (s == start || s == mid) continue;
                if (minCoors > GetCoors(start, mid, s))
                {
                    end = s;
                    minCoors = GetCoors(start, mid, s);
                }
            }

            start = mid;
            mid = end;
            shapes.Find(i => i == end)!.IsInConvexHull = true;
            p1 = new Point(start.X, start.Y);
            p2 = new Point(mid.X, mid.Y);
            context.DrawLine(pen, p1, p2);
            if (end == first)
            {
                break;
            }
        }
    }
    private void UpdateConvexHullJarvis()
    {
        foreach (var shape in shapes)
        {
            shape.IsInConvexHull = false;
        }

        double minX = Int32.MaxValue;
        double minY = Int32.MinValue;
        Shape first = new Circle(0, 0, Colors.Blue);
        foreach (var s in shapes)
        {
            if (s.Y > minY)
            {
                minY = s.Y;
                minX = s.X;
                first = s;
            }
            else if (Math.Abs(s.Y - minY) < 1e-4)
            {
                if (s.X < minX)
                {
                    minY = s.Y;
                    minX = s.X;
                    first = s;
                }
            }
        }

        shapes.Find(s => s == first)!.IsInConvexHull = true;
        Shape mid = new Circle(first.X - 0.1, first.Y, Colors.Blue);
        Shape end = mid;
        double maxCoors = -2;
        foreach (var s in shapes)
        {
            if (s == mid || s == first) continue;
            if (maxCoors < GetCoors(first, mid, s))
            {
                end = s;
                maxCoors = GetCoors(first, mid, s);
            }
        }

        mid = end;
        shapes.Find(i => i == end)!.IsInConvexHull = true;
        var start = first;
        while (true)
        {
            double minCoors = 2;
            foreach (var s in shapes)
            {
                if (s == start || s == mid) continue;
                if (minCoors > GetCoors(start, mid, s))
                {
                    end = s;
                    minCoors = GetCoors(start, mid, s);
                }
            }

            start = mid;
            mid = end;
            shapes.Find(i => i == end)!.IsInConvexHull = true;
            if (end == first)
            {
                break;
            }
        }
    }
    public Tuple<int, double>[] GetChartJarvis()
    {
        int[] sizes = [10, 50, 100, 150, 200, 250, 300, 350, 400, 450, 500];
        Tuple<int, double>[] chart = new Tuple<int, double>[11];
        var rnd = new Random();
        var timer = new Stopwatch();
        List<Shape> shapes = [];
        for (int j = 0; j < sizes.Length; ++j)
        {
            timer.Reset();
            shapes.Clear();
            for (int i = 0; i < sizes[j]; ++i)
            {
                shapes.Add(new Circle(rnd.Next(1, 10000), rnd.Next(1, 10000), Colors.Blue));
            }

            if (j == 0)
            {
                UpdateConvexHullJarvis();
            }

            timer.Start();
            UpdateConvexHullJarvis();
            timer.Stop();
            var elapsed = timer.Elapsed.TotalMilliseconds;
            chart[j] = new(sizes[j], (int)(100 * elapsed));
        }

        return chart;
    }

    public Tuple<int, double>[] GetChartByDef()
    {
        int[] sizes = [10, 50, 100, 150, 200, 250, 300, 350, 400, 450, 500];
        Tuple<int, double>[] chart = new Tuple<int, double>[11];
        var rnd = new Random();
        var timer = new Stopwatch();
        List<Shape> shapes = [];
        for (int j = 0; j < sizes.Length; ++j)
        {
            timer.Reset();
            shapes.Clear();
            for (int i = 0; i < sizes[j]; ++i)
            {
                shapes.Add(new Circle(rnd.Next(1, 10000), rnd.Next(1, 10000), Colors.Blue));
            }

            if (j == 0)
            {
                UpdatePointsInConvexHull();
            }
            timer.Start();
            UpdatePointsInConvexHull();
            timer.Stop();
            var elapsed = timer.Elapsed.TotalMilliseconds;
            chart[j] = new(sizes[j], (int)(100 * elapsed));
        }

        return chart;
    }
}   