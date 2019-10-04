using LudumDareTemplate.Input;
using Microsoft.Xna.Framework.Input;
using System;

namespace LudumDareTemplate.Extensions {
    public static class MouseStateExtensions {
        public static bool IsButtonDown(this MouseState mouseState, MouseButtons button) {
            switch (button) {
                case MouseButtons.Left: {
                    return mouseState.LeftButton == ButtonState.Pressed;
                }
                case MouseButtons.Middle: {
                    return mouseState.MiddleButton == ButtonState.Pressed;
                }
                case MouseButtons.Right: {
                    return mouseState.RightButton == ButtonState.Pressed;
                }
                case MouseButtons.X1: {
                    return mouseState.XButton1 == ButtonState.Pressed;
                }
                case MouseButtons.X2: {
                    return mouseState.XButton2 == ButtonState.Pressed;
                }
                default: {
                    throw new ArgumentException();
                }
            }
        }

        public static bool IsButtonUp(this MouseState mouseState, MouseButtons button) {
            return !mouseState.IsButtonDown(button);
        }
    }
}
