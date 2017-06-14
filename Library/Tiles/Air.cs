using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Air : TileBase, ITile
    {
        public new void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos)
        {
            //spriteBatch.Draw(tileset,
            //    pos,
            //    new Rectangle((int)Id * Map.TileSize, 0, Map.TileSize, Map.TileSize),
            //    Color.White);
        }

        public new void Write(NetOutgoingMessage outmsg)
        {
            outmsg.Write((byte)Id);
        }

        public new ITile Read(NetIncomingMessage inc)
        {
            return this;
        }

        public new bool Intersects(Rectangle rectangle, int row, int column)
        {
            return false;
        }

        public new void OnLeftClick()
        {
            
        }

        public Air() : base(TileType.Air, 1)
        {
        }
    }
}
