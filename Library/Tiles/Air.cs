using System;
using Lidgren.Network;
using MapMaker.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Air : ITile
    {
        public TileType Id { get; }
        public int Health { get; set; }
        public bool Dirty { get; set; }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos)
        {
            //spriteBatch.Draw(tileset,
            //    pos,
            //    new Rectangle((int)Id * Map.TileSize, 0, Map.TileSize, Map.TileSize),
            //    Color.White);
        }

        public void Write(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)Id);
        }

        public ITile Read(NetIncomingMessage inc)
        {
            return this;
        }

        public Rectangle GetCollisionRectangle(int x, int y)
        {
            return Rectangle.Empty;
        }

        public Rectangle GetClickRectangle(int x, int y)
        {
            return new Rectangle(x, y, Map.TileSize, Map.TileSize);
        }

        public void OnLeftClick()
        {
            
        }

        public Air()
        {
            Id = TileType.Air;
            Health = 1;
            Dirty = false;
        }
        public void OnTouch(Player player)
        {

        }
    }
}
