using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GazeClick
{
    class Log
    {
        private static StreamWriter sw;

        public Log()
        {
            String logDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Telezdrowie logs\\";

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            String logPath = logDir + GetTimestamp(DateTime.Now) + ".txt";

            sw = new StreamWriter(logPath);
        }

        public static StreamWriter getStreamWriter()
        {
            return sw;
        }

        public static void writeLine(string line)
        {
            sw.WriteLine(line);
        }

        public static void copyTo(String exerciseFilePath, String exerciseFileName)
        {
            close();
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public static void close()
        {
            sw.Close();
        }
    }
}
