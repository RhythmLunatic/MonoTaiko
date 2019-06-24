using Microsoft.Xna.Framework;
using MonoTaiko.ScreenTransitions;


namespace MonoTaiko.Global.ScreenManager
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Exited,
    }

    public abstract class Screen
    {
        bool isPopup = false;

        ScreenState screenState = ScreenState.TransitionOn;

        bool isExiting = false;

        ScreenManager.Transitions transition1;
        ScreenManager.Transitions transition2;

        public Transition transitionIn;
        public Transition transitionOut;

        public Screen nextScreen;

        public Screen(ScreenManager.Transitions transitionIn, ScreenManager.Transitions transitionOut)
        {
            transition1 = transitionIn;
            transition2 = transitionOut;
        }

        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }

        public bool IsExiting
        {
            get { return isExiting; }
            protected set { isExiting = value; }
        }

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        bool otherScreenHasFocus;

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        public virtual void LoadContent()
        {
            transitionIn = ScreenManager.getTransition(transition1);
            transitionOut = ScreenManager.getTransition(transition2);
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                screenState = ScreenState.TransitionOff;

                if(UpdateTransition(gameTime))
                {
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    screenState = ScreenState.Exited;
                    isExiting = false;

                    ScreenManager.AddScreen(nextScreen);

                    ExitScreen();
                }
            }
            else
            {
                if (UpdateTransition(gameTime))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
            }
        }

        public bool UpdateTransition(GameTime gameTime)
        {
            if (screenState == ScreenState.TransitionOn)
            {
                return transitionIn.Update(gameTime);
            }

            if (screenState == ScreenState.TransitionOff)
            {
                return transitionOut.Update(gameTime);
            }
                
            return false;
        }

        public void DrawTransition(GameTime gameTime)
        {
            if (screenState == ScreenState.TransitionOn)
                transitionIn.Draw(gameTime);
            if (screenState == ScreenState.TransitionOff)
                transitionOut.Draw(gameTime);
        }

        public virtual void HandleInput(Input input) { }

        public abstract void Draw(GameTime gameTime);

        public virtual void Exit(Screen nextScreen)
        {
            this.nextScreen = nextScreen;
            isExiting = true;
        }

        public virtual void ExitScreen()
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}