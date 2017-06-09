using Library;
using Library.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace MapMaker.Tiles
{
    public interface ITile
    {
        TileType Id { get; set; }
        int Health { get; set; }
        bool Dirty { get; set; }
        void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos);
        void Write(NetOutgoingMessage outmsg);
        ITile Read(NetIncomingMessage inc);
        Rectangle GetCollisionRectangle(int x, int y);
        Rectangle GetClickRectangle(int x, int y);
        void OnLeftClick();
        void OnTouch(Player player);
    }
}
