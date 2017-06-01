using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;

namespace MapMaker.Tiles
{
    public interface ITile
    {
        float Id { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void Write(NetOutgoingMessage outmsg);
        void Read(NetIncomingMessage inc);
    }
}
