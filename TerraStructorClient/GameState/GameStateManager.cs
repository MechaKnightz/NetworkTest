using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraStructorClient.GameState
{
    public static class GameStateManager
    {
        private static IGameState _currentGameState;
        private static IGameState _oldGameState;

        public static void Update(GameTime gameTime)
        {
            _currentGameState.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _currentGameState.Draw(spriteBatch, gameTime);
        }

        public static void ChangeState(IGameState state)
        {
            _oldGameState = _currentGameState;
            _oldGameState?.OnChangeFromState();

            _currentGameState = state;
            _currentGameState?.OnChangeToState();
        }
    }
}
