using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeClick
{
    class MyPoint
    {
        private int _x;
        private int _y;

        public MyPoint()
        {
            this._x = 0;
            this._y = 0;
        }

        public MyPoint(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
