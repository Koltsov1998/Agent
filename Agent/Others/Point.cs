using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Others
{

    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Point;
            if (other == null)
            {
                return false;
            }

            return this.X == other.X && this.Y == other.Y;
        }

        public bool Near(Point anotherPoint)
        {
            var dx = Math.Abs(X - anotherPoint.X);
            var dy = Math.Abs(Y - anotherPoint.Y);

            return dx + dy == 1;
        }

        public override string ToString()
        {
            return $"[{X} {Y}]";
        }
    }
}
