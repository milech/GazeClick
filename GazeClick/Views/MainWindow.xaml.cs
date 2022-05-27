using System.Windows;

namespace GazeClick.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Following the MVVM pattern - no code here
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //private void SetDotPosition(EyeXFramework.GazePointEventArgs e)
        //{
        //    gazeDot.Left = e.X - gazeDot.Width / 2.0;
        //    gazeDot.Top = e.Y - gazeDot.Height / 2.0;
        //}


        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Log.Close();
        //    gazeDot.Close();
        //    gazeTimer.Stop();
        //    _eyeXHost.Dispose();
        //    //App.Current.Shutdown();
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
