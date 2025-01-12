using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

namespace Mnogougolniki.Figures
{
    public sealed class Triangle : Shape
    {
        public Triangle(int x = 0, int y= 0) : base(x, y) { }
        private Point point1, point2, point3;
        private static double S => r * r * 0.25 * 3 * Math.Sqrt(3);
        public override bool InSide(int nx, int ny)
        {
            var pointClick = new Point(nx, ny);
            return Math.Abs(S - Heron(point1, point2, point3)
                              - Heron(point1, point3, point2)
                              - Heron(point2, point3, pointClick)) <= 0.1;
        }

        public override void Draw(DrawingContext context)
        {
            Brush lineBrush = new SolidColorBrush(color);
            Pen pen = new(lineBrush, 2, lineCap: PenLineCap.Square);
            var point1 = new Point(x, y - r);
            var point2 = new Point(x - r * (float)Math.Sqrt(3) / 2, y + (float)r / 2);
            var point3 = new Point(x + r * (float)Math.Sqrt(3) / 2, y + (float)r / 2);
            context.DrawLine(pen, point1, point2);
            context.DrawLine(pen, point1, point3);
            context.DrawLine(pen, point2, point3);
        }
        private static double Heron(Point p1, Point p2, Point p3)
        {
            var a = Point.Distance(p1, p2);
            var b = Point.Distance(p1, p3);
            var c = Point.Distance(p2, p3);
            var p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
    
}
