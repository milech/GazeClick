// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - wrapper for serializable/deserializable config for application options
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

using System.ComponentModel;

namespace GazeClick.Models
{
    internal class MyConfigWrapper : INotifyPropertyChanged
    {
        private static MyConfigWrapper _instance;
        private static readonly object _lock = new object();
        private MyConfig _myConfig;

        private MyConfigWrapper()
        {
        }

        public static MyConfigWrapper GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MyConfigWrapper();
                    }
                }
            }
            return _instance;
        }

        public MyConfig Config
        {
            get => _myConfig;
            set
            {
                _myConfig = value;
            }
        }

        public bool IsRegistering   // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.IsRegistering;
            set
            {
                _myConfig.IsRegistering = value;
            }
        }

        public bool IsCursorMoving  // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.IsCursorMoving;
            set
            {
                _myConfig.IsCursorMoving = value;
                OnPropertyChanged("IsCursorMoving");
            }
        }

        public bool IsCursorClicking    // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.IsCursorClicking;
            set
            {
                _myConfig.IsCursorClicking = value;
                OnPropertyChanged("IsCursorClicking");
            }
        }

        public bool IsCursorSmoothening     // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.IsCursorSmoothening;
            set
            {
                _myConfig.IsCursorSmoothening = value;
                OnPropertyChanged("IsCursorSmoothening");
            }
        }

        public int MinClickTime     // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.MinClickTime;
            set
            {
                _myConfig.MinClickTime = value;
                OnPropertyChanged("MinClickTime");
            }
        }

        public int ClickTime    // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.ClickTime;
            set
            {
                _myConfig.ClickTime = value;
            }
        }

        public int MaxClickTime     // referenced via Binding in MainWindow.xaml
        {
            get => _myConfig.MaxClickTime;
            set
            {
                _myConfig.MaxClickTime = value;
                OnPropertyChanged("MaxClickTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
