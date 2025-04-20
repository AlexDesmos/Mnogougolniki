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
        public Triangle(double x, double y, Color c) : base(x, y, c) { }
        private Point point1, point2, point3;
        double S = r * r * 0.75 * Math.Sqrt(3);
        private double r2 = r*(double)Math.Sqrt(3)/2;
        private double r3 =(double)r / 2;
        
        public override bool InSide(double nx, double ny)
        {
            Point p1 = new Point(x, y - r);
            Point p2 = new Point(x - r2, y + r3);
            Point p3 = new Point(x + r2, y + r3);
            Point pointClick = new Point(nx, ny);
            if (Math.Abs(S - (Heron(p1, p2, pointClick)
                              + Heron(p1, p3, pointClick)
                              + Heron(p2, p3, pointClick))) <= 1)
            {
                return true;
            }
            else{return false;}
        }

        public override void Draw(DrawingContext context)
        {
            Brush lineBrush = new SolidColorBrush(color);
            Pen pen = new(lineBrush, 2, lineCap: PenLineCap.Square);
            var point1 = new Point(x, y - r);
            var point2 = new Point(x - r2, y + r3);
            var point3 = new Point(x + r2, y + r3);
            context.DrawLine(pen, point1, point2);
            context.DrawLine(pen, point1, point3);
            context.DrawLine(pen, point2, point3);
        }
        private static double Heron(Point p1, Point p2, Point p3 )
        {
            double a = Point.Distance(p1, p2);
            double b = Point.Distance(p1, p3);
            double c = Point.Distance(p2, p3);
            double p = (a + b + c) / 2;
                return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
    
}
