using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerraStructorClient.GameState
{
    public class ChatOpenGameState : GameStateBase, IGameState
    {
        private MainGameGameState _mainGameState;

        public ChatOpenGameState(Game1 game) : base(game)
        {
            _mainGameState = new MainGameGameState(game);
        }

        public new void Update(GameTime gameTime)
        {
            Game._chatTextBox.Update(gameTime);
            var elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            Game.timer -= elapsed;
            if (Game.timer < 0)
            {
                Game._netManager.CheckServerMessages(gameTime);
                Game.timer = Game.TIMER;
            }
            Game.SetCamera(Game._camera);
            if (Game._inputManager.IsKeyClicked(Keys.Escape))
                GameStateManager.ChangeState(new MainGameGameState(Game));
            if (Game._inputManager.IsKeyClicked(Keys.Enter))
            {
                Game.ChatMessage();
            }
            Game._chatTextBox.X = Convert.ToInt16(Game._camera.ScreenToWorld(Game._chatPadding.X, 0).X);
            var value = Game._halfScreen.Y * 2 - Game._chatPadding.Y - Game._chatFont.MeasureString("T").Y;
            Game._chatTextBox.Y = Convert.ToInt16(Game._camera.ScreenToWorld(0, value).Y);
        }

        public new void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Game._chatTextBox.Draw(spriteBatch, gameTime);

            {
                _mainGameState.Draw(spriteBatch, gameTime);
            }
        }

        public new void OnChangeToState()
        {
            Game.IsMouseVisible = false;
        }

        public new void OnChangeFromState()
        {
            Game.IsMouseVisible = true;
        }
    }
}
