using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerGUI
{
    public class LoggerManager
    {

        public ObservableCollection<LogMessage> LogMessages { get; set; }

        public LoggerManager()
        {
            LogMessages = new ObservableCollection<LogMessage>();
        }

        public void AddLogMessage(LogMessage logMessage)
        {
            LogMessages.Add(logMessage);
            if (LogMessages.Count > 1000)
            {
                LogMessages.RemoveAt(0);
            }
        }

        public void AddLogMessage(string id, string message)
        {
            AddLogMessage(new LogMessage(id, message));
        }

        public void AddServerLogMessage(string message)
        {
            AddLogMessage("Server", message);
        }
    }
}