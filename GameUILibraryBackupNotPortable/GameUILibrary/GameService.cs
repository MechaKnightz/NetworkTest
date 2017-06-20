using Microsoft.Xna.Framework;

namespace GameUILibrary
{
    public class GameService : IGameService
    {
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="gameInstance">The game instance.</param>
        public GameService(Game gameInstance)
        {
            game = gameInstance;
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        public void Exit()
        {
            if (game != null)
            {
                game.Exit();
            }
        }
    }
}
