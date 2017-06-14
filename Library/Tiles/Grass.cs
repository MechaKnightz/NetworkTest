using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Grass : ITile
    {
        public bool Dirty { get; set; }

        private int _health;
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                Dirty = true;
            }
        }

        public int HealthMax { get; }

        public TileType Id { get; }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos)
        {
            spriteBatch.Draw(tileset,
                pos,
                new Rectangle((int)Id * Map.TileSize, 0, Map.TileSize, Map.TileSize),
                Color.White);
        }

        public Rectangle GetClickRectangle(int x, int y)
        {
            return new Rectangle(x, y, Map.TileSize, Map.TileSize);
        }

        public bool Intersects(Rectangle rectangle, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize, Map.TileSize, Map.TileSize);
            return rectangle.Intersects(rect);
        }

        public void OnLeftClick()
        {
            Health--;
        }

        public void OnTouch(Player player)
        {
            
        }

        public ITile Read(NetIncomingMessage inc)
        {
            Health = inc.ReadInt32();
            return this;
        }

        public void Write(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)Id);
            outmsg.Write(Health);
        }

        public Grass()
        {
            Id = TileType.Grass;
            Health = 20;
            HealthMax = 20;
            Dirty = false;
        }
    }
}
