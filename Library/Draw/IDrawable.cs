using System;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Draw
{
    public interface IDrawable
    {
        DateTime Created { get; set; }
        TimeSpan Duration { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
