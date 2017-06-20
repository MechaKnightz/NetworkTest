using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TerraStructorClient
{
    public class InputManager
    {
        private NetManager _netManager;

        public KeyboardState KeyState, OldKeyState;
        private MouseState _mouseState;

        public InputManager(NetManager netManager)
        {
            _netManager = netManager;
        }

        public void Update(Camera2D camera)
        {
            _mouseState = Mouse.GetState();

            CheckKeyState(Keys.W);
            CheckKeyState(Keys.A);
            CheckKeyState(Keys.S);
            CheckKeyState(Keys.D);
            CheckKeyState(Keys.Space);

            if (!KeyState.IsKeyDown(Keys.Space) && OldKeyState.IsKeyDown(Keys.Space))
                _netManager.SendJumpCancel();

            var mousePos = camera.ScreenToWorld(_mouseState.X, _mouseState.Y);

            if (_mouseState.LeftButton == ButtonState.Pressed)
                _netManager.SendMouseInput(MouseButton.Left, mousePos.X, mousePos.Y);
            if (_mouseState.RightButton == ButtonState.Pressed)
                _netManager.SendMouseInput(MouseButton.Right, mousePos.X, mousePos.Y);
            if (_mouseState.MiddleButton == ButtonState.Pressed)
                _netManager.SendMouseInput(MouseButton.Middle, mousePos.X, mousePos.Y);
        }

        private void CheckKeyState(Keys key)
        {
            if (KeyState.IsKeyDown(key))
            {
                _netManager.SendInput(key);
            }
        }

        public bool JoinRoom(string roomName, out string msg)
        {
            return _netManager.SendJoinRoomInput(roomName, out msg);
        }

        public bool IsKeyClicked(Keys key)
        {
            return KeyState.IsKeyDown(key) && !OldKeyState.IsKeyDown(key);
        }
    }
}
