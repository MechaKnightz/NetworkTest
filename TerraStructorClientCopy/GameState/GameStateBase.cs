using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraStructorClient.GameState
{
    public class GameStateBase : IGameState
    {
        public Game1 Game { get; }

        protected GameStateBase(Game1 game)
        {
            Game = game;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public void OnChangeToState()
        {
            
        }

        public void OnChangeFromState()
        {
            
        }
    }
}
