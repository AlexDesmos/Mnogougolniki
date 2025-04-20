using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Mnogougolniki.Figures
{
    public abstract class Shape
    {
        public abstract bool InSide(double nx, double ny);
        protected static readonly int r;
        protected double x, y;
        protected static Color color;
        public bool IsInConvexHull { get; set; } = true;
        public bool Moving {get; set;}
        protected Shape(double x, double y, Color c)
        {
            this.x = x;
            this.y = y;
            color = c;

        }

        static Shape()
        {
            r = 30;
            color = Colors.Chocolate;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public abstract void Draw(DrawingContext context);
        
    }
}
