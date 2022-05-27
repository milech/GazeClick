using System;
using System.Runtime.InteropServices;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;
using GazeClick.Models;
using System.ComponentModel;

namespace GazeClick.ViewModels
{
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
        private double _timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private MyPoint _currentPoint;
        private MyPoint _prevPoint;

        public GazeClickViewModel() // referenced via Binding in MainWindow.xaml
        {
            App.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);    // TODO: refactor

            Log log = Log.GetInstance();
            GazeTimer gazeTimer = GazeTimer.GetInstance();
            gazeTimer.SetLog(log);

            //Console.SetOut(log.GetStreamWriter());

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            _currentPoint = new MyPoint();
            _prevPoint = new MyPoint();

            //gazeDot = new GazeDot();

            gazeTimer.Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    if (log.IsOn)
                    {
                        log.Write(string.Concat("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, DateTime.Now, e.Timestamp));
                        _timeStamp = e.Timestamp;
                    }

                    //SetDotPosition(e);

                    //if (moveCursorCheckbox.IsChecked == true)
                    //{
                    //    _ = SetCursorPos((int)e.X, (int)e.Y);
                    //}

                    //Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, e.Timestamp);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    if (log.IsOn)
                    {
                        log.Write(ex.Message);
                    }
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
=> GazeTimer.GetInstance();

        public Log Log  // Referenced via binding in MainWindow
=> Log.GetInstance();

        public void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
            //gazeDot.Close();
            GazeTimer.Stop();
            _eyeXHost.Dispose();
            //App.Current.Shutdown();
        }
    }
}
