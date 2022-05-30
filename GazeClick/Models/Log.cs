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
        private const string _standardLogEntry = "{0:0.0}\t{1:0.0}\t{2:MM/dd/yy H:mm:ss fffffff}\t{3:0}";

        private Log()
        {
            LogDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GazeClick logs\\";
            LogPath = LogDir + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
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

        public string StandardLogEntry => _standardLogEntry;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (IsOn)
            {
                if (!Directory.Exists(LogDir))  // setting boolean variable would be faster but keep it this way in case someone deletes the folder during runtime
                {
                    _ = Directory.CreateDirectory(LogDir);
                }
                if (_sw == null)
                {
                    _sw = new StreamWriter(LogPath);
                }
            }
        }

        public void Write(string message)
        {
            if (IsOn && _sw != null)
            {
                _sw.WriteLine(message);
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
