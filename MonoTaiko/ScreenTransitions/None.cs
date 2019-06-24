using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTaiko.ScreenTransitions
{
    class None : Transition
    {
        public None(GraphicsDevice gd, SpriteBatch sb) : base(gd, sb) { }

        public override void Draw(GameTime gameTime) { }

        public override bool Update(GameTime gameTime) { return false; }
    }
}
