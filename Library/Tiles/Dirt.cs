using System;
using Lidgren.Network;
using MapMaker.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Dirt : ITile
    {
        public TileType Id { get; set; }
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
            Id = (TileType)inc.ReadByte();
            Health = inc.ReadInt32();
            return this;
        }

        public Dirt()
        {
            Id = TileType.Dirt;
            Health = 10;
            Dirty = false;
        }

        public Rectangle GetCollisionRectangle(int x, int y)
        {
            return new Rectangle(x, y, Map.TileSize, Map.TileSize);
        }

        public Rectangle GetClickRectangle(int x, int y)
        {
            return new Rectangle(x, y, Map.TileSize, Map.TileSize);
        }

        public void OnLeftClick()
        {
            Health--;
        }

        public void OnTouch(Player player)
        {
            
        }
    }
}
