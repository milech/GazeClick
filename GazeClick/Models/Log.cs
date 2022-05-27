namespace GazeClick.Models
{
    using System;
    using System.IO;
    using System.ComponentModel;

    internal class Log : INotifyPropertyChanged
    {
        private static Log _instance;
        private StreamWriter sw;
        private bool _isOn;

        private Log(bool isOn)
        {
            IsOn = isOn;
            string logDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GazeClick logs\\";
            string logPath = logDir + GetTimestamp(DateTime.Now) + ".txt";
            sw = new StreamWriter(logPath);

            if (!Directory.Exists(logDir))
            {
                _ = Directory.CreateDirectory(logDir);
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Log GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Log(false);
            }

            return _instance;
        }

        public StreamWriter GetStreamWriter()
        {
            return sw;
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public void Close()
        {
            sw.Close();
        }
    }
}
