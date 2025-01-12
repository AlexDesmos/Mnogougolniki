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
        public abstract bool InSide(int nx, int ny);
        protected static readonly int r;
        protected int x, y;
        protected static Color color;
        
        public bool Moving {get; set;}
        protected Shape(int x, int y)
        {
            this.x = x;
            this.y = y;
            
        }

        static Shape()
        {
            r = 30;
            color = Colors.Chocolate;
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
