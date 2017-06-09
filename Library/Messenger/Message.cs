using System;
using Lidgren.Network;

namespace Library.Messenger
{
    public class Message
    {
        public Message () { }
        public Message(string text, string sender)
        {
            Sender = sender;
            Text = text;
            Timestamp = DateTime.Now;
        }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public string GetMessage()
        {
            var hourMinute = Timestamp.ToString("HH:mm");
            var fullMessage = hourMinute + " - " + Sender + ": " + Text;
            return fullMessage;
        }
    }
}
