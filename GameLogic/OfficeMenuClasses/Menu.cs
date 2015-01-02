using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storefront.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class Menu
    {
        //Buttons to transition to another view
        SFButton orderBtn, financeBtn, hireBtn, expandBtn, questBtn, ownerBtn, optionsBtn, closeBtn, dataBtn;
        private Rectangle bounds;
        private SubMenuView subMenu;
        private bool? statsScreenClosed = false;
        public static bool subScreenOpen = false;

        public Menu(Rectangle b)
        {
            bounds = b;
        }

        public void loadMenu()
        {
            //init buttons
            int horzSpacing = (int)((bounds.Width * .75) / 3);
            int vertSpacing = (int)(bounds.Height / 3);

            orderBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625),
                (int)(bounds.Top + (bounds.Height * 0.0625)), (int)(horzSpacing * .75), (int)(vertSpacing * .75)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Order", false,
                true, true);

            financeBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625),
                (int)(bounds.Top + (bounds.Height * 0.0625)) + vertSpacing, (int)(horzSpacing * .75),
                (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                Graphics.GlobalGfx.btnActive, "Finance", false, true, true);

            hireBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625),
                (int)(bounds.Top + (bounds.Height * 0.0625)) + (2 * vertSpacing),
                (int)(horzSpacing * .75), (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef,
                Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Employees", false, true, true);

            expandBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + horzSpacing,
                (int)(bounds.Top + (bounds.Height * 0.0625)), (int)(horzSpacing * .75), (int)(vertSpacing * .75)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Expand", false,
                true, true);

            questBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + horzSpacing,
                (int)(bounds.Top + (bounds.Height * 0.0625)) + vertSpacing, (int)(horzSpacing * .75),
                (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                Graphics.GlobalGfx.btnActive, "Log", false, true, true);

            ownerBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + horzSpacing,
                (int)(bounds.Top + (bounds.Height * 0.0625)) + (2 * vertSpacing),
                (int)(horzSpacing * .75), (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef,
                Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Owner", false, true, true);

            optionsBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + (2 * horzSpacing),
                (int)(bounds.Top + (bounds.Height * 0.0625)), (int)(horzSpacing * .75), (int)(vertSpacing * .75)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Options", false,
                true, true);

            dataBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + (2 * horzSpacing),
                (int)(bounds.Top + (bounds.Height * 0.0625)) + vertSpacing, (int)(horzSpacing * .75),
                (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                Graphics.GlobalGfx.btnActive, "Save/Load", false, true, true);

            closeBtn = new SFButton(new Rectangle((int)(bounds.Width * .15625) + (2 * horzSpacing),
                (int)(bounds.Top + (bounds.Height * 0.0625)) + (2 * vertSpacing), (int)(horzSpacing * .75),
                (int)(vertSpacing * .75)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                Graphics.GlobalGfx.btnActive, "Exit Game", false, true, true);
            //end button init

        }

        public void updateMenu(GameTime gt, ContentManager cm)
        {
            if (subScreenOpen == false)
            {
                //update buttons
                orderBtn.updateButton();
                financeBtn.updateButton();
                optionsBtn.updateButton();
                dataBtn.updateButton();
                hireBtn.updateButton();
                closeBtn.updateButton();
                expandBtn.updateButton();
                ownerBtn.updateButton();
                questBtn.updateButton();
                //================

                //get if a button is clicked, then open corresponding menu.

                //close the game
                if (closeBtn.isDown())
                {
                    //show dialog box, then close on confirm
                    Game1.closeTrigger = true;
                }

                //open the order menu
                if (orderBtn.isDown())
                {
                    subMenu = new OrderView();
                    subMenu.InitView(cm);
                    subScreenOpen = true;
                }

                //open the order menu
                if (ownerBtn.isDown())
                {
                    subMenu = new PlayerView();
                    subMenu.InitView(cm);
                    subScreenOpen = true;
                }

                //open the expand menu
                if (expandBtn.isDown())
                {
                    subMenu = new ExpandView();
                    subMenu.InitView(cm);
                    subScreenOpen = true;
                }

                if (hireBtn.isDown())
                {
                    subMenu = new EmployView();
                    subMenu.InitView(cm);
                    subScreenOpen = true;
                }

            }
            else
            {
                //update sub menus
                if (subMenu != null)
                {
                    subMenu.UpdateView(gt, out statsScreenClosed);
                    if (statsScreenClosed == true)
                    {
                        statsScreenClosed = false;
                        subScreenOpen = false;
                        subMenu.UnloadView();
                        subMenu = null;
                    }
                }
            }
        }

        public void drawMenu(SpriteBatch sb, GameTime gt)
        {
            if (subScreenOpen == false)
            {
                //draw buttons
                orderBtn.drawButton(sb, true);
                financeBtn.drawButton(sb, true);
                optionsBtn.drawButton(sb, true);
                dataBtn.drawButton(sb, true);
                hireBtn.drawButton(sb, true);
                closeBtn.drawButton(sb, true);
                expandBtn.drawButton(sb, true);
                ownerBtn.drawButton(sb, true);
                questBtn.drawButton(sb, true);
                //===============


            }


            //draw view if opened.
            if (subMenu != null)
            {
                subMenu.DrawView(gt, sb);
            }

        }

    }
}
