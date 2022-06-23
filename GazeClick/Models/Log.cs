// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - class for logging gaze positions and timestamps
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace GazeClick.Models
{
    internal class Log : INotifyPropertyChanged
    {
        private static Log _instance;
        private MyConfig _config;
        private bool _isOn;
        private StreamWriter _sw;
        private static readonly object _lock = new object();
        private const string _standardLogEntry = "{0:0.0}\t{1:0.0}\t{2:MM/dd/yy H:mm:ss fffffff}\t{3:0}";

        private Log(MyConfig config)
        {
            _config = config;
            _isOn = config.IsRegistering;
            LogDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GazeClick logs\\";
            LogPath = LogDir + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
        }

        public static Log GetInstance(MyConfig config)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Log(config);
                    }
                }
            }
            return _instance;
        }

        public bool IsOn    // referenced via Binding in MainWindow.xaml
        {
            get => _isOn;
            set
            {
                try
                {
                    _isOn = value;
                    _config.IsRegistering = value;
                    OnPropertyChanged("IsOn");
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        public string LogDir { get; private set; }

        public string LogPath { get; private set; }

        public string StandardLogEntry => _standardLogEntry;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (_config.IsRegistering)
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
            if (_config.IsRegistering && _sw != null)
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
