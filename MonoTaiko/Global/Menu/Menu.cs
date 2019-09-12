using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NAudio.Wave;
using Microsoft.Xna.Framework.Audio;

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

        WaveStream katSfx;

        List<MenuItem> items;

        int selected = 0;
        int next = 1;

        public Menu(ContentManager content, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.spriteBatch = spriteBatch;

            items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            katSfx = new WaveFileReader("Content/SFX/Global/kat.wav");

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

        public void Update(GameTime gameTime, Input input, Jukebox sfx, SoundEffect kat)
        {
            if (input.LRim())
            {
                selected--;
                kat.Play();
            }     
            if (input.RRim())
            {
                selected++;
                kat.Play();
            }

            if (selected >= items.Count) selected = 0;
            if (selected < 0) selected = items.Count - 1;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(itemTextures[selected], new Vector2(600, 105), Color.White);

            if (items[selected].GetType() == typeof(Folder))
                spriteBatch.Draw(folderTextures[selected], new Vector2(600, 91), Color.White);
        }
    }
}
