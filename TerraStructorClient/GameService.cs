using GameUILibrary;
using Microsoft.Xna.Framework;

namespace TerraStructorClient
{
    public class GameService : IGameService
    {
        private Game1 Game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="gameInstance">The game instance.</param>
        public GameService(Game1 gameInstance)
        {
            Game = gameInstance;
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        public void Exit()
        {
            Game?.Exit();
        }

        public bool Connect(string username, string password, string ip, string port)
        {
            return Game.Connect(username, password, ip, port);
        }

        public bool JoinRoom(string roomName)
        {
            return Game.JoinRoom(roomName);
        }

        public bool Register(string username, string password, string ip, string port)
        {
            return Game.Register(username, password, ip, port);
        }
    }
}
