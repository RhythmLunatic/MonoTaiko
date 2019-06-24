using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTaiko.Global.Sprite
{
    class SpriteAnimation
    {
        public Texture2D spriteSheet;
        Vector2 spriteDimensions;
        public Rectangle[] spriteCoords;
        Vector2 sheetGridDimensions;

        public int frames;
        public float interval;
        public bool looping;

        public SpriteAnimation(Texture2D spriteSheet, Vector2 spriteDimensions, Vector2 sheetGridDimensions, int frames, int framerate, bool looping)
        {
            this.spriteSheet = spriteSheet;
            this.spriteDimensions = spriteDimensions;
            this.sheetGridDimensions = sheetGridDimensions;
            this.frames = frames;
            this.interval = 1000f / framerate;
            this.looping = looping;
        }

        public SpriteAnimation(Texture2D spriteSheet, Vector2 spriteDimensions, Vector2 sheetGridDimensions, int frames, float interval, bool looping)
        {
            this.spriteSheet = spriteSheet;
            this.spriteDimensions = spriteDimensions;
            this.sheetGridDimensions = sheetGridDimensions;
            this.frames = frames;
            this.interval = interval;
            this.looping = looping;

            Calculate();
        }

        private void Calculate()
        {
            spriteCoords = new Rectangle[frames];

            int index = 0;

            for (int y = 0; y < sheetGridDimensions.Y; y++)
            {
                for (int x = 0; x < sheetGridDimensions.X; x++)
                {
                    if (index > frames) break;

                    spriteCoords[index] = new Rectangle((int)spriteDimensions.X * x,
                                                        (int)spriteDimensions.Y * y,
                                                        (int)spriteDimensions.X,
                                                        (int)spriteDimensions.Y);

                    index++;
                }
            }
        }
    }
}
