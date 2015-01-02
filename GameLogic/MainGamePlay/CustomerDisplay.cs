using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.MainGamePlay
{
    /// <summary>
    /// Display the customer info when one is clicked.
    /// </summary>
    public class CustomerDisplay
    {
        private Input.SFButton upScrollBtn, downScrollBtn;
        private Rectangle panelBounds;
        private Customer customerSelected;
        private int startItemIndex = 0;

        public CustomerDisplay(Customer curCust)
        {
            this.customerSelected = curCust;
            panelBounds = new Rectangle(0, (int)(GameGlobal.GameHeight * 0.7f),
                GameGlobal.GameWidth, GameGlobal.GameHeight - (int)(GameGlobal.GameHeight * 0.7f));
            CustDisLoad();
        }

        private void CustDisLoad()
        {
            upScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                panelBounds.Top, (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                "^", false, true, true);
            downScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                panelBounds.Bottom - (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                "V", false, true, true);
        }

        public void CustDisUpdate(GameTime gt)
        {
            //update buttons to scroll
            upScrollBtn.updateButton();
            downScrollBtn.updateButton();

            //scrolling on button click
            #region Scrolling Through Inventory
            if (downScrollBtn.isDown())
            {
                if (startItemIndex < customerSelected.Inventory.Count - 3)
                {
                    startItemIndex++;
                }
            }

            if (upScrollBtn.isDown())
            {
                if (startItemIndex > 0)
                {
                    startItemIndex--;
                }
            }
            #endregion
        }

        public void CustDisDraw(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            //draw inventory
            drawItemTable(sb);

            int vertSpace = (int)GameGlobal.gameFont.MeasureString(" ").Y;

            //draw name
            sb.DrawString(GameGlobal.gameFont, customerSelected.Name,
                new Vector2(0, panelBounds.Top), Color.Gray);

            //draw customer class
            sb.DrawString(GameGlobal.gameFont, customerSelected.CustClass.ToString(),
                new Vector2(0, panelBounds.Top + vertSpace), Color.Gray);
            //draw customer satisfaction
            sb.DrawString(GameGlobal.gameFont, "Satisfaction: " + customerSelected.Satisfaction,
                new Vector2(0, panelBounds.Top + (2 * vertSpace)), Color.Gray);
            //draw customer thoughts
            sb.DrawString(GameGlobal.gameFont, "Thoughts: " + customerSelected.Thoughts,
                new Vector2(0, panelBounds.Top + (3 * vertSpace)), Color.Gray);

            sb.End();

            upScrollBtn.drawButton(sb, true);
            downScrollBtn.drawButton(sb, true);
        }

        private void drawItemTable(SpriteBatch sb)
        {
            int itemListSpacing = panelBounds.Height / 4;
            Vector2 positionStart = new Vector2(panelBounds.Center.X, panelBounds.Top + itemListSpacing);
            Vector2 positionEnd = new Vector2(upScrollBtn.Bounds.Left, panelBounds.Top + itemListSpacing);

            //draw bounds
            Graphics.SimpleDraw.drawLine(sb, new Vector2(panelBounds.Center.X, panelBounds.Top),
                new Vector2(panelBounds.Center.X, panelBounds.Bottom), 1, Color.DarkGray);

            Graphics.SimpleDraw.drawLine(sb, new Vector2(upScrollBtn.Bounds.Left, panelBounds.Top),
                new Vector2(upScrollBtn.Bounds.Left, panelBounds.Bottom), 1, Color.DarkGray);

            //draw table lines
            for (int sp = 1; sp <= 3; sp++)
            {
                Graphics.SimpleDraw.drawLine(sb, positionStart, positionEnd, 1, Color.DarkGray);
                positionEnd.Y += itemListSpacing;
                positionStart.Y += itemListSpacing;
            }
            //reset Y position
            positionStart.Y = panelBounds.Top;

            //draw items in table
            for (int sp = startItemIndex; sp <= startItemIndex + 3; sp++)
            {
                if (sp < customerSelected.Inventory.Count)
                {
                    //draw all data in the rectangle
                    sb.DrawString(GameGlobal.gameFont, customerSelected.Inventory[sp].item.Name,
                        new Vector2(panelBounds.Center.X, positionStart.Y), Color.Yellow);
                    sb.DrawString(GameGlobal.gameFont, customerSelected.Inventory[sp].item.Cost.ToString(),
                        new Vector2(upScrollBtn.Bounds.Left -
                            GameGlobal.gameFont.MeasureString(customerSelected.Inventory[sp].item.Cost.ToString()).X,
                            positionStart.Y), Color.Yellow);
                    sb.DrawString(GameGlobal.gameFont, customerSelected.Inventory[sp].item.OnShelf.ToString(),
                        new Vector2(panelBounds.Center.X, positionStart.Y +
                            GameGlobal.gameFont.MeasureString(customerSelected.Inventory[sp].item.OnShelf.ToString()).Y),
                            Color.Yellow);
                    positionStart.Y += itemListSpacing;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
