using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Storefront.Menus
{
    internal class TitleBtn
    {
        public Texture2D btnTex;
        public bool? btnAnimate;
        public Vector2 btnPos;
    }

    public class TitleScreen : Menu
    {
        private TitleBtn newGameBtn, loadGameBtn, optionsBtn, exitBtn;
        private readonly int MAXSCROLLX = (int)(GameLogic.GameGlobal.GameWidth / 1.6f);
        private readonly int MINSCROLLX = (int)(GameLogic.GameGlobal.GameWidth / 1.35f);
        private readonly int SCROLLSPEED = 6;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TitleScreen() 
        {
            newGameBtn = new TitleBtn();
            loadGameBtn = new TitleBtn();
            optionsBtn = new TitleBtn();
            exitBtn = new TitleBtn();
        }

        /// <summary>
        /// Load all the images used for the title screen
        /// </summary>
        /// <param name="cm">The ContentManager used to load in assets.</param>
        public override void loadMenu(ContentManager cm)
        {
            //load the background image.
            Background = cm.Load<Texture2D>(@"UI\Menus\StoreFrontTitleScreen");
            newGameBtn.btnTex = cm.Load<Texture2D>(@"UI\Menus\newGameBtn");
            loadGameBtn.btnTex = cm.Load<Texture2D>(@"UI\Menus\loadGameBtn");
            optionsBtn.btnTex = cm.Load<Texture2D>(@"UI\Menus\optionsGameBtn");
            exitBtn.btnTex = cm.Load<Texture2D>(@"UI\Menus\exitGameBtn");

            newGameBtn.btnAnimate = false;
            loadGameBtn.btnAnimate = false;
            optionsBtn.btnAnimate = false;
            exitBtn.btnAnimate = false;

            float step = (((GameLogic.GameGlobal.GameHeight / 1.35f) + exitBtn.btnTex.Height) - 
                (GameLogic.GameGlobal.GameHeight / 14f)) / 4f;
            newGameBtn.btnPos = new Vector2(GameLogic.GameGlobal.GameWidth / 1.35f, GameLogic.GameGlobal.GameHeight / 14f);
            loadGameBtn.btnPos = new Vector2(GameLogic.GameGlobal.GameWidth / 1.35f, (GameLogic.GameGlobal.GameHeight / 14f) + step);
            optionsBtn.btnPos = new Vector2(GameLogic.GameGlobal.GameWidth / 1.35f, (GameLogic.GameGlobal.GameHeight / 14f) + (2 * step));
            exitBtn.btnPos = new Vector2(GameLogic.GameGlobal.GameWidth / 1.35f, (GameLogic.GameGlobal.GameHeight / 14f) + (3 * step));
        }

        /// <summary>
        /// Update the menu every frame.
        /// </summary>
        /// <param name="gt">GameTime, used for any timng events.</param>
        public override void updateMenu(GameTime gt)
        {
            #region New Game Button
            //determine if the mouse is over the texture to animate the button.
            if (MouseWithinBtn(newGameBtn))
            {
                newGameBtn.btnAnimate = true; //animate forward
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
                {
                    NextMenu = 3;
                    MenuTransition = true;
                }
            }
            else
            {
                if (newGameBtn.btnPos.X < MINSCROLLX)
                {
                    newGameBtn.btnAnimate = null; //reverse animate
                }
                else
                {
                    newGameBtn.btnAnimate = false; //stop animate
                }
            }

            //handle animation
            if (newGameBtn.btnAnimate == true && (newGameBtn.btnPos.X >= MAXSCROLLX))
            {
                newGameBtn.btnPos.X -= SCROLLSPEED;
            }
            else if (newGameBtn.btnAnimate == null && (newGameBtn.btnPos.X <= MINSCROLLX))
            {
                newGameBtn.btnPos.X += SCROLLSPEED;
            }
            #endregion

            #region Load Game Button
            //determine if the mouse is over the texture to animate the button.
            if (MouseWithinBtn(loadGameBtn))
            {
                loadGameBtn.btnAnimate = true; //animate forward
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
                {
                    NextMenu = 2;
                    MenuTransition = true;
                }
            }
            else
            {
                if (loadGameBtn.btnPos.X < MINSCROLLX)
                {
                    loadGameBtn.btnAnimate = null; //reverse animate
                }
                else
                {
                    loadGameBtn.btnAnimate = false; //stop animate
                }
            }

            //handle animation
            if (loadGameBtn.btnAnimate == true && (loadGameBtn.btnPos.X >= MAXSCROLLX))
            {
                loadGameBtn.btnPos.X -= SCROLLSPEED;
            }
            else if (loadGameBtn.btnAnimate == null && (loadGameBtn.btnPos.X <= MINSCROLLX))
            {
                loadGameBtn.btnPos.X += SCROLLSPEED;
            }
            #endregion

            #region Options Game Button
            //determine if the mouse is over the texture to animate the button.
            if (MouseWithinBtn(optionsBtn))
            {
                optionsBtn.btnAnimate = true; //animate forward
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
                {
                    NextMenu = 1;
                    MenuTransition = true;
                }
            }
            else
            {
                if (optionsBtn.btnPos.X < MINSCROLLX)
                {
                    optionsBtn.btnAnimate = null; //reverse animate
                }
                else
                {
                    optionsBtn.btnAnimate = false; //stop animate
                }
            }

            //handle animation
            if (optionsBtn.btnAnimate == true && (optionsBtn.btnPos.X >= MAXSCROLLX))
            {
                optionsBtn.btnPos.X -= SCROLLSPEED;
            }
            else if (optionsBtn.btnAnimate == null && (optionsBtn.btnPos.X <= MINSCROLLX))
            {
                optionsBtn.btnPos.X += SCROLLSPEED;
            }
            #endregion

            #region Exit Game Button
            //determine if the mouse is over the texture to animate the button.
            if (MouseWithinBtn(exitBtn))
            {
                exitBtn.btnAnimate = true; //animate forward
                if (GameLogic.GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
                {
                    Game1.closeTrigger = true;
                }
            }
            else
            {
                if (exitBtn.btnPos.X < MINSCROLLX)
                {
                    exitBtn.btnAnimate = null; //reverse animate
                }
                else
                {
                    exitBtn.btnAnimate = false; //stop animate
                }
            }

            //handle animation
            if (exitBtn.btnAnimate == true && (exitBtn.btnPos.X >= MAXSCROLLX))
            {
                exitBtn.btnPos.X -= SCROLLSPEED;
            }
            else if (exitBtn.btnAnimate == null && (exitBtn.btnPos.X <= MINSCROLLX))
            {
                exitBtn.btnPos.X += SCROLLSPEED;
            }
            #endregion
        }

        /// <summary>
        /// Draw the menu to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch object used to draw the assets to the screen</param>
        /// <param name="gt">GameTime, used for any timng events.</param>
        public override void drawMenu(SpriteBatch sb, GameTime gt)
        {
            //draw the background
            sb.Draw(Background, new Rectangle(0, 0, Background.Width, Background.Height), Color.White);

            sb.Draw(newGameBtn.btnTex, newGameBtn.btnPos, Color.White);
            sb.Draw(loadGameBtn.btnTex, loadGameBtn.btnPos, Color.White);
            sb.Draw(optionsBtn.btnTex, optionsBtn.btnPos, Color.White);
            sb.Draw(exitBtn.btnTex, exitBtn.btnPos, Color.White);
        }

        /// <summary>
        /// Determine if the mouse is within the opaque sections of the buttons.
        /// </summary>
        /// <param name="btn">The button to check</param>
        /// <returns>True if within bounds, false if not.</returns>
        private bool MouseWithinBtn(TitleBtn btn)
        {
            if (GameLogic.GameGlobal.InputControl.CurrentMouseState.X >= btn.btnPos.X &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.X <= (btn.btnPos.X + btn.btnTex.Width) &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.Y >= btn.btnPos.Y &&
                GameLogic.GameGlobal.InputControl.CurrentMouseState.Y <= (btn.btnPos.Y + btn.btnTex.Height))
            {
                //determine mouse location on texture and check transparency (don't register as hovering
                //over texture if that part of texture is transparent).
                Color[] texColorArray = new Color[btn.btnTex.Width * btn.btnTex.Height];
                btn.btnTex.GetData<Color>(texColorArray);
                int MouseAdjX = (int)(GameLogic.GameGlobal.InputControl.CurrentMouseState.X - btn.btnPos.X);
                int MouseAdjY = (int)(GameLogic.GameGlobal.InputControl.CurrentMouseState.Y - btn.btnPos.Y);
                if (texColorArray[MouseAdjX + MouseAdjY * btn.btnTex.Width].A > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
