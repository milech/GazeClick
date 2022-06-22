using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeClick.Models
{
    internal class MyConfig
    {
        public bool IsRegistering { get; set; }
        public bool IsCursorMoving { get; set; }
        public bool IsCursorClicking { get; set; }
        public bool IsCursorSmoothening { get; set; }
        public int MinClickTime { get; set; } = 500;
        public int ClickTime { get; set; } = 2000;
        public int MaxClickTime { get; set; } = 5000;
        public int DeltaThr { get; set; } = 200;
    }
}
