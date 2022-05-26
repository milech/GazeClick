namespace GazeClick.Views
{
    using System.Windows;
    using GazeClick.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GazeClickViewModel();
        }
            //_eyeXHost = new WpfEyeXHost();
            //_eyeXHost.Start();

            //Log log = new Log();
            //Console.SetOut(Log.GetStreamWriter());

            //currentPoint = new MyPoint();
            //prevPoint = new MyPoint();

        //    gazeDot = new GazeDot();

        //    gazeTimer = new DispatcherTimer
        //    {
        //        Interval = new TimeSpan(0, 0, 0, 0, (int)stareThrSlider.Value)
        //    };
        //    gazeTimer.Tick += GazeTimer_Tick;
        //    gazeTimer.Start();

        //    EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

        //    lightlyFilteredGazeDataStream.Next += (s, e) =>
        //        {
        //            try
        //            {
        //                if (logCheckbox.IsChecked == true)
        //                {
        //                    Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} ", e.X, e.Y, DateTime.Now, e.Timestamp);
        //                    timeStamp = e.Timestamp;
        //                }

        //                SetDotPosition(e);

        //                if (moveCursorCheckbox.IsChecked == true)
        //                {
        //                    _ = SetCursorPos((int)e.X, (int)e.Y);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        };

        //    registerButton.Click += (s, e) =>
        //        {
        //            try
        //            {
        //                clicksCounter++;
        //                Console.WriteLine(clicksCounter.ToString() + ": ----------- Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} -----------", gazeDot.Left, gazeDot.Top, DateTime.Now, timeStamp);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        };
        //}


        //private void SetDotPosition(EyeXFramework.GazePointEventArgs e)
        //{
        //    gazeDot.Left = e.X - gazeDot.Width / 2.0;
        //    gazeDot.Top = e.Y - gazeDot.Height / 2.0;
        //}


        //private void GazeTimer_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        currentPoint.X = (int)gazeDot.Left;
        //        currentPoint.Y = (int)gazeDot.Top;
        //        int xDiff = Math.Abs(prevPoint.X - currentPoint.X);
        //        int yDiff = Math.Abs(prevPoint.Y - currentPoint.Y);

        //        if (currentPoint.X > 0 && currentPoint.Y > 0 &&
        //            xDiff != 0 && yDiff != 0 && xDiff < 400 && yDiff < 400 &&
        //            emulateClicksCheckbox.IsChecked == true)
        //        {
        //            //%mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Convert.ToUInt32(p.X), Convert.ToUInt32(p.Y), 0, 0);
        //            mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(currentPoint.X), Convert.ToUInt32(currentPoint.Y), 0, 0);
        //            Thread.Sleep(100);
        //            mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(currentPoint.X), Convert.ToUInt32(currentPoint.Y), 0, 0);
        //        }

        //        prevPoint.X = currentPoint.X;
        //        prevPoint.Y = currentPoint.Y;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}


        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Log.Close();
        //    gazeDot.Close();
        //    gazeTimer.Stop();
        //    _eyeXHost.Dispose();
        //    //App.Current.Shutdown();
        //}


        //private void FixationThrSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    if (gazeTimer != null)
        //    {
        //        gazeTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)stareThrSlider.Value);
        //    }
        //}


        //private void EmulateClicksCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    stareThrSlider.IsEnabled = (bool)emulateClicksCheckbox.IsChecked;
        //}


        //private void ShowMarkerCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    gazeDot.Visibility = showMarkerCheckbox.IsChecked == false ? Visibility.Hidden : Visibility.Visible;
        //}
    }
}
