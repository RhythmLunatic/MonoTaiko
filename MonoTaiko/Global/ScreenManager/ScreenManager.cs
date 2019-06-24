using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTaiko.ScreenTransitions;

namespace MonoTaiko.Global.ScreenManager
{
    public class ScreenManager : DrawableGameComponent
    {
        public enum Transitions
        {
            None,
            FadeIn,
            FadeOut
        }

        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        List<Screen> screensToDraw = new List<Screen>();

        IGraphicsDeviceService graphicsDeviceService;

        GraphicsDeviceManager graphics;

        ContentManager content;
        SpriteBatch spriteBatch;
        Texture2D blankTexture;
        Rectangle titleSafeArea;

        public Global.Input input;

        bool traceEnabled;

        new public Game Game
        {
            get { return base.Game; }
        }

        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }

        public ContentManager Content
        {
            get { return content; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        public Rectangle TitleSafeArea
        {
            get { return titleSafeArea; }
        }

        public ScreenManager(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services, "Content");

            graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(
                                                        typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //generate blank texture for fade ins/outs
            blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.Black;
            blankTexture.SetData(data);

            input = new Global.Input();

            // Tell each of the screens to load their content.
            foreach (Screen screen in screens)
            {
                screen.LoadContent();
            }

            // update the title-safe area
            titleSafeArea = new Rectangle(
                (int)Math.Floor(GraphicsDevice.Viewport.X +
                   GraphicsDevice.Viewport.Width * 0.05f),
                (int)Math.Floor(GraphicsDevice.Viewport.Y +
                   GraphicsDevice.Viewport.Height * 0.05f),
                (int)Math.Floor(GraphicsDevice.Viewport.Width * 0.9f),
                (int)Math.Floor(GraphicsDevice.Viewport.Height * 0.9f));
        }

        protected override void UnloadContent()
        {
            // Unload content belonging to the screen manager.
            content.Unload();

            // Tell each of the screens to unload their content.
            foreach (Screen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others
            // (or it happens on another thread)
            screensToUpdate.Clear();

            foreach (Screen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input and update presence.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (Screen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of drawing one screen adds or removes others
            // (or it happens on another thread)
            screensToDraw.Clear();

            foreach (Screen screen in screens)
                screensToDraw.Add(screen);

            foreach (Screen screen in screensToDraw)
            {
                if (screen.ScreenState == ScreenState.Exited)
                    continue;

                screen.Draw(gameTime);
                screen.DrawTransition(gameTime);
            }
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            //SpriteBatch.Begin();
            // We changed this to be Opaque
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null);
            SpriteBatch.Draw(blankTexture, rectangle, color);
            SpriteBatch.End();
        }

        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;

            // If we have a graphics device, tell the screen to load content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                screen.LoadContent();
            }

            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
            screensToDraw.Remove(screen);
        }

        public void ExitCurrentScreen()
        {
            screens[0].ExitScreen();
        }

        public Screen[] GetScreens()
        {
            return screens.ToArray();
        }

        public Transition getTransition(Transitions transition)
        {
            var type = Type.GetType(typeof(Transition).Namespace + "." + transition.ToString(), throwOnError: false);

            if (type == null)
                throw new InvalidOperationException(transition.ToString() + "is not a known Transition type.");

            if (!typeof(Transition).IsAssignableFrom(type))
                throw new InvalidOperationException(type.Name + "does not inherit from Transition.");

            return (Transition)Activator.CreateInstance(type, GraphicsDevice, SpriteBatch);
        }
    }
}