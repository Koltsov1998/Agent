using System;
using System.Collections.Generic;
using System.Text;

namespace Crosses
{
    public class Size
    {
        private readonly int _width;

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Height { set; get; }
        public int Width { set; get; }
    }
}
