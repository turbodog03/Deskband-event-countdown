using System;
using System.IO;

namespace eventCountDown
{
    class LogHelper
    {
        private readonly string LogFile;
        private readonly string LogHeader;

        /// <summary>
        /// Helper class for logging
        /// </summary>
        /// <param name="LogPath">the folder path which will contain the log file</param>
        /// <param name="LogFileName">name of the output log File (will be created if not exists)</param>
        /// <param name="LogHeader">an custom string that display in the head of each log. can be class name or whatever, can be empty string</param>
        public LogHelper(string LogPath, string LogFileName, string LogHeader)
        {
            this.LogFile = Path.Combine(LogPath, LogFileName);
            this.LogHeader = LogHeader;
            InitLog();
        }

        private void LogText(string content)
        {
            string message = $"{DateTime.Now}: {LogHeader}: {content} \n";
            File.AppendAllText(LogFile, message);
        }

        private void InitLog()
        {
            LogInfo("LogHelper started");
        }

        public void LogInfo(string content)
        {
            if (Global.LOG_LEVELS.INFO >= Global.LOG_LEVEL)
            {
                string message = $"INFO：{content}";
                LogText(message);
            }
        }

        public void LogError(Exception ex)
        {
            if (Global.LOG_LEVELS.ERROR >= Global.LOG_LEVEL)
            {
                string message = $"ERROR：Exception caught : {ex}";
                LogText(message);
            }
        }


        public void LogError(string error)
        {
            if (Global.LOG_LEVELS.ERROR >= Global.LOG_LEVEL)
            {
                string message = $"ERROR: {error}";
                LogText(message);
            }
        }


    }
}
