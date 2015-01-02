using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Menus
{
    public class IntroMenu
    {
        //states the menu can be in.
        private enum MenuState
        {
            Title,
            Options,
            Load
        }

        //Stores which state the menu is currently in
        private MenuState menuState;
        //the current menu being used.
        private Menu currentMenu;
        //content manager passed from the Game1 class to load in assets.
        private ContentManager cm;

        /// <summary>
        /// Default Constructor, sets initial game state and menu
        /// </summary>
        /// <param name="cm">Content Manager to be used by all menus.</param>
        public IntroMenu(ContentManager cm)
        {
            menuState = MenuState.Title;
            currentMenu = new TitleScreen();
            this.cm = cm;
        }

        /// <summary>
        /// Load the current menu
        /// </summary>
        public void loadIntroMenus()
        {
            currentMenu.loadMenu(cm);
        }

        /// <summary>
        /// update the current menu
        /// </summary>
        /// <param name="gt">gametime to be used for potential timing events.</param>
        public void updateIntroMenus(GameTime gt)
        {
            if (currentMenu.MenuTransition == true)
            { //move forward through the menus.
                switch (menuState)
                {
                    case MenuState.Title:
                        if (currentMenu.NextMenu == 1)
                        { //go to the options screen
                            menuState = MenuState.Options;
                            currentMenu = new OptionsScreen();
                            loadIntroMenus();
                        }
                        else if (currentMenu.NextMenu == 2)
                        { //go to the save/load screen
                            menuState = MenuState.Load;
                            currentMenu = new Save_LoadScreen();
                            loadIntroMenus();
                        }
                        else if (currentMenu.NextMenu == 3)
                        {
                            //go to new game state for new game
                            GameLogic.GameGlobal.CurrentGS = GameLogic.GameState.NewGame;
                            Game1.newState = true;
                        }
                        break;
                    default: //no menu past these
                    case MenuState.Options:
                    case MenuState.Load:
                        break;
                }
            }
            else if (currentMenu.MenuTransition == false)
            { //move back through the menus
                switch (menuState)
                {
                    default:
                    case MenuState.Title:
                        //close the game if already at the title screen.
                        Game1.closeTrigger = true;
                        break;
                    //if in the options or load screen, go back to the title screen.
                    case MenuState.Options:
                    case MenuState.Load:
                        menuState = MenuState.Title;
                        currentMenu = new TitleScreen();
                        loadIntroMenus();
                        break;
                }
            }

            //update the current menu if there is no game state switching
            currentMenu.updateMenu(gt);
        }

        /// <summary>
        /// draw the current menu
        /// </summary>
        /// <param name="sb">spritebatch object to draw assets.</param>
        /// <param name="gt">gametime object ot be used for timing events.</param>
        public void drawIntroMenus(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            currentMenu.drawMenu(sb, gt);

            sb.End();
        }
    }
}
