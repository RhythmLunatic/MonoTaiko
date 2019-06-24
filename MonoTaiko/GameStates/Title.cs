using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTaiko.Global;
using MonoTaiko.Global.ScreenManager;

namespace MonoTaiko.GameStates
{
    class Title : Screen
    {
        Texture2D background;
        Texture2D foreground;

        SpriteFont dfp20;

        const float TEXT_FADE_LENGTH = 1f;
        float textAlpha = 1f;
        bool fadeout = true;

        public Title() : base(ScreenManager.Transitions.FadeIn, ScreenManager.Transitions.FadeOut) { }

        public override void LoadContent()
        {
            base.LoadContent();

            background = ScreenManager.Content.Load<Texture2D>("Texture/Title/background");
            foreground = ScreenManager.Content.Load<Texture2D>("Texture/Title/foreground");

            dfp20 = ScreenManager.Content.Load<SpriteFont>("Font/dfp20");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            FadeText(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public void FadeText(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(fadeout)
            {
                textAlpha -= timeDelta / TEXT_FADE_LENGTH;

                if (textAlpha <= 0) fadeout = false;
            }
            else
            {
                textAlpha += timeDelta / TEXT_FADE_LENGTH;

                if (textAlpha >= 1) fadeout = true;
            }
        }

        public override void HandleInput(Input input)
        {
            if(input.Start())
            {
                Exit(new SongSelect());
            }

            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.Draw(foreground, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.DrawString(dfp20, "Press Start to Play-don!", new Vector2(448, 380), Color.White * textAlpha);
            ScreenManager.SpriteBatch.End();
        }
    }
}
