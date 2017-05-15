using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Library
{
    public class Message
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }

        public TimeSpan AppearTime { get; set; }
        public TimeSpan Age { get; set; }

        public Vector2 Position { get; set; }

        public Message(string text, SpriteFont font,  TimeSpan appearTime, TimeSpan age, Vector2 position)
        {
            Text = text;
            Font = font;
            AppearTime = appearTime;
            Age = age;
            Position = position;
        }

        public Message(string text, SpriteFont font, TimeSpan appearTime)
        {
            Text = text;
            Font = font;
            AppearTime = appearTime;
            Age = new TimeSpan(0, 0, 0, 3);

            Position = new Vector2(Screen.PrimaryScreen.Bounds.Width / 2f,
                Screen.PrimaryScreen.Bounds.Height / 2f);
        }
    }
}
