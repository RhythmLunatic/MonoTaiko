using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoTaiko.Global
{
    public class Input
    {
        public KeyboardState keyState;
        public KeyboardState oldKeyState;

        public GamePadState buttonState;
        public GamePadState oldButtonState;

        public void Update()
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();

            oldButtonState = buttonState;
            buttonState = GamePad.GetState(PlayerIndex.One);
        }

        public bool Start()
        {
            return keyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter)
                   || buttonState.Buttons.Start == ButtonState.Pressed && oldButtonState.Buttons.Start == ButtonState.Released;
        }

        public bool Back()
        {
            return keyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape)
                   || buttonState.Buttons.Back == ButtonState.Pressed && oldButtonState.Buttons.Back == ButtonState.Released;
        }

        public bool LRim()
        {
            return (keyState.IsKeyDown(Keys.D) && oldKeyState.IsKeyUp(Keys.D)
                || buttonState.DPad.Left == ButtonState.Pressed && oldButtonState.DPad.Left == ButtonState.Released
                || buttonState.DPad.Up == ButtonState.Pressed && oldButtonState.DPad.Up == ButtonState.Released
                || buttonState.ThumbSticks.Left.X < -0.30 && oldButtonState.ThumbSticks.Left.X > -0.30
                || buttonState.ThumbSticks.Left.Y > 0.30 && oldButtonState.ThumbSticks.Left.Y < 0.30);
        }

        public bool RRim()
        {
            return (keyState.IsKeyDown(Keys.K) && oldKeyState.IsKeyUp(Keys.K)
                || buttonState.Buttons.B == ButtonState.Pressed && oldButtonState.Buttons.B == ButtonState.Released
                || buttonState.Buttons.Y == ButtonState.Pressed && oldButtonState.Buttons.Y == ButtonState.Released
                || buttonState.ThumbSticks.Right.Y > 0.30 && oldButtonState.ThumbSticks.Right.Y < 0.30
                || buttonState.ThumbSticks.Right.X > 0.30 && oldButtonState.ThumbSticks.Right.X < 0.30);
        }

        public bool LCenter()
        {
            return (keyState.IsKeyDown(Keys.F) && oldKeyState.IsKeyUp(Keys.F)
                || buttonState.DPad.Down == ButtonState.Pressed && oldButtonState.DPad.Down == ButtonState.Released
                || buttonState.DPad.Right == ButtonState.Pressed && oldButtonState.DPad.Right == ButtonState.Released
                || buttonState.ThumbSticks.Left.X > 0.30 && oldButtonState.ThumbSticks.Left.X < 0.30
                || buttonState.ThumbSticks.Left.Y < -0.30 && oldButtonState.ThumbSticks.Left.Y > -0.30);
        }

        public bool RCenter()
        {
            return (keyState.IsKeyDown(Keys.J) && oldKeyState.IsKeyUp(Keys.J)
                || buttonState.Buttons.A == ButtonState.Pressed && oldButtonState.Buttons.A == ButtonState.Released
                || buttonState.Buttons.X == ButtonState.Pressed && oldButtonState.Buttons.X == ButtonState.Released
                || buttonState.ThumbSticks.Right.Y < -0.30 && oldButtonState.ThumbSticks.Right.Y > -0.30
                || buttonState.ThumbSticks.Right.X < -0.30 && oldButtonState.ThumbSticks.Right.X > -0.30);
        }
    }
}
