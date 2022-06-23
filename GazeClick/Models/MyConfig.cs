// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - serializable/deserializable config for application options
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

namespace GazeClick.Models
{
    internal class MyConfig
    {
        public bool IsRegistering { get; set; } = false;
        public bool IsCursorMoving { get; set; } = false;
        public bool IsCursorClicking { get; set; } = false;
        public bool IsCursorSmoothening { get; set; } = true;
        public int MinClickTime { get; set; } = 500;
        public int ClickTime { get; set; } = 2000;
        public int MaxClickTime { get; set; } = 5000;
        public int DeltaThr { get; set; } = 200;
    }
}
