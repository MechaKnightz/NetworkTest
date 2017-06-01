using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace MapMaker.Tiles
{
    public interface ITile
    {
        float Id { get; set; }
        void Draw(SpriteBatch spriteBatch, Texture2D tileset, Vector2 pos);
        void Write(NetOutgoingMessage outmsg);
        ITile Read(NetIncomingMessage inc);
    }
}
