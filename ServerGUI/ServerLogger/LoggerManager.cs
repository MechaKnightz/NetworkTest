using System.Collections.ObjectModel;

namespace ServerGUI.ServerLogger
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

        public void ServerMsg(string message)
        {
            AddLogMessage("Server", message);
        }
    }
}