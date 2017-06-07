using Library;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace MainGame
{
    public class InputManager
    {
        private NetManager _netManager;

        private KeyboardState _keyState;
        private MouseState _mouseState;

        public InputManager(NetManager netManager)
        {
            _netManager = netManager;
        }

        public void Update(Camera2D camera)
        {
            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            CheckKeyState(Keys.W);
            CheckKeyState(Keys.A);
            CheckKeyState(Keys.S);
            CheckKeyState(Keys.D);
            CheckKeyState(Keys.Space);

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
            if (_keyState.IsKeyDown(key))
            {
                _netManager.SendInput(key);
            }
        }

        public bool JoinRoom(string roomName, out string msg)
        {
            return _netManager.SendJoinRoomInput(roomName, out msg);
        }
    }
}
