using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Storefront.Input;
using Storefront.Database;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class OrderView : SubMenuView
    {
        //buttons
        private SFButton closeBtn;
        private SFButton scrollUpBtn, scrollDownBtn, scrollLeftBtn, scrollRightBtn;
        private SFButton queryWriterBtn;
        private SFButton orderBtn;
        private SFButton defaultDBBtn;
        //text fields
        private SFTextField jumpToIndexField;
        private SFTextField searchByNameField;
        private SFTextField orderAmtField;
        private SFTextField priceField;
        private SFTextField weeksToOrder;
        //dialog box
        private SFDialogBox confirmOrder;
        private bool showOrderConfirmation = false;
        //textures
        //private Texture2D columnHeaderTextures, background;
        //database vars
        private DBConnector dbc;
        private DataTable items;
        //display vars
        private int numEntries; // the number of entries in the item database.
        private int projRev = 0; //projected revenue.
        private int orderCost; //the cost of the order for this item (orderAmt * PPU)
        private int selectedID; //id of the selected item (also the index).
        private Rectangle selectedRect;
        private List<string> columnHeaders;
        //scrolling vars
        private byte horzIndex;
        private int vertIndex = 0;
        private Rectangle scrollThumb;
        private int pageSize = 9;
        //screen calc vars
        private int rightPanelHeight, rightPanelWidth;
        private int leftPanelHeight, leftPanelWidth;
        private int dbPanelHeight, dbPanelWidth;
        //ordering vars
        int itemOrderIndex;
        GameLogic.MainGamePlay.Order order;
        bool confirmed = false;

        public OrderView()
        {

        }

        public override void InitView(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            //get items from the database
            dbc = new DBConnector();
            items = dbc.GetItemsFromDB(GameGlobal.player.TableName);
            numEntries = items.Rows.Count;

            //load buttons
            //close button
            closeBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth * .8f),
                                        (int)(GameGlobal.GameHeight / 1.0833f),
                                        (int)(GameGlobal.GameWidth - (GameGlobal.GameWidth * .8f)),
                                        (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0833f))),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Close", false, true, true);

            //auto order text field
            weeksToOrder = new SFTextField(new Rectangle((int)(GameGlobal.GameWidth * .6f),
                                            (int)(GameGlobal.GameHeight / 1.0416f),
                                            (int)((GameGlobal.GameWidth * .8f) - (GameGlobal.GameWidth * .6f)),
                                            (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0416f))),
                                            Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 2);
            weeksToOrder.ResetFocus = true;
            weeksToOrder.NumbersOnly = true;
            weeksToOrder.Text = "Weeks To Order";           

            //order button
            orderBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth * .6f),
                                        (int)(GameGlobal.GameHeight / 1.0833f),
                                        (int)((GameGlobal.GameWidth * .8f) - (GameGlobal.GameWidth * .6f)),
                                        (int)(weeksToOrder.Bounds.Top - (GameGlobal.GameHeight / 1.0833f))),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Order", false, true, true);
            //scroll buttons
            scrollLeftBtn = new SFButton(new Rectangle(0, (int)((GameGlobal.GameHeight * .7f) - (GameGlobal.GameHeight * .05f)),
                                        (int)(GameGlobal.GameWidth * .05f), (int)(GameGlobal.GameHeight * .05f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "<-", false, true, true);

            scrollRightBtn = new SFButton(new Rectangle((int)((GameGlobal.GameWidth * .6f) - 2 * (GameGlobal.GameWidth * .05f)),
                                        (int)((GameGlobal.GameHeight * .7f) - (GameGlobal.GameHeight * .05f)),
                                        (int)(GameGlobal.GameWidth * .05f), (int)(GameGlobal.GameHeight * .05f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "->", false, true, true);

            scrollUpBtn = new SFButton(new Rectangle((int)((GameGlobal.GameWidth * .6f) - (GameGlobal.GameWidth * .05f)), 0,
                                        (int)(GameGlobal.GameWidth * .05f), (int)(GameGlobal.GameHeight * .05f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "^", false, true, true);

            scrollDownBtn = new SFButton(new Rectangle((int)((GameGlobal.GameWidth * .6f) - (GameGlobal.GameWidth * .05f)),
                                        (int)((GameGlobal.GameHeight * .7f) - (GameGlobal.GameHeight * .05f)),
                                        (int)(GameGlobal.GameWidth * .05f), (int)(GameGlobal.GameHeight * .05f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "v", false, true, true);

            //reset button
            defaultDBBtn = new SFButton(new Rectangle((scrollRightBtn.Bounds.Left / 2),
                                        (int)((GameGlobal.GameHeight * .7f) - (GameGlobal.GameHeight * .05f)),
                                        (int)(GameGlobal.GameWidth * .075f), (int)(GameGlobal.GameHeight * .05f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Reset", false, true, true);

            //query button
            queryWriterBtn = new SFButton(new Rectangle((int)(GameGlobal.GameWidth * .9f), 0,
                                        (int)(GameGlobal.GameWidth - (GameGlobal.GameWidth * .9f)),
                                        (int)(GameGlobal.GameHeight * .15f)),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "Query\nWriter", false, true, true);

            //text fields
            //search by name field
            searchByNameField = new SFTextField(new Rectangle((int)(GameGlobal.GameWidth * .6f), 0,
                                        (int)((GameGlobal.GameWidth * .9f) - (GameGlobal.GameWidth * .6f)),
                                        (int)(GameGlobal.GameHeight * .15f / 2)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 20);
            searchByNameField.ResetFocus = true;
            searchByNameField.Text = "Search By Name";

            //jump to index field
            jumpToIndexField = new SFTextField(new Rectangle((int)(GameGlobal.GameWidth * .6f), searchByNameField.Bounds.Bottom,
                                        (int)((GameGlobal.GameWidth * .9f) - (GameGlobal.GameWidth * .6f)),
                                        (int)(GameGlobal.GameHeight * .15f / 2)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 4);
            jumpToIndexField.ResetFocus = true;
            jumpToIndexField.NumbersOnly = true;
            jumpToIndexField.Text = "Jump to Index";

            //price set field
            int vertSpacing = (closeBtn.Bounds.Top - (int)(queryWriterBtn.Bounds.Bottom + (GameGlobal.gameFont.MeasureString(")").Y / 2))) / 8;
            int drawHeight = (int)(queryWriterBtn.Bounds.Bottom + (GameGlobal.gameFont.MeasureString(")").Y / 2) + (3*vertSpacing));
            priceField = new SFTextField(
                new Rectangle((int)(GameGlobal.GameWidth * .85f), drawHeight,
                    (int)(GameGlobal.GameWidth * 0.15f), (int)(GameGlobal.gameFont.MeasureString(")").Y * 1.25)),
                    Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 6);

            priceField.NumbersOnly = true;

            //order amount field
            drawHeight = (int)(queryWriterBtn.Bounds.Bottom + (GameGlobal.gameFont.MeasureString(")").Y / 2) + (5 * vertSpacing));
            orderAmtField = new SFTextField(
                new Rectangle((int)(GameGlobal.GameWidth * .85f), drawHeight,
                    (int)(GameGlobal.GameWidth * 0.15f), (int)(GameGlobal.gameFont.MeasureString(")").Y * 1.25)),
                    Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 3);

            orderAmtField.NumbersOnly = true;

            //setup order confirmation dialog box
            confirmOrder = new SFDialogBox(Rectangle.Empty, Graphics.GlobalGfx.dialogTexture, "", DefaultChoices.Yes_No);
                

            //get dimensions of right panel
            rightPanelHeight = orderBtn.Bounds.Top - jumpToIndexField.Bounds.Bottom;
            rightPanelWidth = (int)(GameGlobal.GameWidth - GameGlobal.GameWidth * .6f);
            //get dimensions of left panel
            leftPanelHeight = (int)((GameGlobal.GameHeight / 1.0833f) - (GameGlobal.GameHeight * .7f));
            leftPanelWidth = (int)(GameGlobal.GameWidth * .6f);
            //get DB view dimensions
            dbPanelHeight = scrollLeftBtn.Bounds.Top;
            dbPanelWidth = scrollUpBtn.Bounds.Left;

            //setup Headers
            setupColumnHeaders();
            selectedRect = new Rectangle(0, (dbPanelHeight / 10), dbPanelWidth, (dbPanelHeight / 10));
            scrollThumb = new Rectangle(dbPanelWidth, scrollUpBtn.Bounds.Bottom, scrollUpBtn.Bounds.Width, pageSize);
        }

        public override void LoadView()
        {
            
        }

        public override void UpdateView(GameTime gt, out bool? stateSwitch)
        {
            stateSwitch = false;
            if (showOrderConfirmation == false)
            {
                //update buttons
                closeBtn.updateButton();
                scrollLeftBtn.updateButton();
                scrollRightBtn.updateButton();
                scrollUpBtn.updateButton();
                scrollDownBtn.updateButton();
                orderBtn.updateButton();
                queryWriterBtn.updateButton();
                defaultDBBtn.updateButton();
                //update text fields
                searchByNameField.updateText();
                jumpToIndexField.updateText();
                priceField.updateText();
                weeksToOrder.updateText();
                orderAmtField.updateText();
                //scroll horizontally
                #region Horizontal Scrolling
                if (scrollLeftBtn.isDown())
                {
                    if (horzIndex > 0)
                    {
                        horzIndex--;
                    }
                }
                if (scrollRightBtn.isDown())
                {
                    if (horzIndex < columnHeaders.Count - 3)
                    {
                        horzIndex++;
                    }
                }
                #endregion
                //scroll vertically
                #region Vertical Scrolling
                if (scrollDownBtn.isDown())
                {
                    if (vertIndex < (numEntries - 9))
                    {
                        vertIndex++;
                    }
                }
                if (scrollUpBtn.isDown())
                {
                    if (vertIndex > 0)
                    {
                        vertIndex--;
                    }
                }
                #endregion
                //update rectangle to highlight selected
                updateSelectedRectangle();
                //update scroll bar rectangle
                updateScrollRectangle();
                //search fields

                //jump to index
                if (jumpToIndexField.Focus)
                {
                    if (GameGlobal.InputControl.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        int i = Int32.Parse(jumpToIndexField.Text);
                        if (i >= numEntries || i <= 0)
                        {
                            //error
                        }
                        else if (i < numEntries && i > (numEntries - 8))
                        {
                            vertIndex = numEntries - 9;
                        }
                        else
                        {
                            vertIndex = i - 1;
                        }
                    }
                }

                //search by name
                if (searchByNameField.Focus)
                {
                    if (GameGlobal.InputControl.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        selectedID = 0;
                        vertIndex = 0;
                        string query = "SELECT * FROM " + GameGlobal.player.TableName + 
                            " WHERE Name LIKE '%" + searchByNameField.Text + "%';";
                        items = dbc.runQuery(query);
                        numEntries = items.Rows.Count;
                    }
                }

                //reset database to default display
                if (defaultDBBtn.isDown())
                {
                    selectedID = 0;
                    vertIndex = 0;
                    items = dbc.GetItemsFromDB(GameGlobal.player.TableName);
                    numEntries = items.Rows.Count;
                }

                //close button
                if (closeBtn.isDown())
                {
                    stateSwitch = true;
                }

                //get the index
                getSelectedID();

                //if the place order button is clicked
                if (orderBtn.isDown())
                {
                    //and data available in the amount to order text field
                    if (orderAmtField.Text.Length > 0)
                    {
                        //get the amount out of the text field
                        int amt = Int32.Parse(orderAmtField.Text);
                        //if at least 1 item is being ordered
                        if (amt > 0)
                        {
                            //create the order and trigger dialog box
                            order = new MainGamePlay.Order();
                            order.name = items.Rows[selectedID].ItemArray[1].ToString().Trim();
                            if (weeksToOrder.Text == "Weeks To Order" || weeksToOrder.Text == "")
                            {
                                order.weeksLeft = 1;
                            }
                            else
                            {
                                order.weeksLeft = Byte.Parse(weeksToOrder.Text);
                            }
                            order.amtToOrder = amt;
                            order.totalCost = orderCost;
                            showOrderConfirmation = true;
                        }
                    }
                }
            }
            else
            {
                #region Dialog Box and Order Placing
                //search for the item already in the order list (to add more amount to order)
                itemOrderIndex = GameGlobal.orderHandler.itemSearch(order);
                //if the item is in the list already
                if (!confirmed)
                {
                    if (itemOrderIndex >= 0)
                    {
                        //if you are over the max amount to order, throw error
                        if (!GameGlobal.orderHandler.overOrder(itemOrderIndex, order))
                        {
                            confirmOrder.Dialog = "You can not order more than 255 of an item per week...";
                            confirmOrder.PresetChoices = DefaultChoices.Close;
                            confirmed = true;
                        }
                        else //else add it to the order
                        {
                            if (order.weeksLeft > 1)
                            {
                                confirmOrder.Dialog = "Are you sure you want to buy " + order.amtToOrder + " " +
                                    order.name + "(s) for " + order.weeksLeft + " weeks for a total cost of " +
                                    GameGlobal.orderHandler.getTotalCost(order, itemOrderIndex) + " per week?";
                            }
                            else
                            {
                                confirmOrder.Dialog = "Are you sure you want to buy " + order.amtToOrder + " " +
                                    order.name + "(s) for a total cost of " +
                                    GameGlobal.orderHandler.getTotalCost(order, itemOrderIndex) + "?";
                            }
                            confirmOrder.PresetChoices = DefaultChoices.Yes_No;
                        }
                    }
                    else //if the item is not in the list, add it to the list.
                    {
                        if (order.amtToOrder > 255)
                        {
                            confirmOrder.Dialog = "You can not order more than 255 of an item per week...";
                            confirmOrder.PresetChoices = DefaultChoices.Close;
                            confirmed = true;
                        }
                        else
                        {
                            //update the text & choices of the dialog box
                            if (order.weeksLeft > 1)
                            {
                                confirmOrder.Dialog = "Are you sure you want to buy " + order.amtToOrder + " " +
                                    order.name + "(s) for " + order.weeksLeft + " weeks for a total cost of " +
                                    order.totalCost + " per week?";
                            }
                            else
                            {
                                confirmOrder.Dialog = "Are you sure you want to order " + order.amtToOrder + " " +
                                    order.name + "(s) for a cost of " + order.totalCost + "?";
                            }
                            confirmOrder.PresetChoices = DefaultChoices.Yes_No;
                        }
                    }
                }
                //update the box
                confirmOrder.updateDialogBox();
                //get the selection
                int selection = confirmOrder.Selection;
                //if in stage two (order confirmed)
                if (confirmed)
                {
                    //if the user clicks to close, go back to main window
                    if (selection == 0)
                    {
                        showOrderConfirmation = false;
                        confirmed = false;
                    }
                }
                else //if in the confirm order stage
                {
                    switch (selection)
                    {
                        case 0: //place order on confirmation
                            GameGlobal.orderHandler.addItemToOrder(order, itemOrderIndex);
                            confirmOrder.Dialog = "Order has been placed!";
                            confirmOrder.PresetChoices = DefaultChoices.Close;
                            confirmed = true;
                            break;
                        case 1: //close the window to allow changes.
                            showOrderConfirmation = false;
                            break;
                        default:

                            break;
                    }
                }
                #endregion
            }
        }

        public override void DrawView(GameTime gt, SpriteBatch sb)
        {
            sb.Begin();

            //draw borders
            Graphics.SimpleDraw.drawLine(sb, new Vector2(GameGlobal.GameWidth * .6f, 0),
                new Vector2(GameGlobal.GameWidth * .6f, GameGlobal.GameHeight), 1, Color.Yellow);
            //horizontal line
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight * .7f),
                new Vector2(GameGlobal.GameWidth * .6f, GameGlobal.GameHeight * .7f), 1, Color.Yellow);
            //money horizontal line
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 1.0833f),
                new Vector2(GameGlobal.GameWidth * .6f, GameGlobal.GameHeight / 1.0833f), 1, Color.Yellow);

            Graphics.SimpleDraw.fillArea(sb, scrollThumb, Color.DarkGoldenrod);

            drawTableSetup(sb);

            drawRightPanel(sb);
            drawLeftPanel(sb);

            drawDataInTable(sb);

            sb.End();

            //draw buttons
            closeBtn.drawButton(sb, true);
            orderBtn.drawButton(sb, true);
            scrollLeftBtn.drawButton(sb, true);
            scrollRightBtn.drawButton(sb, true);
            scrollDownBtn.drawButton(sb, true);
            scrollUpBtn.drawButton(sb, true);
            queryWriterBtn.drawButton(sb, true);
            defaultDBBtn.drawButton(sb, true);
            //draw text fields
            searchByNameField.drawField(sb, gt);
            jumpToIndexField.drawField(sb, gt);
            priceField.drawField(sb, gt);
            weeksToOrder.drawField(sb, gt);
            orderAmtField.drawField(sb, gt);
            //draw dialog box if needed
            if (showOrderConfirmation == true)
            {
                confirmOrder.drawDialogBox(sb, gt);
            }
        }

        private void updateScrollRectangle()
        {
            float ratio = pageSize / (float)numEntries;
            int space = (int)((scrollDownBtn.Bounds.Top - scrollUpBtn.Bounds.Bottom) / (float)numEntries);
            if (ratio <= 1)
            {
                scrollThumb.Height = (int)(ratio *
                    (scrollDownBtn.Bounds.Top - scrollUpBtn.Bounds.Bottom));
            }
            else
            {
                scrollThumb.Height = scrollDownBtn.Bounds.Top - scrollUpBtn.Bounds.Bottom;
            }
            scrollThumb.Y = (vertIndex * space) + scrollUpBtn.Bounds.Bottom;
        }

        /// <summary>
        /// Sets up the list of columns.
        /// </summary>
        private void setupColumnHeaders()
        {
            columnHeaders = new List<string>();
            columnHeaders.Add("PPU");
            columnHeaders.Add("In Stock");
            columnHeaders.Add("Demand");
            columnHeaders.Add("AvgC Price");
            columnHeaders.Add("Your Price");
            columnHeaders.Add("S Past Wk");
            columnHeaders.Add("S Past Yr");
            columnHeaders.Add("B Past Wk");
            columnHeaders.Add("Weight");
            horzIndex = 0;
        }

        /// <summary>
        /// Draws Columns and outer lines
        /// </summary>
        /// <param name="sb">SpriteBatch to draw, does not use Begin/End</param>
        private void drawTableSetup(SpriteBatch sb)
        {
            //column vertical lines
            Graphics.SimpleDraw.drawLine(sb,
                new Vector2(dbPanelWidth / 4, 0), new Vector2(dbPanelWidth / 4, dbPanelHeight),
                1, Color.Yellow);

            Graphics.SimpleDraw.drawLine(sb,
                new Vector2(dbPanelWidth / 2, 0), new Vector2(dbPanelWidth / 2, dbPanelHeight),
                1, Color.Yellow);

            Graphics.SimpleDraw.drawLine(sb,
                new Vector2(3 * (dbPanelWidth / 4), 0), new Vector2(3 * (dbPanelWidth / 4), dbPanelHeight),
                1, Color.Yellow);

            Graphics.SimpleDraw.drawLine(sb,
                new Vector2(dbPanelWidth, 0), new Vector2(dbPanelWidth, dbPanelHeight),
                1, Color.Yellow);

            //bottom horizontal line
            Graphics.SimpleDraw.drawLine(sb,
                new Vector2(0, dbPanelHeight), new Vector2(dbPanelWidth, dbPanelHeight),
                1, Color.Yellow);
            //draw horizonal lines
            int spacing = dbPanelHeight / 10;
            int spacCount = spacing;
            for (int i = 0; i < 9; i++)
            {
                Graphics.SimpleDraw.drawLine(sb,
                    new Vector2(0, spacCount), new Vector2(dbPanelWidth, spacCount),
                    1, Color.Yellow);
                spacCount += spacing;
            }

            //draw highlighed rectangle over selected item
            if (selectedRect != null)
            {
                Graphics.SimpleDraw.fillArea(sb, selectedRect, Color.IndianRed * 0.35f);
            }

            //draw column strings
            //get the string dimensions
            Vector2 stringMeas = GameLogic.GameGlobal.gameFont.MeasureString("Name");
            //center the text based on string dimensions and button bounds
            Vector2 pos = new Vector2((dbPanelWidth / 8) - (stringMeas.X / 2), 
                (dbPanelHeight / 20) - (stringMeas.Y / 2));
            sb.DrawString(GameGlobal.gameFont, "Name", pos, Color.Wheat);
            //In Stock column
            stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(columnHeaders[horzIndex]);
            int width = ((dbPanelWidth / 2) - (dbPanelWidth / 4)) / 2;
            //center the text based on string dimensions and button bounds
            pos = new Vector2(((dbPanelWidth / 4) + width) - (stringMeas.X / 2),
                (dbPanelHeight / 20) - (stringMeas.Y / 2));
            sb.DrawString(GameGlobal.gameFont, columnHeaders[horzIndex], pos, Color.Wheat);
            //Demand Column ("cur/proj")
            stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(columnHeaders[horzIndex+1]);
            width = ((3 * dbPanelWidth / 4) - (dbPanelWidth / 2)) / 2;
            //center the text based on string dimensions and button bounds
            pos = new Vector2(((dbPanelWidth / 2) + width) - (stringMeas.X / 2),
                (dbPanelHeight / 20) - (stringMeas.Y / 2));
            sb.DrawString(GameGlobal.gameFont, columnHeaders[horzIndex+1], pos, Color.Wheat);
            //Your Price Column
            stringMeas = GameLogic.GameGlobal.gameFont.MeasureString(columnHeaders[horzIndex+2]);
            width = (dbPanelWidth - (3 * dbPanelWidth / 4)) / 2;
            //center the text based on string dimensions and button bounds
            pos = new Vector2(((3 * dbPanelWidth / 4) + width) - (stringMeas.X / 2),
                (dbPanelHeight / 20) - (stringMeas.Y / 2));
            sb.DrawString(GameGlobal.gameFont, columnHeaders[horzIndex+2], pos, Color.Wheat);
        }

        private void drawDataInTable(SpriteBatch sb)
        {
            Rectangle cellRect;
            Vector2 stringMeas;
            string drawingString = "";
            int cellWidth = dbPanelWidth / 4;
            int cellHeight = dbPanelHeight / 10;

            for (int yCell = 0; yCell < 9; yCell++)
            {
                if (yCell >= numEntries)
                {
                    return;
                }
                for (int xCell = 0; xCell < 4; xCell++)
                {
                    cellRect = new Rectangle((xCell * cellWidth), (yCell + 1) * cellHeight, cellWidth, cellHeight);
                    //get string length
                    if (xCell == 0)
                    {
                        drawingString = items.Rows[yCell + vertIndex].ItemArray[1].ToString().Trim();
                        stringMeas = GameGlobal.gameFont.MeasureString(drawingString);
                        if (stringMeas.X >= cellWidth)
                        {
                            drawingString = truncateString(drawingString, cellRect.Width);
                        }
                    }
                    else
                    {
                        //adjust column drawing for changed demand
                        if ((horzIndex + xCell + 1) == 4)
                        {
                            drawingString = items.Rows[yCell + vertIndex].ItemArray[horzIndex + xCell + 1].ToString().Trim() + "\\"+
                                items.Rows[yCell + vertIndex].ItemArray[15].ToString().Trim();
                            stringMeas = GameGlobal.gameFont.MeasureString(drawingString);
                            if (stringMeas.X >= cellWidth)
                            {
                                drawingString = truncateString(drawingString, cellRect.Width);
                            }
                        }
                        else
                        {
                            drawingString = items.Rows[yCell + vertIndex].ItemArray[horzIndex + xCell + 1].ToString().Trim();
                            stringMeas = GameGlobal.gameFont.MeasureString(drawingString);
                            if (stringMeas.X >= cellWidth)
                            {
                                drawingString = truncateString(drawingString, cellRect.Width);
                            }
                        }
                    }
                    sb.DrawString(GameGlobal.gameFont, drawingString, new Vector2(cellRect.Left, cellRect.Top), Color.Aquamarine);
                }
            }
        }

        /// <summary>
        /// Truncates a string to fit into the database table view.
        /// </summary>
        /// <param name="s">The string to truncate.</param>
        /// <param name="cellWidth">The width of a cell to compare against.</param>
        /// <returns>The truncated string.</returns>
        public string truncateString(string s, int cellWidth)
        {
            string newString = s;
            string finalString;
            Vector2 newStringMeas;
            do
            {
                newString = newString.Remove(newString.Length - 1);
                finalString = newString + "...";
                newStringMeas = GameGlobal.gameFont.MeasureString(finalString);
            } while (newStringMeas.X >= cellWidth);
            return finalString;
        }

        private void drawRightPanel(SpriteBatch sb)
        {
            Rectangle rightPanel = new Rectangle((int)(GameGlobal.GameWidth * .6f), queryWriterBtn.Bounds.Bottom,
                GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * .6f), orderBtn.Bounds.Top - queryWriterBtn.Bounds.Bottom);
            Vector2 stringPos = GameGlobal.gameFont.MeasureString(vertIndex + "/" + items.Rows.Count);
            //draw index
            sb.DrawString(GameGlobal.gameFont, (vertIndex + 1) + "/" + numEntries,
                new Vector2((rightPanel.Center.X) - (stringPos.X / 2), rightPanel.Top + (stringPos.Y / 2)),
                    Color.Wheat);

            //divide rest of panel into 6 even segments
            int vertSpacing = (rightPanel.Bottom - (int)(rightPanel.Top + (stringPos.Y / 2))) / 8;
            int drawHeight = (int)(rightPanel.Top + (stringPos.Y / 2) + vertSpacing);

            //draw PPU
            if (items.Rows.Count > 0)
            {
                sb.DrawString(GameGlobal.gameFont, "   Price Per Unit: " + items.Rows[selectedID].ItemArray[2].ToString(),
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw & calc current value
                int curVal = (int)(items.Rows[selectedID].ItemArray[2]) * (short)(items.Rows[selectedID].ItemArray[7]);
                sb.DrawString(GameGlobal.gameFont, "    Current Value: " + curVal,
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw asking price (needs textfield)
                sb.DrawString(GameGlobal.gameFont, "     Asking Price: ",
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw & calc projected revenue
                sb.DrawString(GameGlobal.gameFont, "Projected Revenue: " + projRev,
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw order amount (needs textfield)
                sb.DrawString(GameGlobal.gameFont, "     Order Amount: ",
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw order total cost
                //make sure there is an amount in the text field before trying to parse.
                if (orderAmtField.Text.Length > 0)
                {
                    orderCost = Int32.Parse(orderAmtField.Text) * (int)items.Rows[selectedID].ItemArray[2];
                    sb.DrawString(GameGlobal.gameFont, "      Order Total: " + orderCost,
                        new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                }
                else
                {
                    orderCost = 0;
                    sb.DrawString(GameGlobal.gameFont, "      Order Total: 0",
                        new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                }
            }
            else
            {
                sb.DrawString(GameGlobal.gameFont, "   Price Per Unit: " ,
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw & calc current value
                sb.DrawString(GameGlobal.gameFont, "    Current Value: " ,
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw asking price (needs textfield)
                sb.DrawString(GameGlobal.gameFont, "     Asking Price: ",
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw & calc projected revenue
                sb.DrawString(GameGlobal.gameFont, "Projected Revenue: " + projRev,
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw order amount (needs textfield)
                sb.DrawString(GameGlobal.gameFont, "     Order Amount: ",
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
                drawHeight += vertSpacing;
                //draw order total cost
                //make sure there is an amount in the text field before trying to parse.
                sb.DrawString(GameGlobal.gameFont, "      Order Total: 0",
                    new Vector2(rightPanel.Right * 0.01f + rightPanel.Left, drawHeight), Color.Wheat);
            }
        }

        private void drawLeftPanel(SpriteBatch sb)
        {
            int heightStep = leftPanelHeight / 3;

            if (items.Rows.Count > 0)
            {
                sb.DrawString(GameGlobal.gameFont, "Name:   " + items.Rows[selectedID].ItemArray[1].ToString(),
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f)), Color.Aquamarine);

                sb.DrawString(GameGlobal.gameFont, "ID:     " + items.Rows[selectedID].ItemArray[0].ToString(),
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f + heightStep)), Color.Aquamarine);

                sb.DrawString(GameGlobal.gameFont, "Weight: " + items.Rows[selectedID].ItemArray[10].ToString(),
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f) + (2 * heightStep)), Color.Aquamarine);

                if ((byte)items.Rows[selectedID].ItemArray[11] < Graphics.GlobalGfx.iconTextures.Count)
                {
                    sb.Draw(Graphics.GlobalGfx.iconTextures[(byte)items.Rows[selectedID].ItemArray[11]],
                        new Rectangle((int)(leftPanelWidth / 1.5), (int)((GameGlobal.GameHeight * .725f) + (1 * heightStep)), 32, 32),
                        new Rectangle((byte)items.Rows[selectedID].ItemArray[12] * 32, (byte)items.Rows[selectedID].ItemArray[13] * 32, 32, 32),
                        Color.White);
                }
            }
            else
            {
                sb.DrawString(GameGlobal.gameFont, "Name:   " ,
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f)), Color.Aquamarine);

                sb.DrawString(GameGlobal.gameFont, "ID:     " ,
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f + heightStep)), Color.Aquamarine);

                sb.DrawString(GameGlobal.gameFont, "Weight: " ,
                    new Vector2(GameGlobal.GameWidth * 0.025f, (GameGlobal.GameHeight * .725f) + (2 * heightStep)), Color.Aquamarine);
            }
        }

        /// <summary>
        /// Gets where the user clicks in the grid view and retrieves item data.
        /// </summary>
        private void getSelectedID()
        {
            if (GameGlobal.InputControl.CurrentMouseState.X >= 0 &&
                GameGlobal.InputControl.CurrentMouseState.X <= dbPanelWidth)
            {
                if (GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
                {
                    int spacing = dbPanelHeight / 10;
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y <= (2 * spacing))
                    {
                        selectedID = vertIndex;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (2 * spacing) &&
                        GameGlobal.InputControl.CurrentMouseState.Y <= (3 * spacing))
                    {
                        selectedID = vertIndex + 1;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (3 * spacing) &&
                             GameGlobal.InputControl.CurrentMouseState.Y <= (4 * spacing))
                    {
                        selectedID = vertIndex + 2;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (4 * spacing) &&
                             GameGlobal.InputControl.CurrentMouseState.Y <= (5 * spacing))
                    {
                        selectedID = vertIndex + 3;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (5 * spacing) &&
                            GameGlobal.InputControl.CurrentMouseState.Y <= (6 * spacing))
                    {
                        selectedID = vertIndex + 4;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (6 * spacing) &&
                            GameGlobal.InputControl.CurrentMouseState.Y <= (7 * spacing))
                    {
                        selectedID = vertIndex + 5;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (7 * spacing) &&
                            GameGlobal.InputControl.CurrentMouseState.Y <= (8 * spacing))
                    {
                        selectedID = vertIndex + 6;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (8 * spacing) &&
                            GameGlobal.InputControl.CurrentMouseState.Y <= (9 * spacing))
                    {
                        selectedID = vertIndex + 7;
                    }
                    else if (GameGlobal.InputControl.CurrentMouseState.Y >= (9 * spacing) &&
                            GameGlobal.InputControl.CurrentMouseState.Y <= (10 * spacing))
                    {
                        selectedID = vertIndex + 8; 
                    }
                }
            }
        }

        /// <summary>
        /// Adjusts the highlighted rectangle position.
        /// </summary>
        private void updateSelectedRectangle()
        {
            int spacing = dbPanelHeight / 10;
            if (selectedID == vertIndex)
            {
                selectedRect.Y = spacing;
            }
            else if (selectedID == (vertIndex + 1))
            {
                selectedRect.Y = (2 * spacing);
            }
            else if (selectedID == vertIndex + 2)
            {
                selectedRect.Y = (3 * spacing);
            }
            else if (selectedID == vertIndex + 3)
            {
                selectedRect.Y = (4 * spacing);
            }
            else if (selectedID == vertIndex + 4)
            {
                selectedRect.Y = (5 * spacing);
            }
            else if (selectedID == vertIndex + 5)
            {
                selectedRect.Y = (6 * spacing);
            }
            else if (selectedID == vertIndex + 6)
            {
                selectedRect.Y = (7 * spacing);
            }
            else if (selectedID == vertIndex + 7)
            {
                selectedRect.Y = (8 * spacing);
            }
            else if (selectedID == vertIndex + 8)
            {
                selectedRect.Y = (9 * spacing);
            }
            else
            {
                selectedRect.Y = -200;
            }
        }


        public override void UnloadView()
        {
            scrollThumb = Rectangle.Empty;
            selectedRect = Rectangle.Empty;
            for (int u = 0; u < columnHeaders.Count; u++)
            {
                columnHeaders[u] = "";
            }
            closeBtn.unloadBtn();
            scrollUpBtn.updateButton();
            scrollDownBtn.unloadBtn();
            scrollLeftBtn.unloadBtn();
            scrollRightBtn.unloadBtn();
            queryWriterBtn.unloadBtn();
            orderBtn.unloadBtn();
            weeksToOrder.unloadField();
            defaultDBBtn.unloadBtn();
            jumpToIndexField.unloadField();
            searchByNameField.unloadField();
            orderAmtField.unloadField();
            priceField.unloadField();
            confirmOrder.unloadDialogBox();
            items.Dispose();
        }
    }
}
