using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Slab : TileBase, ITile
    {
        public new TileType Id { get; } = TileType.Slab;

        public new bool Intersects(Rectangle rectangle, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize, Map.TileSize, Map.TileSize / 2);
            return rectangle.Intersects(rect);
        }

        public new bool MouseIntersect(float mouseX, float mouseY, int row, int column)
        {
            var rect = new Rectangle(row, column, Map.TileSize, Map.TileSize / 2);
            return rect.Contains(mouseX, mouseY);
        }
    }
}
