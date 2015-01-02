using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.MainGamePlay
{
    public class RoomInfoPanel
    {
        private Rooms.Room baseRoom;
        private Rectangle panelBounds;
        private InventoryDisplay id;
        private OfficeMenuClasses.Menu menu;
        private bool showInv = false;

        #region Type 1 Vars
        //Controls
        private Input.SFButton upScrollBtn, downScrollBtn;
        private Input.SFButton removeItemBtn;
        //room casted
        private Rooms.DisplayRoom shelfTypeRoom;
        //indexes and states
        private int startItemIndex = 0;
        private int selectedIndex = -1;
        private int selectedRoomIndex;
        private int editIndex;
        private bool removeState = false;
        #endregion

        #region Attributes

        public Rooms.Room DisplayRoom
        {
            get { return baseRoom; }
            set 
            { 
                baseRoom = value;
                loadInfoPanel();
            }
        }

        public bool InventoryOpen
        {
            get { return showInv; }
        }

        public int SelectedRoomIndex
        {
            get { return selectedRoomIndex; }
            set { selectedRoomIndex = value; }
        }

        #endregion

        //default constructor
        public RoomInfoPanel()
        {
            panelBounds = new Rectangle(0, (int)(GameGlobal.GameHeight * 0.7f),
                GameGlobal.GameWidth, GameGlobal.GameHeight - (int)(GameGlobal.GameHeight * 0.7f));
            id = new InventoryDisplay();
            id.initInvDisplay();
        }

        //constructor to pass in a room for display
        public RoomInfoPanel(Rooms.Room displayRoom)
        {
            this.baseRoom = displayRoom;
            //set the bounds of the panel
            panelBounds = new Rectangle(0, (int)(GameGlobal.GameHeight * 0.7f), 
                GameGlobal.GameWidth, GameGlobal.GameHeight - (int)(GameGlobal.GameHeight * 0.7f));
            id = new InventoryDisplay();
            id.initInvDisplay();
        }

        public void loadInfoPanel()
        {
            //switch to determine what type of room is being displayed.
            if (baseRoom != null)
            {
                switch (baseRoom.RoomType())
                {
                    case 1:
                        typeOneLoad();

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:
                        menu = new OfficeMenuClasses.Menu(panelBounds);
                        menu.loadMenu();
                        break;
                    case 8:

                        break;
                    default:
                        break;
                }
            }
        }

        public void updateInfoPanel(GameTime gt, ContentManager cm)
        {
            //switch to determine what type of room is being displayed.
            if (baseRoom != null)
            {
                switch (baseRoom.RoomType())
                {
                    case 1:
                        typeOneUpdate(gt);
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:
                        //open menu
                        menu.updateMenu(gt, cm);
                        break;
                    case 8:

                        break;
                    default:
                        break;
                }
            }
        }

        public void drawInfoPanel(SpriteBatch sb, GameTime gt)
        {
            //switch to determine what type of room is being displayed.
            if (baseRoom != null)
            {
                switch (baseRoom.RoomType())
                {
                    case 1:
                        typeOneDraw(sb, gt);
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:
                        menu.drawMenu(sb, gt);
                        break;
                    case 8:

                        break;
                    default:
                        break;
                }
            }
        }

        #region Type 1 Functions
        private void typeOneLoad()
        {
            upScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                panelBounds.Top, (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)), 
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, 
                "^", false, true, true);
            downScrollBtn = new Input.SFButton(new Rectangle(GameGlobal.GameWidth - (int)(GameGlobal.GameWidth * 0.05f),
                panelBounds.Bottom - (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.05f)),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive,
                "V", false, true, true);

            removeItemBtn = new Input.SFButton(new Rectangle(panelBounds.Center.X - (int)(GameGlobal.GameWidth * 0.085f) - 1,
                GameGlobal.GameHeight - (int)(GameGlobal.GameWidth * 0.05f), (int)(GameGlobal.GameWidth * 0.085f),
                (int)(GameGlobal.GameWidth * 0.05f)), Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                Graphics.GlobalGfx.btnActive, "Remove", true, true, true);

            //since this is a type 1 room, cast it to a DisplayRoom for needed functions.
            shelfTypeRoom = (Rooms.DisplayRoom)baseRoom;
        }

        private void typeOneUpdate(GameTime gt)
        {
            if (showInv) //if the inventory is being displayed
            {
                id.updateInvDisplay(gt); //update the inventory screen.
                if (id.IsClosed) //if the event to close the inventory is called.
                {
                    showInv = false; //toggle the inventory switch off.
                    //add item into shelf if display item is not null
                    if (id.AddedItem != null)
                    {
                        //search for the item already on the shelf (compares names)
                        int foundIndex = shelfTypeRoom.Inventory.FindIndex(
                                            delegate(Database.ShelfItem si)
                                            {
                                                return si.Name == id.AddedItem.Name;
                                            });
                        //if the item was already found on the shelf.
                        if (foundIndex > -1)
                        {
                            //add it to the original placement.
                            shelfTypeRoom.Inventory[foundIndex].OnShelf += id.AddedItem.OnShelf;
                            shelfTypeRoom.Inventory[foundIndex].Weight += id.AddedItem.Weight;
                        }
                        else
                        {
                            //add it to the end if clicking past the bounds of the inventory
                            if (editIndex >= shelfTypeRoom.Inventory.Count)
                            {
                                shelfTypeRoom.Inventory.Add(id.AddedItem);
                            }
                            else //replace the clicked index with the new item
                            {
                                shelfTypeRoom.Inventory[editIndex] = id.AddedItem;
                            }
                        }
                    }
                }
            }
            else
            {
                //update buttons to scroll
                upScrollBtn.updateButton();
                downScrollBtn.updateButton();
                removeItemBtn.updateButton();

                //scrolling on button click
                #region Scrolling Through Inventory
                if (downScrollBtn.isDown())
                {
                    if (startItemIndex < shelfTypeRoom.Inventory.Count - 3)
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

                //Removing items on button click
                if (removeItemBtn.isDown())
                {
                    removeState = true;
                }

                //get which slot was clicked
                selectedIndex = listClick();
                //if a slot was actually clicked.
                if (selectedIndex > -1)
                {
                    if (!removeState)
                    {
                        //Program.gameConsole.AddLine("EDIT ME!" + selectedIndex, Color.Turquoise);
                        //hold the index of the clicked item (to replace if needed)
                        editIndex = selectedIndex;
                        //pass the selected room index to the inventory to determine what types of items to show.
                        id.SelectedRoomIndex = selectedRoomIndex;
                        //reset the selected slot index
                        selectedIndex = -1;
                        //toggle inventory display
                        showInv = true;
                        //load the inventory.
                        id.loadInvDisplay();
                    }
                    else
                    {
                        //remove an item.
                        removeState = false;
                        removeItemBtn.ButtonState = Input.SFButtonState.Up;
                        if (selectedIndex < shelfTypeRoom.Inventory.Count)
                        {
                            shelfTypeRoom.Inventory.RemoveAt(selectedIndex);
                        }
                    }
                }
            }
        }

        private void typeOneDraw(SpriteBatch sb, GameTime gt)
        {
            if (showInv)
            {
                id.drawInvDisplay(sb, gt);
            }
            else
            {
                sb.Begin();

                //draw inventory
                drawItemTable(sb);

                int vertSpace = (int)GameGlobal.gameFont.MeasureString(" ").Y;

                //draw name
                sb.DrawString(GameGlobal.gameFont, baseRoom.RoomID,
                    new Vector2(0, panelBounds.Top), Color.Gray);

                //draw durabilities
                sb.DrawString(GameGlobal.gameFont, "Current Durability: " + shelfTypeRoom.CurrentDurability,
                    new Vector2(0, panelBounds.Top + vertSpace), Color.Gray);
                sb.DrawString(GameGlobal.gameFont, "Maximum Durability: " + shelfTypeRoom.MaximumDurability,
                    new Vector2(0, panelBounds.Top + (2 * vertSpace)), Color.Gray);
                //draw weight amounts
                sb.DrawString(GameGlobal.gameFont, "Current Weight: " + shelfTypeRoom.CurrentWeight,
                    new Vector2(0, panelBounds.Top + (3 * vertSpace)), Color.Gray);
                sb.DrawString(GameGlobal.gameFont, "Maximum Weight: " + shelfTypeRoom.MaximumWeight,
                    new Vector2(0, panelBounds.Top + (4 * vertSpace)), Color.Gray);
                //draw capacities
                sb.DrawString(GameGlobal.gameFont, "Current Capacity: " + shelfTypeRoom.CurrentCapacity,
                    new Vector2(0, panelBounds.Top + (5 * vertSpace)), Color.Gray);
                sb.DrawString(GameGlobal.gameFont, "Maximum Capacity: " + shelfTypeRoom.MaximumCapacity,
                    new Vector2(0, panelBounds.Top + (6 * vertSpace)), Color.Gray);

                sb.End();

                upScrollBtn.drawButton(sb, true);
                downScrollBtn.drawButton(sb, true);
                removeItemBtn.drawButton(sb, true);
            }
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
                if (sp < shelfTypeRoom.Inventory.Count)
                {
                    //draw all data in the rectangle
                    sb.DrawString(GameGlobal.gameFont, shelfTypeRoom.Inventory[sp].Name,
                        new Vector2(panelBounds.Center.X, positionStart.Y), Color.Yellow);
                    sb.DrawString(GameGlobal.gameFont, shelfTypeRoom.Inventory[sp].Cost.ToString(),
                        new Vector2(upScrollBtn.Bounds.Left -
                            GameGlobal.gameFont.MeasureString(shelfTypeRoom.Inventory[sp].Cost.ToString()).X,
                            positionStart.Y), Color.Yellow);
                    sb.DrawString(GameGlobal.gameFont, shelfTypeRoom.Inventory[sp].Weight.ToString(),
                        new Vector2(upScrollBtn.Bounds.Left -
                            GameGlobal.gameFont.MeasureString(shelfTypeRoom.Inventory[sp].Weight.ToString()).X,
                            positionStart.Y + GameGlobal.gameFont.MeasureString(shelfTypeRoom.Inventory[sp].Weight.ToString()).Y),
                            Color.Yellow);
                    sb.DrawString(GameGlobal.gameFont, shelfTypeRoom.Inventory[sp].OnShelf.ToString(),
                        new Vector2(panelBounds.Center.X, positionStart.Y +
                            GameGlobal.gameFont.MeasureString(shelfTypeRoom.Inventory[sp].OnShelf.ToString()).Y),
                            Color.Yellow);
                    positionStart.Y += itemListSpacing;
                }
                else
                {
                    if (shelfTypeRoom.CurrentCapacity < shelfTypeRoom.MaximumCapacity)
                    {
                        sb.DrawString(GameGlobal.gameFont, "Click to add new item.",
                            new Vector2(panelBounds.Center.X, positionStart.Y), Color.Yellow);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Handles getting which item is clicked on in the display.
        /// </summary>
        /// <returns>The zero-based index of the clicked item.</returns>
        private int listClick()
        {
            int itemListSpacing = panelBounds.Height / 4;
            if (GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
            {
                if (GameGlobal.InputControl.CurrentMouseState.X < upScrollBtn.Bounds.Left &&
                    GameGlobal.InputControl.CurrentMouseState.X > panelBounds.Center.X &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (panelBounds.Top + itemListSpacing) &&
                    GameGlobal.InputControl.CurrentMouseState.Y > panelBounds.Top)
                {
                    if (shelfTypeRoom.CurrentCapacity < shelfTypeRoom.MaximumCapacity || 
                        ((shelfTypeRoom.CurrentCapacity == shelfTypeRoom.MaximumCapacity) &&
                        startItemIndex < shelfTypeRoom.Inventory.Count))
                    {
                        return startItemIndex;
                    }
                }
                if (GameGlobal.InputControl.CurrentMouseState.X < upScrollBtn.Bounds.Left &&
                    GameGlobal.InputControl.CurrentMouseState.X > panelBounds.Center.X &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (panelBounds.Top + (2 * itemListSpacing)) &&
                    GameGlobal.InputControl.CurrentMouseState.Y > (panelBounds.Top + itemListSpacing))
                {
                    if (shelfTypeRoom.CurrentCapacity < shelfTypeRoom.MaximumCapacity ||
                        ((shelfTypeRoom.CurrentCapacity == shelfTypeRoom.MaximumCapacity) &&
                        startItemIndex < shelfTypeRoom.Inventory.Count))
                    {
                        return startItemIndex + 1;
                    }
                }
                if (GameGlobal.InputControl.CurrentMouseState.X < upScrollBtn.Bounds.Left &&
                    GameGlobal.InputControl.CurrentMouseState.X > panelBounds.Center.X &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (panelBounds.Top + (3 * itemListSpacing)) &&
                    GameGlobal.InputControl.CurrentMouseState.Y > (panelBounds.Top + (2 * itemListSpacing)))
                {
                    if (shelfTypeRoom.CurrentCapacity < shelfTypeRoom.MaximumCapacity ||
                        ((shelfTypeRoom.CurrentCapacity == shelfTypeRoom.MaximumCapacity) &&
                        startItemIndex < shelfTypeRoom.Inventory.Count))
                    {
                        return startItemIndex + 2;
                    }
                }
                if (GameGlobal.InputControl.CurrentMouseState.X < upScrollBtn.Bounds.Left &&
                    GameGlobal.InputControl.CurrentMouseState.X > panelBounds.Center.X &&
                    GameGlobal.InputControl.CurrentMouseState.Y < (panelBounds.Top + (4 * itemListSpacing)) &&
                    GameGlobal.InputControl.CurrentMouseState.Y > (panelBounds.Top + (3 * itemListSpacing)))
                {
                    if (shelfTypeRoom.CurrentCapacity < shelfTypeRoom.MaximumCapacity ||
                        ((shelfTypeRoom.CurrentCapacity == shelfTypeRoom.MaximumCapacity) &&
                        startItemIndex < shelfTypeRoom.Inventory.Count))
                    {
                        return startItemIndex + 3;
                    }
                }
            }
            return -1;
        }
        #endregion

        #region Type 2 Functions

        #endregion

        #region Type 3 Functions

        #endregion

        #region Type 4 Functions

        #endregion

        #region Type 5 Functions

        #endregion

        #region Type 6 Functions

        #endregion
    }
}
