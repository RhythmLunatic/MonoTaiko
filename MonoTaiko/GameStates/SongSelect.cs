using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTaiko.Global;
using MonoTaiko.Global.ScreenManager;
using NAudio.Vorbis;
using NAudio.Wave;
using MonoTaiko.Global.Sprite;
using MonoTaiko.Global.Menu;
using Microsoft.Xna.Framework.Audio;

namespace MonoTaiko.GameStates
{
    class SongSelect : Screen
    {
        Input input;

        SpriteFont dfp20;

        bool playing = false;

        VorbisWaveReader idleIntro;
        VorbisWaveReader chartMusic;

        WaveFileReader idleLoop;
        LoopStream idleLoopStream;

        Texture2D donIdleSheet;
        Texture2D bgMask;
        Texture2D timerBg;
        Texture2D selectSongText;

        SpriteAnimator animator;

        Menu menu;

        SpriteAnimation donIdle;

        Jukebox music;
        Jukebox sfx;
        Jukebox voice;

        SoundEffect kat;

        public SongSelect() : base(ScreenManager.Transitions.None, ScreenManager.Transitions.FadeOut) { }

        public override void LoadContent()
        {
            input = new Input();

            dfp20 = ScreenManager.Content.Load<SpriteFont>("Font/dfp20");

            donIdleSheet = ScreenManager.Content.Load<Texture2D>("Texture/Don/Anim/SongSelect/idle");
            bgMask = ScreenManager.Content.Load<Texture2D>("Texture/SongSelect/bgMask");
            timerBg = ScreenManager.Content.Load<Texture2D>("Texture/SongSelect/timer");
            selectSongText = ScreenManager.Content.Load<Texture2D>("Texture/SongSelect/selectText");

            menu = new Menu(ScreenManager.Content, ScreenManager.SpriteBatch);
            menu.LoadContent();

            animator = new SpriteAnimator(ScreenManager.SpriteBatch);
            donIdle = new SpriteAnimation(donIdleSheet, new Vector2(291, 291), new Vector2(10, 3), 30, 1000f / ((30 * 116) / 60), true);

            animator.Add("idle", donIdle);
            animator.Switch("idle");

            idleLoop = new WaveFileReader("Content/Music/SongSelect/idle.wav");
            idleLoopStream = new LoopStream(idleLoop, 254437);

            music = new Jukebox();
            music.Play(idleLoopStream);

            sfx = new Jukebox();

            kat = ScreenManager.Content.Load<SoundEffect>("kat");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            animator.Update(gameTime);

            input.Update();

            menu.Update(gameTime, input, sfx, kat);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Blue);

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.Draw(bgMask, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.Draw(selectSongText, new Vector2(8, 12), Color.White);

            menu.Draw(gameTime);

            ScreenManager.SpriteBatch.Draw(timerBg, new Vector2(1115, 9), Color.White);

            animator.Draw(gameTime, new Vector2(-20, 342));

            ScreenManager.SpriteBatch.End();
        }
    }
}
