using System;
using System.ComponentModel;

namespace ServerGUI.ServerLogger
{
    public class LogMessage : INotifyPropertyChanged
    {
        public LogMessage(string id, string message)
        {
            Id = id;
            Message = message;
            Timestamp = DateTime.Now.TimeOfDay;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TimeSpan _timestamp;
        public TimeSpan Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }
        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
    }
}
