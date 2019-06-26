using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SeparatingAxis.Input
{
    public class InputDetector
    {
        private readonly List<Keys> _pressedKeys;
        private readonly List<Keys> _downKeys;

        private KeyboardState _oldKeyboardState;

        private Vector2 _position;

        public InputDetector()
        {
            _oldKeyboardState = Keyboard.GetState();
            _downKeys =  new List<Keys>();
            _pressedKeys =  new List<Keys>();
            _position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _pressedKeys.Contains(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _downKeys.Contains(key);
        }

        public void Update(GameTime gameTime)
        {
            var newKeyboardState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            _downKeys.Clear();
            _pressedKeys.Clear();

            _downKeys.AddRange(newKeyboardState.GetPressedKeys());
            var oldDownKeys = _oldKeyboardState.GetPressedKeys();
            _pressedKeys.AddRange(oldDownKeys.Where(x => !_downKeys.Contains(x)));

            _position = new Vector2(newMouseState.X, newMouseState.Y);
            _oldKeyboardState = newKeyboardState;
        }

        public Vector2 MousePosition()
        {
            return _position;
        }

        public void SetMousePosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }
    }
}
