using Microsoft.Xna.Framework;

namespace Library.Tiles
{
    public class Slab : TileBase, ITile
    {

        public new bool Intersects(Rectangle rectangle, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize + Map.TileSize / 2, Map.TileSize, Map.TileSize / 2);
            return rectangle.Intersects(rect);
        }

        public new bool MouseIntersect(float mouseX, float mouseY, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize + Map.TileSize / 2, Map.TileSize, Map.TileSize / 2);
            return rect.Contains(mouseX, mouseY);
        }

        public Slab() : base(TileType.Slab, 5)
        {
        }
    }
}
