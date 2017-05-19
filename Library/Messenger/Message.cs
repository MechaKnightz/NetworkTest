using System;

namespace Library.Messenger
{
    public class Message
    {
        public Message () { }
        public Message(string text, string sender)
        {
            Sender = sender;
            Text = text;
            Timestamp = DateTime.Now.TimeOfDay;
        }
        public string Sender { get; set; }
        public string Text { get; set; }
        public TimeSpan Timestamp { get; set; }
    }
}
