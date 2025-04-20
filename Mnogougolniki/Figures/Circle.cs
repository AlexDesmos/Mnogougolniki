using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

namespace Mnogougolniki.Figures
{
    public sealed class Circle : Shape
    {
        public Circle(double x, double y, Color c) : base(x, y, c) { }

        public override bool InSide(double nx, double ny)
        {
            return (x - nx) * (x - nx) + (y - ny) * (y - ny) <= r * r;
        }
        public override void Draw(DrawingContext context)
        {
            Brush brush = new SolidColorBrush(Colors.Black);
            Brush lineBrush = new SolidColorBrush(color);
            Pen pen = new(lineBrush, 2, lineCap: PenLineCap.Square);
            context.DrawEllipse(brush, pen, new Point(x, y), r, r);

        }

    }
}