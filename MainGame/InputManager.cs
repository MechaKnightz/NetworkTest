using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class InputManager
    {
        private NetManager _netManager;

        private KeyboardState _keyState, _oldKeyState;

        public InputManager(NetManager netManager)
        {
            _netManager = netManager;
        }

        public void Update()
        {
            _keyState = Keyboard.GetState();

            CheckKeyState(Keys.W);
            CheckKeyState(Keys.A);
            CheckKeyState(Keys.S);
            CheckKeyState(Keys.D);
            CheckKeyState(Keys.Space);

            _oldKeyState = Keyboard.GetState();
        }

        private void CheckKeyState(Keys key)
        {
            if (_keyState.IsKeyDown(key))
            {
                _netManager.SendInput(key);
            }
        }
    }
}
