using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MonoTaiko.Global.Menu
{
    class Menu
    {
        public enum Categories
        {
            Anime,
            Vocaloid,
            Classic,
            Variety,
            VideoGames,
            Namco,
            JPop,
            Random
        }

        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D[] itemTextures = new Texture2D[Enum.GetNames(typeof(Categories)).Length];
        Texture2D[] folderTextures = new Texture2D[Enum.GetNames(typeof(Categories)).Length];

        List<MenuItem> items;

        public Menu(ContentManager content, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.spriteBatch = spriteBatch;

            items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            for(int i = 0; i < Enum.GetNames(typeof(Categories)).Length; i++)
            {
                itemTextures[i] = content.Load<Texture2D>("Texture/Menu/item_" + i);
                folderTextures[i] = content.Load<Texture2D>("Texture/Menu/folder_" + i);
            }

            for(int i = 0; i < Enum.GetNames(typeof(Categories)).Length; i++)
            {
                Categories cat = (Categories)i;

                items.Add(new Folder(cat.ToString(), cat));
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
