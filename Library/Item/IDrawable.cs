using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Library.Item
{
    public interface IDrawable
    {
        DateTime Created { get; set; }
        TimeSpan Duration { get; set; }

        void Draw(SpriteBatch spriteBatch, List<Texture2D> textureList);
    }
}
