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
