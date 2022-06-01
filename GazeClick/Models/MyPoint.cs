// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - class for (x, y) point
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

namespace GazeClick.Models
{
    internal class MyPoint
    {
        // not using System.Windows.Point as it is sealed
        // thus cannot be inherited and creating association
        // would demand creating new instance for deep copy

        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public MyPoint(double x, double y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public int X
        {
            get; set;
        }

        public int Y
        {
            get; set;
        }

        public MyPoint Clone()
        {
            return (MyPoint)this.MemberwiseClone();
        }
    }
}
