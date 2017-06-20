using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraStructorClient.GameState
{
    public class MainMenuGameState : GameStateBase, IGameState
    {

        public MainMenuGameState(Game1 game) : base(game)
        {
            
        }


        public new void Update(GameTime gameTime)
        {
            Game._mainMenuRoot.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
            Game._mainMenuRoot.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public new void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Red);
            Game._mainMenuRoot.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
