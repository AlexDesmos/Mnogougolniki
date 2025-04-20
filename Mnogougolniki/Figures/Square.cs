using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Color = Avalonia.Media.Color;
using Point = Avalonia.Point;

namespace Mnogougolniki.Figures
{
    public sealed class Square : Shape
    {
        private Point point1, point2, point3, point4;   
        
        public Square(double x, double y, Color c) : base(x, y, c) { }

        public override bool InSide(double newX, double newY)
        {
            double InR = Math.Sqrt(r * r / 2);
            if (Math.Abs(newX - x)<= InR && Math.Abs(newY - y) <= InR)
            {
                return true;
            }
            else{return false;}
        }
        public override void Draw(DrawingContext context)
        {
            Brush lineBrush = new SolidColorBrush(color);
            Pen pen = new Pen(lineBrush, 2, lineCap: PenLineCap.Square);
            var r2 = r / (float)Math.Sqrt(2);
            var point1 = new Point(x-r2, y-r2);
            var point2 = new Point(x+r2, y-r2);
            var point3 = new Point(x+r2, y+r2);
            var point4 = new Point(x-r2, y+r2);
            context.DrawLine(pen, point1, point2);
            context.DrawLine(pen, point2, point3);
            context.DrawLine(pen, point3, point4);
            context.DrawLine(pen, point4, point1);
        }
    }
}
