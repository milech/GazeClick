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

        public bool IsRegistering
        {
            get => _myConfig.IsRegistering;
            set
            {
                _myConfig.IsRegistering = value;
                OnPropertyChanged("IsRegistering");
            }
        }

        public bool IsCursorMoving
        {
            get => _myConfig.IsCursorMoving;
            set
            {
                _myConfig.IsCursorMoving = value;
                OnPropertyChanged("IsCursorMoving");
            }
        }

        public bool IsCursorClicking
        {
            get => _myConfig.IsCursorClicking;
            set
            {
                _myConfig.IsCursorClicking = value;
                OnPropertyChanged("IsCursorClicking");
            }
        }

        public bool IsCursorSmoothening
        {
            get => _myConfig.IsCursorSmoothening;
            set
            {
                _myConfig.IsCursorSmoothening = value;
                OnPropertyChanged("IsCursorSmoothening");
            }
        }

        public int MinClickTime
        {
            get => _myConfig.MinClickTime;
            set
            {
                _myConfig.MinClickTime = value;
                OnPropertyChanged("MinClickTime");
            }
        }

        public int ClickTime
        {
            get => _myConfig.ClickTime;
            set
            {
                _myConfig.ClickTime = value;
                OnPropertyChanged("ClickTime");
            }
        }

        public int MaxClickTime
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
