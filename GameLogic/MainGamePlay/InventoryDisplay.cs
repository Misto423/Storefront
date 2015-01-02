using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.MainGamePlay
{
    public class InventoryDisplay
    {
        private int curCapacity, maxCapacity;
        private Database.DBConnector dbc;
        private System.Data.DataTable items;
        private bool displayMode = true; //false to just browse inventory, true to select items
        //and place them on a shelf.
        private byte typeToDisplay = 0;

        //inventory numbers vars
        private int inStorage, onShelf;

        //placing items
        private bool close = false;
        private Database.ShelfItem displayItem;
        private int selectedRoomIndex;

        private Input.SFButton closeBtn;
        private Input.SFTextField searchByNameField;
        private Input.SFButton upScrollBtn, downScrollBtn;
        private Input.SFButton acceptBtn; //for true displayMode only
        private Input.SFTextField amtField; //for true displayMode only

        //display only vars
        private int spacing;
        private string query;
        private int selectedIndex = -1, startDisplayIndex = 0;
        private bool displaySelectedData = false;
        private string val = "", weight = "";
        private Rectangle highlightSelectedRect;

        #region Attributes
        public int CurrentCapacity
        {
            get { return curCapacity; }
            set { curCapacity = value; }
        }
        public int MaximumCapacity
        {
            get { return maxCapacity; }
            set { maxCapacity = value; }
        }
        public bool DisplayMode
        {
            get { return displayMode; }
            set { displayMode = value; }
        }
        public byte DisplayType
        {
            get { return typeToDisplay; }
            set { typeToDisplay = value; }
        }
        public bool IsClosed
        {
            get { return close; }
        }
        public Database.ShelfItem AddedItem
        {
            get { return displayItem; }
        }
        public int SelectedRoomIndex
        {
            get { return selectedRoomIndex; }
            set { selectedRoomIndex = value; }
        }
        #endregion

        //Call once when initialized
        public void initInvDisplay()
        {
            dbc = new Database.DBConnector();
            //calculate table spacings (used in button locations and line drawings).
            spacing = (int)((GameGlobal.GameHeight - (GameGlobal.GameHeight * 0.7f)) / 2.65f);

            //load controls
            upScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                0, (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "^",
                false, true, true);

            downScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                (int)(spacing * 7) - (int)(GameGlobal.GameWidth * 0.05f),
                (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "V",
                false, true, true);

            searchByNameField = new Input.SFTextField(new Rectangle(0, (int)(spacing * 7),
                (int)((GameGlobal.GameWidth * .9f) - (GameGlobal.GameWidth * .6f)), (int)(GameGlobal.GameHeight * .15f / 2)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 20);
            searchByNameField.ResetFocus = true;
            searchByNameField.Text = "Search By Name";

            closeBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth * .8f),
                                        (int)(GameGlobal.GameHeight / 1.0833f),
                                        (int)(GameGlobal.GameWidth - (GameGlobal.GameWidth * .8f)),
                                        (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0833f))),
                                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                                "Close", false, true, true);

            acceptBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth * .8f),
                            closeBtn.Bounds.Top - (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0833f)),
                            (int)(GameGlobal.GameWidth - (GameGlobal.GameWidth * .8f)),
                            (int)(GameGlobal.GameHeight - (GameGlobal.GameHeight / 1.0833f))),
                          Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                          "Accept", false, true, true);

            amtField = new Input.SFTextField(new Rectangle(0, GameGlobal.GameHeight - (int)(GameGlobal.GameHeight * .15f / 2),
                    (int)((GameGlobal.GameWidth * .9f) - (GameGlobal.GameWidth * .6f)), (int)(GameGlobal.GameHeight * .15f / 2)),
                    Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.fieldCursor, 3);
            amtField.ResetFocus = true;
            amtField.NumbersOnly = true;
            amtField.Text = "Amount to Display";

            //rectangle for highlighted selection
            highlightSelectedRect = new Rectangle(0, -200, upScrollBtn.Bounds.Left, spacing);
        }

        //call every time view is opened
        public void loadInvDisplay()
        {
            //query to get inventory
            query = QueryBuilder(String.Empty);
            //get most recent update of items.
            items = dbc.runQuery(query);
            close = false;
        }

        public void updateInvDisplay(GameTime gt)
        {
            upScrollBtn.updateButton();
            downScrollBtn.updateButton();
            closeBtn.updateButton();
            searchByNameField.updateText();
            //if adding items to a shelf, show the accept button and amount field
            if (displayMode)
            {
                acceptBtn.updateButton();
                amtField.updateText();
            }

            //scroll up
            if (upScrollBtn.isDown())
            {
                if (startDisplayIndex > 0)
                {
                    startDisplayIndex--;
                }
            }

            //scroll down
            if (downScrollBtn.isDown())
            {
                if (startDisplayIndex < (items.Rows.Count - 7))
                {
                    startDisplayIndex++;
                }
            }

            //search by name
            if (searchByNameField.Focus)
            {
                if (GameGlobal.InputControl.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    //run a new query, searching for items by name
                    startDisplayIndex = 0;
                    query = QueryBuilder(searchByNameField.Text);
                    items = dbc.runQuery(query);
                }
            }

            //stuff to do when adding items to shelf
            if (displayMode)
            {
                getSelectedField();
                //update rectangle
                updateHighlighRectangle();
                //toggle data display
                if (selectedIndex >= 0 && selectedIndex < items.Rows.Count)
                {
                    displaySelectedData = true;
                }
                else
                {
                    displaySelectedData = false;
                }
                //reset item
                displayItem = null;
                //handle accept button
                if (acceptBtn.isDown())
                {
                    if (selectedIndex >= 0 && selectedIndex < items.Rows.Count)
                    {
                        //make sure the player has enough to place in the shelf.
                        onShelf = determineOnShelf(items.Rows[selectedIndex].ItemArray[1].ToString(),
                                (byte)items.Rows[selectedIndex].ItemArray[14]);
                        if (Int32.Parse(amtField.Text) <= ((short)items.Rows[selectedIndex].ItemArray[3] - onShelf))
                        {
                            //create the ShelfItem object for the shelf, and close the inventory view
                            displayItem = new Database.ShelfItem(
                                items.Rows[selectedIndex].ItemArray[1].ToString(),
                                Int32.Parse(amtField.Text), (int)items.Rows[selectedIndex].ItemArray[6],
                                ((float)items.Rows[selectedIndex].ItemArray[10] * Int32.Parse(amtField.Text)), 
                                (short)items.Rows[selectedIndex].ItemArray[4]);
                            closeView();
                        }
                        else
                        {
                            //show dialog box error message
                            //not enough in stock
                            
                        }
                    }
                }
            }

            //handle close button
            if (closeBtn.isDown())
            {
                closeView();
            }
        }

        public void drawInvDisplay(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            //vertical line separating scroll bar and table.
            Graphics.SimpleDraw.drawLine(sb, new Vector2(upScrollBtn.Bounds.Left, 0),
                new Vector2(upScrollBtn.Bounds.Left, spacing * 7), 1, Color.Gray);

            //draw Key
            #region Inventory Key

            //draw name of item
            sb.DrawString(GameGlobal.gameFont, "Item Name",
                new Vector2(0, 0), Color.HotPink);
            //draw number in stock of item
            sb.DrawString(GameGlobal.gameFont, "On Shelves/In Storage/Total",
                new Vector2(upScrollBtn.Bounds.Left -
                    GameGlobal.gameFont.MeasureString("On Shelves/In Storage/Total ").X,
                    0), Color.HotPink);
            //draw average competitor price for item
            sb.DrawString(GameGlobal.gameFont, "Average Competitor Price",
                new Vector2(0,
                    GameGlobal.gameFont.MeasureString(" ").Y), Color.HotPink);
            //draw demand of item
            sb.DrawString(GameGlobal.gameFont, "Current Demand/Projected Demand",
                new Vector2(upScrollBtn.Bounds.Left -
                    GameGlobal.gameFont.MeasureString("Current Demand/Projected Demand ").X,
                    GameGlobal.gameFont.MeasureString(" ").Y), Color.HotPink);
            //draw your asking price for item
            sb.DrawString(GameGlobal.gameFont, "Your Asking Price",
                new Vector2(0,
                    (2 * GameGlobal.gameFont.MeasureString(" ").Y)), Color.HotPink);
            //draw weight of item
            sb.DrawString(GameGlobal.gameFont, "Item Weight",
                new Vector2(upScrollBtn.Bounds.Left -
                    GameGlobal.gameFont.MeasureString("Item Weight ").X,
                    (2 * GameGlobal.gameFont.MeasureString(" ").Y)), Color.HotPink);

            #endregion

            //lines and table data drawn to screen.
            int invIndex = startDisplayIndex;
            int yTextSpacing = 1;
            for (int yPos = spacing; yPos < (GameGlobal.GameHeight * 0.8f); yPos += spacing)
            {
                //draw dividing line
                Graphics.SimpleDraw.drawLine(sb, new Vector2(0, yPos), new Vector2(upScrollBtn.Bounds.Left, yPos),
                    1, Color.Gray);
                if (invIndex < items.Rows.Count)
                {
                    if (yTextSpacing <= 6)
                    {
                        //draw name of item
                        sb.DrawString(GameGlobal.gameFont, items.Rows[invIndex].ItemArray[1].ToString(),
                            new Vector2(0, yTextSpacing * spacing), Color.HotPink);
                        //get number on shelves
                        onShelf = determineOnShelf(items.Rows[invIndex].ItemArray[1].ToString(), 
                            (byte)items.Rows[invIndex].ItemArray[14]);
                        //get number in storage
                        inStorage = (short)(items.Rows[invIndex].ItemArray[3]) - onShelf;
                        //draw number in stock of item
                        sb.DrawString(GameGlobal.gameFont, onShelf + "/" + inStorage + "/" + 
                            items.Rows[invIndex].ItemArray[3].ToString(),
                            new Vector2(upScrollBtn.Bounds.Left -
                                GameGlobal.gameFont.MeasureString(onShelf + "/" + inStorage + "/" +
                            items.Rows[invIndex].ItemArray[3].ToString() + " ").X,
                                yTextSpacing * spacing), Color.HotPink);
                        //draw average competitor price for item
                        sb.DrawString(GameGlobal.gameFont, items.Rows[invIndex].ItemArray[5].ToString(),
                            new Vector2(0,
                                (yTextSpacing * spacing) + GameGlobal.gameFont.MeasureString(" ").Y), Color.HotPink);
                        //draw demand of item
                        sb.DrawString(GameGlobal.gameFont, items.Rows[invIndex].ItemArray[4].ToString() + "/" + items.Rows[invIndex].ItemArray[15].ToString(),
                            new Vector2(upScrollBtn.Bounds.Left -
                                GameGlobal.gameFont.MeasureString(items.Rows[invIndex].ItemArray[4].ToString() +
                                "/" + items.Rows[invIndex].ItemArray[15].ToString() + " ").X,
                                (yTextSpacing * spacing) + GameGlobal.gameFont.MeasureString(" ").Y), Color.HotPink);
                        //draw your asking price for item
                        sb.DrawString(GameGlobal.gameFont, items.Rows[invIndex].ItemArray[6].ToString(),
                            new Vector2(0,
                                (yTextSpacing * spacing) + (2 * GameGlobal.gameFont.MeasureString(" ").Y)), Color.HotPink);
                        //draw weight of item
                        sb.DrawString(GameGlobal.gameFont, items.Rows[invIndex].ItemArray[10].ToString(),
                            new Vector2(upScrollBtn.Bounds.Left -
                                GameGlobal.gameFont.MeasureString(items.Rows[invIndex].ItemArray[10].ToString() + " ").X,
                                (yTextSpacing * spacing) + (2 * GameGlobal.gameFont.MeasureString(" ").Y)), Color.HotPink);
                    }
                }
                //increment indexes
                invIndex++;
                yTextSpacing++;
            }

            if (displayMode)
            {
                if (displaySelectedData)
                {
                    //draw the total weight and value for the specified amount.
                    if (amtField.Text.Length > 0 && amtField.Text != "Amount to Display")
                    {
                        val = ((int)items.Rows[selectedIndex].ItemArray[6] * Int32.Parse(amtField.Text)).ToString();
                        weight = ((float)items.Rows[selectedIndex].ItemArray[10] * Int32.Parse(amtField.Text)).ToString();
                    }
                    else
                    {
                        val = "";
                        weight = "";
                    }

                    //draw highlight rectangle
                    Graphics.SimpleDraw.fillArea(sb, highlightSelectedRect, Color.IndianRed * 0.35f);
                }

                //display labels
                sb.DrawString(GameGlobal.gameFont, "Total Value: " + val, new Vector2(searchByNameField.Bounds.Right +
                    (GameGlobal.GameWidth * 0.05f), (spacing * 7) + (GameGlobal.GameWidth * 0.015f)), Color.LightSteelBlue);

                sb.DrawString(GameGlobal.gameFont, "Total Weight: " + weight, new Vector2(searchByNameField.Bounds.Right +
                    (GameGlobal.GameWidth * 0.05f), (spacing * 7) + (GameGlobal.GameWidth * 0.05f)), Color.LightSteelBlue);
            }

            sb.End();

            //draw controls
            upScrollBtn.drawButton(sb, true);
            downScrollBtn.drawButton(sb, true);
            searchByNameField.drawField(sb, gt);
            closeBtn.drawButton(sb, true);

            if (displayMode)
            {
                acceptBtn.drawButton(sb, true);
                amtField.drawField(sb, gt);
            }
        }

        /// <summary>
        /// Gets the rectangle that is clicked.
        /// </summary>
        private void getSelectedField()
        {
            if (GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
            {
                if (GameGlobal.InputControl.CurrentMouseState.X >= 0 &&
                    GameGlobal.InputControl.CurrentMouseState.X <= upScrollBtn.Bounds.Left)
                {
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 2 * spacing)
                    {
                        selectedIndex = startDisplayIndex;
                    }
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= 2 * spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 3 * spacing)
                    {
                        selectedIndex = startDisplayIndex + 1;
                    }
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= 3 * spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 4 * spacing)
                    {
                        selectedIndex = startDisplayIndex + 2;
                    }
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= 4 * spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 5 * spacing)
                    {
                        selectedIndex = startDisplayIndex + 3;
                    }
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= 5 * spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 6 * spacing)
                    {
                        selectedIndex = startDisplayIndex + 4;
                    }
                    if (GameGlobal.InputControl.CurrentMouseState.Y >= 6 * spacing &&
                        GameGlobal.InputControl.CurrentMouseState.Y < 7 * spacing)
                    {
                        selectedIndex = startDisplayIndex + 5;
                    }
                }
            }
        }

        /// <summary>
        /// Update the hightlighing rectangle position.
        /// </summary>
        private void updateHighlighRectangle()
        {
            if (selectedIndex == (startDisplayIndex))
            {
                highlightSelectedRect.Y = spacing;
            }
            else if (selectedIndex == (startDisplayIndex + 1))
            {
                highlightSelectedRect.Y = (2 * spacing);
            }
            else if (selectedIndex == (startDisplayIndex + 2))
            {
                highlightSelectedRect.Y = (3 * spacing);
            }
            else if (selectedIndex == (startDisplayIndex + 3))
            {
                highlightSelectedRect.Y = (4 * spacing);
            }
            else if (selectedIndex == (startDisplayIndex + 4))
            {
                highlightSelectedRect.Y = (5 * spacing);
            }
            else if (selectedIndex == (startDisplayIndex + 5))
            {
                highlightSelectedRect.Y = (6 * spacing);
            }
            else
            {
                highlightSelectedRect.Y = -200;
            }
        }

        /// <summary>
        /// Called to close the view.
        /// </summary>
        private void closeView()
        {
            selectedIndex = -1;
            startDisplayIndex = 0;
            amtField.Text = "Amount to Display";
            searchByNameField.Text = "Search By Name";
            close = true;
        }

        /// <summary>
        /// Creates a query for the inventory items to display.
        /// </summary>
        /// <param name="nameSearch"></param>
        /// <returns></returns>
        private string QueryBuilder(string nameSearch)
        {
            string query;
            if (displayMode)
            {
                //get the room obeject and cast to DisplayRoom
                Rooms.DisplayRoom temp = (Rooms.DisplayRoom)(GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex]);
                //get the types that can be stored on this type of shelf
                List<int> showT = temp.GetDisplayTypes();
                //run a query to only show the compatible types that are in your inventory.
                query = "SELECT * FROM " + GameGlobal.player.TableName;
                if (nameSearch != String.Empty)
                {
                    query += " WHERE Name LIKE '%" + nameSearch + "%' AND";
                }
                else
                {
                    query += " WHERE";
                }

                query += " [In Stock] > 0 AND (";
                for (int i = 0; i < showT.Count; i++)
                {
                    query += "(Type = " + showT[i] + ") ";
                    if (i != showT.Count - 1)
                    {
                        query += "OR ";
                    }
                }
                query += ");";
            }
            else
            {
                query = "SELECT * FROM " + GameGlobal.player.TableName;
                if (nameSearch != String.Empty)
                {
                    query += " WHERE Name LIKE '%" + nameSearch + "%' AND";
                }
                else
                {
                    query += " WHERE";
                }

                query += " [In Stock] > 0;";
            }
            return query;
        }

        /// <summary>
        /// Calculates the number that is on shelves and sitting in storage (stuff that can be placed) from
        /// the number in stock and searching through pieces.
        /// </summary>
        private int determineOnShelf(string itemName, int itemType)
        {
            //reset shelved to zero
            int shelved = 0;
            //go through each room that is placed in the store.
            foreach (Rooms.Room r in GameGlobal.player.PlayerStore.RoomList)
            {
                //Display room object for casting.
                Rooms.DisplayRoom dr;
                //if the store type is 1, it is a display room
                if (r.RoomType() == 1)
                {
                    //cast it to a display room
                    dr = (Rooms.DisplayRoom)r;
                }
                else { continue; } //if not a display room, move to the next room in the list.
                //get the list of type the room can display
                List<int> displayTypes = dr.GetDisplayTypes();
                //if it can display the type of item you are looking for, look for the actual item.
                if (displayTypes.Contains(itemType))
                {
                    //get the index of the item the function is searching for.
                    int index = dr.Inventory.FindIndex(
                         delegate(Database.ShelfItem si)
                        {
                            return si.Name == itemName;
                        });
                    //if the item is found.
                    if (index > -1)
                    {
                        //increment the shelved var with how many are on that shelf.
                        shelved += dr.Inventory[index].OnShelf;
                    }
                }
                //procede to next room, or exit loop if no more rooms in list.
            }
            //return the total amount on shelves.
            return shelved;
        }
    }
}
