using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.PopupHandler
{
    public static class MessageHandler
    {

        private static Texture2D _boxTexture;
        private static readonly List<Message> Messages = new List<Message>();
        private static Vector2 Origin { get; set; }
        private static SpriteFont Font { get; set; }
        private static Color Color { get; set; }

        private static Vector2 _padding = new Vector2(10, 10);

        public static void Initialize(Texture2D boxTexture, SpriteFont font, Color color)
        {
            _boxTexture = boxTexture;
            Font = font;
            Color = color;
            Origin = new Vector2(_boxTexture.Width / 2f, _boxTexture.Height / 2f);
        }

        public static void Update(GameTime currentTime)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].AppearTime + Messages[i].Age <= currentTime.TotalGameTime)
                {
                    Messages.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void CreateMessage(Message message)
        {
            Messages.Add(message);
        }

        public static void CreateMessage(string text, GameTime gameTime)
        {
            CreateMessage(text, gameTime, Font);
        }

        public static void CreateMessage(string text, GameTime gameTime, SpriteFont font)
        {
            var message = new Message(text, font, gameTime.TotalGameTime);
            Messages.Add(message);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var message in Messages)
            {
                DrawMessage(message, spriteBatch);
            }
        }

        private static void DrawMessage(Message message, SpriteBatch spriteBatch)
        {
            var lineAmount = (_boxTexture.Height - _padding.Y * 2) / message.Font.MeasureString(message.Text).Y;
            var lines = WrapText(message.Text, _boxTexture.Width - _padding.X * 2, message.Font);

            Vector2 boxPos = new Vector2(message.Position.X, 
                message.Position.Y);
            Vector2 firstLinePos  = boxPos + _padding - Origin;

            spriteBatch.Draw(_boxTexture, boxPos, origin: Origin);

            for (int i = 0; i < lineAmount; i++)
            {
                if (i >= lines.Count) break;
                var drawPos = firstLinePos + new Vector2(0, message.Font.MeasureString(lines[0]).Y) * i;
                spriteBatch.DrawString(message.Font, lines[i], drawPos, Color);
            }
        }

        private static List<string> WrapText(string text, float length, SpriteFont font)
        {
            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            var linewidth = 0f;
            var spaceWidth = font.MeasureString(" ").X;
            int curLine = 0;
            lines.Add(string.Empty);
            foreach (string word in words)
            {
                var size = font.MeasureString(word);
                if (linewidth + size.X < length)
                {
                    lines[curLine] += word + " ";
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    lines.Add(word + " ");
                    linewidth = size.X + spaceWidth;
                    curLine++;
                }
            }
            for (int i = 0; i < lines.Count; i++)
            {
                {
                    while (true)
                    {
                        if (font.MeasureString(lines[i]).X > length)
                            lines[i] = lines[i].Remove(lines[i].Length - 1);
                        else break;
                    }
                }
            }
            return lines;
        }
    }
}
