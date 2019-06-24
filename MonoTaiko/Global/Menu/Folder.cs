using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoTaiko.Global.Menu
{
    class Folder : MenuItem
    {
        List<MenuItem> subitems;

        public Folder(string title, Menu.Categories category) : base(title, category) { }

        public Folder(string title, List<MenuItem> subitems, Menu.Categories category) : base(title, category)
        {
            this.subitems = subitems;
        }

        public void Add(MenuItem item)
        {
            subitems.Add(item);
        }
    }
}
