using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Input
{
    public enum SFButtonState
    {
        Up, //not clicked on or hovered over
        Hover, //mouse hovering over button
        Down, //Stay in the click position after left click released or mouse outside bounds
    }

    public class SFButton
    {
        private SFButtonState btnState;
        private Rectangle bounds;
        private Texture2D background;
        private Texture2D hoverTex, activeTex;
        private string buttonText;
        private bool vis, activeSup, enabled;

        //constructors
        /// <summary>
        /// Constructor to init a button
        /// </summary>
        /// <param name="bounds">The location for the button to be drawn</param>
        /// <param name="background">The texture to draw for the button</param>
        /// <param name="hover">Texture for when the mouse hovers over button</param>
        /// <param name="activeTex">Texture when button is clicked or active</param>
        /// <param name="startingText">The Text that will appear on the button</param>
        /// <param name="activeSupport">If the button supports active mode (stay active after a click).</param>
        /// <param name="defEnable">The initial state of being enabled.</param>
        /// <param name="defVis">The initial visible state.</param>
        public SFButton(Rectangle bounds, Texture2D background, Texture2D hover, Texture2D activeTex, 
            string startingText, bool activeSupport, bool defEnable, bool defVis)
        {
            this.bounds = bounds;
            this.background = background;
            this.buttonText = startingText;
            this.activeSup = activeSupport;
            this.hoverTex = hover;
            this.activeTex = activeTex;
            this.vis = defVis;
            this.enabled = defEnable;
            btnState = SFButtonState.Up;
        }

        #region Accessors
        /// <summary>
        /// Gets or Sets the button text
        /// </summary>
        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; }
        }

        /// <summary>
        /// Get the bounds of the button.
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        /// <summary>
        /// Gets or Sets the Current Button State
        /// </summary>
        public SFButtonState ButtonState
        {
            get { return btnState; }
            set { btnState = value; }
        }

        /// <summary>
        /// Gets or Sets the visibility of the button
        /// </summary>
        public bool IsVisible
        {
            get { return vis; }
            set { vis = value; }
        }

        /// <summary>
        /// Gets or Sets whether the button is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        #endregion

        #region State Functions
        public bool isUp()
        {
            return btnState == SFButtonState.Up;
        }

        public bool isDown()
        {
            return btnState == SFButtonState.Down;
        }

        public bool isHover()
        {
            return btnState == SFButtonState.Hover;
        }
        #endregion

        public void updateButton()
        {
            if (enabled)
            {
                if (GameLogic.GameGlobal.InputControl.CurrentMouseState.X >= bounds.Left &&
                    GameLogic.GameGlobal.InputControl.CurrentMouseState.X <= bounds.Right &&
                    GameLogic.GameGlobal.InputControl.CurrentMouseState.Y >= bounds.Top &&
                    GameLogic.GameGlobal.InputControl.CurrentMouseState.Y <= bounds.Bottom)
                {
                    if (btnState == SFButtonState.Down && activeSup == false)
                    {
                        btnState = SFButtonState.Hover;
                    }
                    if (GameLogic.GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
                    {
                        if (btnState == SFButtonState.Hover || btnState == SFButtonState.Up)
                        {
                            btnState = SFButtonState.Down;
                        }
                        else if (btnState == SFButtonState.Down)
                        {
                            btnState = SFButtonState.Up;
                        }
                    }
                    else
                    {
                        if (btnState != SFButtonState.Down)
                        {
                            btnState = SFButtonState.Hover;
                        }
                    }
                }
                else
                {
                    if (btnState == SFButtonState.Down && activeSup == true)
                    {
                        btnState = SFButtonState.Down;
                    }
                    else
                    {
                        btnState = SFButtonState.Up;
                    }
                }
            }
        }

        public void drawButton(SpriteBatch sb, bool drawText)
        {
            if (vis)
            {
                sb.Begin();
                if (enabled)
                {
                    switch (btnState)
                    {
                        case SFButtonState.Up:
                            sb.Draw(background, bounds, Color.White);
                            if (drawText)
                                sb.DrawString(GameLogic.GameGlobal.gameFont, buttonText, centerBtnText(), Color.SandyBrown);
                            break;
                        case SFButtonState.Hover:
                            sb.Draw(hoverTex, bounds, Color.White);
                            if (drawText)
                                sb.DrawString(GameLogic.GameGlobal.gameFont, buttonText, centerBtnText(), Color.SandyBrown);
                            break;
                        case SFButtonState.Down:
                            sb.Draw(activeTex, bounds, Color.White);
                            if (drawText)
                                sb.DrawString(GameLogic.GameGlobal.gameFont, buttonText, centerBtnText(), Color.SandyBrown);
                            break;
                    }
                }
                else
                {
                    sb.Draw(background, bounds, Color.DarkGray);
                    if (drawText)
                        sb.DrawString(GameLogic.GameGlobal.gameFont, buttonText, new Vector2(bounds.Left + 5, bounds.Top), Color.White);
                }
                sb.End();
            }
        }

        /// <summary>
        /// Centers the text of a button.
        /// </summary>
        /// <returns>A vector2 object ot pass to the drawing function to center text in the button.</returns>
        private Vector2 centerBtnText()
        {
            //get the string dimensions
            Vector2 stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(buttonText);
            //center the text based on string dimensions and button bounds
            Vector2 pos = new Vector2(bounds.Center.X - (stringMeas.X / 2), bounds.Center.Y - (stringMeas.Y / 2));
            //return the centered vector to draw the text.
            return pos;
        }

        public void unloadBtn()
        {
            bounds = Rectangle.Empty;
        }

        #region Overriden Methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            SFButton btn = obj as SFButton;
            if ((System.Object)btn == null) return false;

            return (this.background == btn.background) && (this.bounds == btn.bounds) && (this.buttonText == btn.buttonText);
        }

        public override int GetHashCode()
        {
            return this.buttonText.GetHashCode() ^ this.bounds.GetHashCode() | this.background.GetHashCode();
        }

        public override string ToString()
        {
            return this.buttonText;
        }
        #endregion
    }
}
