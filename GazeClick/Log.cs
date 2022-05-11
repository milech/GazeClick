using System;
using System.IO;

namespace GazeClick
{
    internal class Log
    {
        private static StreamWriter sw;

        public Log()
        {
            string logDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\GazeClick logs\\";

            if (!Directory.Exists(logDir))
            {
                _ = Directory.CreateDirectory(logDir);
            }

            string logPath = logDir + GetTimestamp(DateTime.Now) + ".txt";

            sw = new StreamWriter(logPath);
        }

        public static StreamWriter GetStreamWriter()
        {
            return sw;
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public static void Close()
        {
            sw.Close();
        }
    }
}
