namespace GazeClick.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using System.Runtime.InteropServices;
    using EyeXFramework.Wpf;
    using Tobii.EyeX.Framework;
    using GazeClick.Models;

    internal class GazeClickViewModel
    {
        //private GazeDot gazeDot;

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        //private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private int clicksCounter = 0;
        private double timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private MyPoint _currentPoint;
        private MyPoint _prevPoint;

        public GazeClickViewModel()
        {
            Log log = new Log();
            Console.SetOut(Log.GetStreamWriter());

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            _currentPoint = new MyPoint();
            _prevPoint = new MyPoint();

            //gazeDot = new GazeDot();

            GazeTimer.GetInstance().Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    //if (logCheckbox.IsChecked == true)
                    //{
                    //    Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, DateTime.Now, e.Timestamp);
                    //    timeStamp = e.Timestamp;
                    //}

                    //SetDotPosition(e);

                    //if (moveCursorCheckbox.IsChecked == true)
                    //{
                    //    _ = SetCursorPos((int)e.X, (int)e.Y);
                    //}

                    Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, e.Timestamp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };

            //registerButton.Click += (s, e) =>
            //{
            //    try
            //    {
            //        clicksCounter++;
            //        Console.WriteLine(clicksCounter.ToString() + ": ----------- Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} -----------", gazeDot.Left, gazeDot.Top, DateTime.Now, timeStamp);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //};
        }

        public GazeTimer GazeTimer  // Referenced via binding in MainWindow
        {
            get
            {
                return GazeTimer.GetInstance();
            }
        }
    }
}
