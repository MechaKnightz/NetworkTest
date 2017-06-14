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
    public abstract class TileBase : ITile
    {
        public TileType Id { get; }
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

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos)
        {
            spriteBatch.Draw(tileset,
                pos,
                new Rectangle((int)Id * Map.TileSize, 0, Map.TileSize, Map.TileSize),
                Color.White);
        }

        public void Write(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)Id);
            outmsg.Write(Health);
        }

        public ITile Read(NetIncomingMessage inc)
        {
            Health = inc.ReadInt32();
            return this;
        }

        public bool Intersects(Rectangle rectangle, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize, Map.TileSize, Map.TileSize);
            return rectangle.Intersects(rect);
        }

        public bool MouseIntersect(float mouseX, float mouseY, int row, int column)
        {
            var rect = new Rectangle(row * Map.TileSize, column * Map.TileSize, Map.TileSize, Map.TileSize);
            return rect.Contains(mouseX, mouseY);
        }

        public void OnLeftClick()
        {
            Health--;
        }

        public void OnTouch(Player player)
        {

        }

        protected TileBase(TileType type, int healthMax)
        {
            Id = type;
            HealthMax = healthMax;
            Health = HealthMax;
            Dirty = false;
        }
    }
}
