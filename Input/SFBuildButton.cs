using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Input
{
    public class SFBuildButton : SFButton
    {
        Texture2D fore;
        Vector2 textCostPosition, textNamePosition, texturePosition;
        string nameText;

        public Texture2D BtnTexture
        {
            get { return fore; }
        }

        public SFBuildButton(Vector2 position, bool defaultEnabled, string text, string nameText, Texture2D foreground)
            : base(new Rectangle((int)position.X, (int)position.Y, 
                (int)(GameLogic.GameGlobal.GameWidth / 5f), GameLogic.GameGlobal.GameHeight - (int)(GameLogic.GameGlobal.GameHeight / 1.45f)), 
            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, text, false, defaultEnabled, true)
        {
            fore = foreground;
            this.nameText = nameText;
        }

        public Vector2 Position
        {
            get { return new Vector2(Bounds.X, Bounds.Y); }
            set { Bounds = new Rectangle((int)value.X, (int)value.Y, Bounds.Width, Bounds.Height); }
        }

        public void updateBtn()
        {
            base.updateButton();
        }

        public void drawBtn(SpriteBatch sb)
        {
            base.drawButton(sb, false);

            sb.Begin();
            //draw the room texture and text, centered
            texturePosition = centerBtnText(out textCostPosition, out textNamePosition);
            sb.Draw(fore, new Rectangle((int)texturePosition.X, (int)texturePosition.Y, fore.Width, fore.Height), Color.White);
            sb.DrawString(GameLogic.GameGlobal.gameFont, this.ButtonText, textCostPosition, Color.White);
            sb.DrawString(GameLogic.GameGlobal.gameFont, this.nameText, textNamePosition, Color.White);
            sb.End();
        }

        private Vector2 centerBtnText(out Vector2 textPos, out Vector2 namePos)
        {
            //get the texture dimensions
            Vector2 texturePos = new Vector2(this.Bounds.Center.X - (fore.Bounds.Width / 2), this.Bounds.Center.Y - (fore.Bounds.Height / 1.5f));
            //get the string dimensions
            Vector2 stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(this.ButtonText);
            Vector2 nameMeas = GameLogic.GameGlobal.gameFont.MeasureString(this.nameText);
            //center the text based on string dimensions and button bounds
            textPos = new Vector2(this.Bounds.Center.X - (stringMeas.X / 2), (this.Bounds.Center.Y - (fore.Bounds.Height / 1.5f)) + fore.Bounds.Height + 15);
            namePos = new Vector2(this.Bounds.Center.X - (nameMeas.X / 2), (this.Bounds.Center.Y - (fore.Bounds.Height / 0.35f)) + fore.Bounds.Height - 15);
            //return the centered vector to draw the text.
            return texturePos;
        }

        public void unloadBuildBtn()
        {
            //fore.Dispose();
            textCostPosition = Vector2.Zero;
            texturePosition = Vector2.Zero;
            base.unloadBtn();
        }
    }
}
