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
        protected bool InSide = false;
        protected static int r;
        protected int x, y;
        protected Color color;

        protected Shape(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }

        static Shape()
        {
            r = 30;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public abstract void Draw(DrawingContext context);
        
    }
}
