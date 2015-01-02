using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Menus
{
    public abstract class Menu
    {
        private Texture2D background;
        private byte nextMenu;
        private bool? menuTransition = null;

        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }

        public bool? MenuTransition
        {
            get { return menuTransition; }
            set { menuTransition = value; }
        }

        public byte NextMenu
        {
            get { return nextMenu; }
            set { nextMenu = value; }
        }

        public abstract void loadMenu(ContentManager cm);

        public abstract void updateMenu(GameTime gt);

        public abstract void drawMenu(SpriteBatch sb, GameTime gt);
    }
}
