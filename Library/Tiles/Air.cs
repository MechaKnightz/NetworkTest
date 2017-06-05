using System;
using Lidgren.Network;
using MapMaker.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Air : ITile
    {
        public float Id { get; set; }
        public void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos)
        {
            //spriteBatch.Draw(tileset,
            //    pos,
            //    new Rectangle((int)Id * Map.TileSize, 0, Map.TileSize, Map.TileSize),
            //    Color.White);
        }

        public void Write(NetOutgoingMessage outmsg)
        {
            outmsg.Write(Id);
        }

        public ITile Read(NetIncomingMessage inc)
        {
            Id = inc.ReadFloat();

            return this;
        }

        public Rectangle GetCollisionRectangle(int x, int y)
        {
            return Rectangle.Empty;
        }

        public Rectangle GetClickRectangle()
        {
            return new Rectangle(0, 0, Map.TileSize, Map.TileSize);
        }

        public void OnClick()
        {
            throw new NotImplementedException();
        }

        public Air()
        {
            Id = 0;
        }
    }
}
