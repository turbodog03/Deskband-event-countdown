using System;
using System.IO;

namespace eventCountDown
{
    class LogHelper
    {
        private readonly string LogFile;

        public LogHelper(string LogPath)
        {
            this.LogFile = Path.Combine(LogPath, "EventCountDown.log");
            InitLog();
        }

        private void InitLog()
        {
            LogInfo("LogHelper started");
        }

        public void LogInfo(string content)
        {
            // Create a text line with the current date and the message
            // string logLine = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + content + "\n";
            string message = $"INFO：{DateTime.Now}: {content} \n";
            File.AppendAllText(LogFile, message);
        }

        public void LogError(Exception ex)
        {
            string message = $"ERROR：Exception caught at {DateTime.Now}: {ex} \n";
            File.AppendAllText(LogFile, message);
        }

        public void LogError(string error)
        {
            string message = $"ERROR：{DateTime.Now}: {error} \n";
            File.AppendAllText(LogFile, message);
        }


    }
}
