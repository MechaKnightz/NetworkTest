using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MapMaker
{
    public static class MouseInput
    {
        public static MouseState mouseState, oldMouseState;

        public static bool IsLeftKeyClicked()
        {
            return mouseState.LeftButton == ButtonState.Pressed &&
                   oldMouseState.LeftButton != ButtonState.Pressed;
        }
    }
}
