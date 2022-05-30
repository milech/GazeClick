using System;
using System.IO;
using System.ComponentModel;

namespace GazeClick.Models
{
    internal class Log : INotifyPropertyChanged
    {
        private static Log _instance;
        private StreamWriter _sw;
        private bool _isOn = false;
        private static readonly object _lock = new object();

        private Log()
        {
            LogDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GazeClick logs\\";
            LogPath = LogDir + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            PunchInCounter = 0;
        }

        public static Log GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Log();
                    }
                }
            }

            return _instance;
        }

        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                OnPropertyChanged("IsOn");
            }
        }

        public string LogDir
        {
            get; private set;
        }

        public string LogPath
        {
            get; private set;
        }

        public int PunchInCounter
        {
            get; private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (IsOn && !Directory.Exists(LogDir))  // setting boolean variable would be faster but keep it this way in case someone deletes the folder during runtime
            {
                _ = Directory.CreateDirectory(LogDir);
                _sw = new StreamWriter(LogPath);
            }
        }

        public void Write(string msg)
        {
            if (IsOn)
            {
                _sw.WriteLine(msg);
            }
        }

        public void Close()
        {
            if (_sw != null)
            {
                _sw.Close();
            }
        }
    }
}
