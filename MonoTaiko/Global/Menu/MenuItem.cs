using Microsoft.Xna.Framework;

namespace MonoTaiko.Global.Menu
{
    class MenuItem
    {
        public Menu.Categories category { get; set; }

        public string Title { get; set; }

        public MenuItem(string Title, Menu.Categories category)
        {
            this.Title = Title;
            this.category = category;
        }
    }
}
