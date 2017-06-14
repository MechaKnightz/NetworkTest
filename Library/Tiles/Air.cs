using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Air : ITile
    {
        public TileType Id { get; }
        public int Health { get; set; }
        public int HealthMax { get; }
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

        public bool Intersects(Rectangle rectangle, int row, int column)
        {
            return false;
        }

        public bool MouseIntersect(float mouseX, float mouseY, int row, int column)
        {
            var rect = new Rectangle(row, column, Map.TileSize, Map.TileSize);
            return rect.Contains(mouseX, mouseY);
        }

        public void OnLeftClick()
        {
            
        }

        public Air()
        {
            Id = TileType.Air;
            Health = 1;
            HealthMax = 1;
            Dirty = false;
        }
        public void OnTouch(Player player)
        {

        }
    }
}
