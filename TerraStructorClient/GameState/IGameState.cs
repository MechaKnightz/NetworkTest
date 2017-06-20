using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraStructorClient.GameState
{
    public interface IGameState
    {
        Game1 Game { get; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        void OnChangeToState();
        void OnChangeFromState();
    }
}
