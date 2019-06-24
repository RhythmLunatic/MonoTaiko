using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTaiko.Global.Sprite
{
    class SpriteAnimator
    {
        SpriteBatch spriteBatch;

        Dictionary<String, SpriteAnimation> spriteAnimations;

        SpriteAnimation animation;

        int curFrame = 0;

        float timer;

        string curAnim;

        public SpriteAnimator(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            spriteAnimations = new Dictionary<string, SpriteAnimation>();
        }

        public void Add(string name, SpriteAnimation spranim)
        {
            spriteAnimations.Add(name, spranim);
        }

        public void Switch(string name)
        {
            if (spriteAnimations.ContainsKey(name))
            {
                curAnim = name;
                curFrame = 0;
                spriteAnimations.TryGetValue(curAnim, out animation);
            } 
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timer >= animation.interval)
            {
                curFrame++;

                if (curFrame >= animation.frames)
                {
                    if (animation.looping)
                        curFrame = 0;
                    else
                        curFrame--;
                }

                timer -= animation.interval;
            }
        }

        public void Draw(GameTime gameTime, Vector2 pos)
        {
            spriteBatch.Draw(animation.spriteSheet, pos, animation.spriteCoords[curFrame], Color.White);
        }
    }
}
