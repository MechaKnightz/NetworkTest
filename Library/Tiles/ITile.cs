using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public interface ITile
    {
        TileType Id { get; }
        int Health { get; set; }
        int HealthMax { get; }
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
