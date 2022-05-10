using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;


namespace GazeClick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WpfEyeXHost _eyeXHost;

        private GazeDot gazeDot;
        private DispatcherTimer gazeTimer;
        private MyPoint prevPoint;

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool GetCursorPos(out POINT lpPoint);

        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    public int X;
        //    public int Y;

        //    public POINT(int x, int y)
        //    {
        //        this.X = x;
        //        this.Y = y;
        //    }
        //}

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private int clicksCounter = 0;
        private double timeStamp = 0;


        public MainWindow()
        {
            InitializeComponent();

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            Log log = new Log();
            Console.SetOut(Log.getStreamWriter());

            this.prevPoint = new MyPoint();
            this.gazeDot = new GazeDot();

            gazeTimer = new DispatcherTimer();
            gazeTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)stareThrSlider.Value);
            gazeTimer.Tick += gazeTimer_Tick;
            gazeTimer.Start();            

            var lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
                {
                    try
                    {
                        if (logCheckbox.IsChecked == true)
                        {
                            Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, DateTime.Now, e.Timestamp);
                            timeStamp = e.Timestamp;
                        }
                            
                        this.setDotPosition(e);
                            
                        if (moveCursorCheckbox.IsChecked == true)
                            SetCursorPos((int)(e.X), (int)(e.Y));
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };

            registerButton.Click += (s, e) =>
                {
                    try
                    {
                        clicksCounter++;
                        Console.WriteLine(clicksCounter.ToString() + ": ----------- Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} -----------", this.gazeDot.Left, this.gazeDot.Top, DateTime.Now, timeStamp);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
        }


        private void setDotPosition(EyeXFramework.GazePointEventArgs e)
        {
            this.gazeDot.Left = e.X;
            this.gazeDot.Top = e.Y;
        }


        private void gazeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                MyPoint p = new MyPoint((int) this.gazeDot.Left, (int) this.gazeDot.Top);
                int xDiff = Math.Abs(this.prevPoint.X - p.X);
                int yDiff = Math.Abs(this.prevPoint.Y - p.Y);

                if (p.X > 0 && p.Y > 0 && xDiff != 0 && yDiff != 0 && xDiff < 400 && yDiff < 400 && emulateClicksCheckbox.IsChecked == true)
                {
                    //%mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Convert.ToUInt32(p.X), Convert.ToUInt32(p.Y), 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(p.X), Convert.ToUInt32(p.Y), 0, 0);
                    Thread.Sleep(100);
                    mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(p.X), Convert.ToUInt32(p.Y), 0, 0);
                }

                this.prevPoint = p;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.close();
            this.gazeDot.Close();
            this.gazeTimer.Stop();
            _eyeXHost.Dispose();
            //App.Current.Shutdown();
        }


        private void stareThrSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.gazeTimer != null)
                this.gazeTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)stareThrSlider.Value);
        }


        private void emulateClicksCheckbox_Click(object sender, RoutedEventArgs e)
        {
            stareThrSlider.IsEnabled = (bool)emulateClicksCheckbox.IsChecked;
        }


        private void showMarkerCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (showMarkerCheckbox.IsChecked == false)
                gazeDot.Visibility = System.Windows.Visibility.Hidden;
            else
                gazeDot.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
