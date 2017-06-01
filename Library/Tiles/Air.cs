using Lidgren.Network;
using MapMaker.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Tiles
{
    public class Air : ITile
    {
        public float Id { get; set; }
        public void Draw(SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public void Write(NetOutgoingMessage outmsg)
        {
            throw new System.NotImplementedException();
        }

        public void Read(NetIncomingMessage inc)
        {
            inc.ReadFloat();
        }

        public Air()
        {
            Id = 0;
        }
    }
}
