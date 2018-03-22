using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TerraStructorClient.GameState
{
    public class MainGameGameState : GameStateBase, IGameState
    {

        public MainGameGameState(Game1 game) : base(game)
        {
        }

        public new void Update(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            Game.timer -= elapsed;
            if (Game.timer < 0)
            {
                Game._netManager.CheckServerMessages(gameTime);
                Game.timer = Game.TIMER;
            }
            if (Game._inputManager.IsKeyClicked(Keys.Enter))
                GameStateManager.ChangeState(new ChatOpenGameState(Game));
            Game.SetCamera(Game._camera);
            Game._inputManager.Update(Game._camera);

            //Game.InputPredictionUpdate(gameTime);
        }

        public new void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.DrawMap(gameTime);
            Player localPlayer = new Player();
            for (int i = 0; i < Game._netManager.CurrentRoom.Players.Count; i++)
            {
                Game.DrawPlayer(Game._netManager.CurrentRoom.Players[i], Color.Red);
                if (Game._netManager.CurrentRoom.Players[i].Username == Game._netManager.Username)
                    localPlayer = Game._netManager.CurrentRoom.Players[i];
            }

            spriteBatch.DrawString(Game._nameFont, new Vector2(localPlayer.X, localPlayer.Y).ToString(), Game._camera.ScreenToWorld(0, 0), Color.White);
            spriteBatch.DrawString(Game._nameFont, gameTime.TotalGameTime.ToString(), Game._camera.ScreenToWorld(0, 50), Color.White);
            spriteBatch.DrawString(Game._nameFont, Game._netManager.CurrentRoom.Name, Game._camera.ScreenToWorld(0, 100), Color.White);

            Game.DrawMessages(spriteBatch, Game._netManager.ChatMessages, Game._camera, this);

            var mouse = Mouse.GetState();
            spriteBatch.Draw(Game._cursorTexture, Game._camera.ScreenToWorld(mouse.X, mouse.Y), Color.White);
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
