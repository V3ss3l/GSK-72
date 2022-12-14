using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSK_72
{
    public class CustomPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int C { get; set; }

        public CustomPoint(int x = 0, int y = 0, int c = 1)
        {
            X = x;
            Y = y;
            C = c;
        }

        public Point ToPoint() => new Point(X, Y);
    }
}
