﻿using Microsoft.Xna.Framework.Graphics;

namespace Library.Item
{
    public interface IItem : IDrawable
    {
        int StackableCount { get; set; }
        bool Dirty { get; set; }
        void Use(float mouseX, float mouseY, Player player);

    }
}
