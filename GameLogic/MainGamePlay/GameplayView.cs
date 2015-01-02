using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.MainGamePlay
{
    public class GameplayView
    {
        #region Vars
        Random rand = new Random();
        //map vars
        private Vector2 doorCoord;
        private Vector2 layoutCoord = new Vector2();
        private Vector2 startCoord = new Vector2();
        private int layoutWidth, layoutHeight;
        //info panel vars
        /* what should be displayed in the info panel
         * 0 - Nothing
         * 1 - Room Info
         * 2 - Item Browser (to place items in shelves)
         * 3 - Customer Info
         * 4 - Event
         */
        private byte selectionType = 1;
        private sbyte selectedRoom = -1;
        private RoomInfoPanel rip;
        private CustomerDisplay cd;
        
        //Customer Vars
        private List<Customer> activeCustomers = new List<Customer>();
        private const int CST_NORMAL = 5;
        private const int CST_FAST = 1;
        private float timerCount = 0;
        private const byte MAX_CUSTOMERS = 15;

        //GUI Buttons
        private Input.SFButton pauseBtn;
        private Input.SFButton playBtn;
        private Input.SFButton ffBtn;
        private Input.SFRadioButtons timeBtns;

        //Customer Selection
        private int cIndex = -1;
        private Texture2D chighlight;

        #endregion

        #region Attributes

        public int SelectedRoomIndex
        {
            get { return selectedRoom; }
        }

        #endregion

        public GameplayView()
        {
            //variable initialization for drawing the map grid.
            layoutWidth = GameGlobal.player.PlayerStore.BuildingArray.GetLength(1) * GameGlobal.TILESIZE;
            layoutHeight = GameGlobal.player.PlayerStore.BuildingArray.GetLength(0) * GameGlobal.TILESIZE;
            startCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
            startCoord.Y = (GameGlobal.GameWidth / 3.5f) - (layoutHeight / 2);

            doorCoord = new Vector2((GameGlobal.player.PlayerStore.DoorPosition.X * 32) + startCoord.X,
                (GameGlobal.player.PlayerStore.DoorPosition.Y * 32) + startCoord.Y);

            //setup the RoomInfoPanel
            rip = new RoomInfoPanel();
            //rip.SelectedRoomIndex = selectedRoom;
            //rip.loadInfoPanel();

        }

        public void loadGV(ContentManager cm)
        {
            //load player store (for store exclusive textures)
            GameGlobal.player.PlayerStore.loadStore(cm);
            //load customer highlight UI piece
            chighlight = cm.Load<Texture2D>(@"MiscGfx/highlight");
            //load GUI buttons
            //load pause button
            pauseBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth / 1.3f), 
                                    GameGlobal.GameHeight / 16, GameGlobal.TILESIZE, GameGlobal.TILESIZE),
                       cm.Load<Texture2D>(@"ControlAssets/pauseBtn"), 
                       Graphics.GlobalGfx.btnHover,
                       Graphics.GlobalGfx.btnActive,
                       "", true, true, true);
            //load play button
            playBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth / 1.3f) + 
                                    pauseBtn.Bounds.Width + GameGlobal.TILESIZE, GameGlobal.GameHeight / 16, 
                                    GameGlobal.TILESIZE, GameGlobal.TILESIZE),
                       cm.Load<Texture2D>(@"ControlAssets/pauseBtn"), 
                       Graphics.GlobalGfx.btnHover, 
                       Graphics.GlobalGfx.btnActive,
                       "", true, true, true);
            //load fast forward button
            ffBtn = new Input.SFButton(new Rectangle((int)(GameGlobal.GameWidth / 1.3f) + 
                                    (2 * (pauseBtn.Bounds.Width + GameGlobal.TILESIZE)), GameGlobal.GameHeight / 16, 
                                    GameGlobal.TILESIZE, GameGlobal.TILESIZE),
                       cm.Load<Texture2D>(@"ControlAssets/pauseBtn"), 
                       Graphics.GlobalGfx.btnHover, 
                       Graphics.GlobalGfx.btnActive,
                       "", true, true, true);
            //add all the time buttons to a radio button set
            timeBtns = new Input.SFRadioButtons(true, 1);
            timeBtns.addButton(pauseBtn);
            timeBtns.addButton(playBtn);
            timeBtns.addButton(ffBtn);
        }

        public void updateGV(GameTime gt, ContentManager cm)
        {
            if (OfficeMenuClasses.Menu.subScreenOpen == false)
            {
                //update game time
                GameGlobal.player.TimeInGame.updateChronology(gt);


                int xCell = (int)(GameGlobal.InputControl.CurrentMouseState.X - startCoord.X) / 32;
                int yCell = (int)(GameGlobal.InputControl.CurrentMouseState.Y - startCoord.Y) / 32;
                if (GameGlobal.InputControl.IsNewPress(Input.MouseBtns.LeftClick))
                {
                    if (MouseWithinGrid())
                    {
                        #region Room Selection
                        if (GameGlobal.player.PlayerStore.BuildingArray[yCell, xCell] == false)
                        {
                            //figure out which room was clicked on.
                            sbyte examinedIndex = 0;
                            //check the coordinates of the placed rooms to determine which one is selected.
                            foreach (Rooms.Room r in GameGlobal.player.PlayerStore.RoomList)
                            {
                                if (xCell >= r.RoomLocation.X && xCell <= r.RoomLocation.X + (r.TileDimensions.X - 1) &&
                                    yCell >= r.RoomLocation.Y && yCell <= r.RoomLocation.Y + (r.TileDimensions.Y - 1))
                                {
                                    selectedRoom = examinedIndex;
                                    selectionType = 1;
                                    rip.SelectedRoomIndex = selectedRoom;
                                    rip.DisplayRoom = GameGlobal.player.PlayerStore.RoomList[selectedRoom];
                                    break;
                                }
                                examinedIndex++;
                            }
                        }
                        else
                        {
                            selectedRoom = -1;
                            selectionType = 0;
                            rip.SelectedRoomIndex = selectedRoom;
                            rip.DisplayRoom = null;
                        }
                        #endregion

                        #region Customer Selection
                        cIndex = activeCustomers.FindIndex(delegate(Customer c)
                                {
                                    return ((c.Position.X <= GameGlobal.InputControl.CurrentMouseState.X) &&
                                           (c.Position.X + c.SpriteSheet.Width >= GameGlobal.InputControl.CurrentMouseState.X) &&
                                           (c.Position.Y <= GameGlobal.InputControl.CurrentMouseState.Y) &&
                                           (c.Position.Y + c.SpriteSheet.Height >= GameGlobal.InputControl.CurrentMouseState.Y));
                                });
                        if (cIndex >= 0)
                        {
                            selectionType = 3;
                            cd = new CustomerDisplay(activeCustomers[cIndex]);
                        }
                        if (selectionType != 1 && cIndex < 0)
                        {
                            selectionType = 0;
                            cd = null;
                        }
                        #endregion
                    }
                }


                //remove customers that have exited
                List<int> exitList = new List<int>();
                int counter = 0;
                foreach (Customer c in activeCustomers)
                {
                    c.updateCustomer(gt);
                    if (c.Exited)
                    {
                        exitList.Add(counter);
                        if (cIndex == counter)
                        {
                            cIndex = -1;
                            selectionType = 0;
                        }
                    }
                    counter++;
                }

                for (int i = 0; i < exitList.Count; i++)
                {
                    activeCustomers[exitList[i]].unloadCustomer();
                    activeCustomers.RemoveAt(exitList[i]);
                }
                exitList.Clear();
                //-----


                //update GUI buttons
                timeBtns.updateRadioBtns();

                //handle button presses
                if (timeBtns.CurrentSelectedIndex == 0)
                { //change speed to pause
                    GameGlobal.player.TimeInGame.GameMoveSpeed = Chronology.GameSpeed.Pause;
                }
                else if (timeBtns.CurrentSelectedIndex == 1)
                { //change speed to normal
                    GameGlobal.player.TimeInGame.GameMoveSpeed = Chronology.GameSpeed.Normal;
                }
                else if (timeBtns.CurrentSelectedIndex == 2)
                { //change speed to fast
                    GameGlobal.player.TimeInGame.GameMoveSpeed = Chronology.GameSpeed.Fast;
                }
                else
                {
                    timeBtns.CurrentSelectedIndex = 0;
                }
            }

            //update room textures
            foreach (Rooms.Room dr in GameGlobal.player.PlayerStore.RoomList)
            {
                if (dr.GetType().BaseType == typeof(Rooms.DisplayRoom))
                {
                    //cast and update texture
                    ((Rooms.DisplayRoom)(dr)).UpdateTexture();                  
                }
            }

            //show what to display in the Info Panel
            switch (selectionType)
            {
                case 0:
                default:

                    break;
                case 1:
                    rip.updateInfoPanel(gt, cm);
                    break;
                case 2:

                    break;
                case 3:
                    cd.CustDisUpdate(gt);
                    break;
            }
            
        }

        public void drawGV(SpriteBatch sb, GameTime gt, GraphicsDevice gd)
        {
            if (OfficeMenuClasses.Menu.subScreenOpen == false)
            {
                //if the inventory is not open (for placing items), draw the gameplay view.
                if (!rip.InventoryOpen)
                {
                    //spawn customer code
                    if ((GameGlobal.player.TimeInGame.GameMoveSpeed == Chronology.GameSpeed.Normal && timerCount >= CST_NORMAL) ||
                        (GameGlobal.player.TimeInGame.GameMoveSpeed == Chronology.GameSpeed.Fast && timerCount >= CST_FAST))
                    {
                        timerCount = 0;
                        bool custSpawn = rand.Next(0, 100) < GameGlobal.player.Satisfaction ? true : false;
                        bool custType = rand.Next(0, 100) < 30 ? true : false;

                        if (custSpawn && activeCustomers.Count < MAX_CUSTOMERS)
                        {
                            activeCustomers.Add(CreateCustomer(gd, sb, custType));
                        }
                    }
                    else
                    {
                        //dont update the time if the game is not on pause.
                        if (GameGlobal.player.TimeInGame.GameMoveSpeed != Chronology.GameSpeed.Pause)
                        {
                            timerCount += (float)gt.ElapsedGameTime.TotalSeconds;
                        }
                    }

                    gd.Clear(Color.Black);

                    //end customer spawner
                    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    //draw the actual grid
                    #region Store Map Drawing
                    //reset layoutCoord
                    layoutCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
                    layoutCoord.Y = (GameGlobal.GameWidth / 3.5f) - (layoutHeight / 2);
                    //loop through layout array
                    for (int vert = 0; vert < GameGlobal.player.PlayerStore.BuildingArray.GetLength(0); vert++)
                    {
                        for (int horz = 0; horz < GameGlobal.player.PlayerStore.BuildingArray.GetLength(1); horz++)
                        {
                            //if the space is usable, draw it
                            if (GameGlobal.player.PlayerStore.BuildingArray[vert, horz] != null)
                            {
                                sb.Draw(Graphics.GlobalGfx.gridLargeTest, layoutCoord, Color.White);
                            }
                            //update position for next space
                            layoutCoord.X += GameGlobal.TILESIZE;
                        }
                        //update position for next row
                        layoutCoord.Y += GameGlobal.TILESIZE;
                        layoutCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
                    }
                    //loop through room list to draw rooms


                    //----
                    #endregion

                    //Draw the rooms in the layout
                    #region Draw Rooms
                    //loop through the list containing temp building data and draw them in the layout
                    foreach (Rooms.Room room in GameGlobal.player.PlayerStore.RoomList)
                    {
                        //get the origin of the sprite for rotation

                        Vector2 origin = new Vector2(room.RoomTexture.Width / 2,
                            room.RoomTexture.Height / 2);

                        //draw
                        if ((room.RotationInDegrees == 90 || room.RotationInDegrees == 270) && origin.X != origin.Y)
                        {
                            sb.Draw(room.RoomTexture,
                                    new Vector2(room.RoomLocation.X * 32 + startCoord.X + origin.Y, room.RoomLocation.Y * 32 + startCoord.Y + origin.X), null,
                                    Color.White, room.Rotation, origin, 1.0f, SpriteEffects.None, 0);
                        }
                        else
                        {
                            sb.Draw(room.RoomTexture,
                                    new Vector2(room.RoomLocation.X * 32 + startCoord.X + origin.X, room.RoomLocation.Y * 32 + startCoord.Y + origin.Y), null,
                                    Color.White, room.Rotation, origin, 1.0f, SpriteEffects.None, 0);
                        }
                    }
                    #endregion

                    #region  GUI Drawing
                    //draw the players money.  Gold if positive, red if negative.
                    if (GameGlobal.player.CurrentMoney >= 0)
                    {
                        sb.DrawString(GameGlobal.gameFont, "Gold: " + GameGlobal.player.CurrentMoney,
                            new Vector2(GameGlobal.GameWidth / 50f, GameGlobal.GameHeight / 50f), Color.Gold);
                    }
                    else
                    {
                        sb.DrawString(GameGlobal.gameFont, "Gold: " + GameGlobal.player.CurrentMoney,
                            new Vector2(GameGlobal.GameWidth / 50f, GameGlobal.GameHeight / 50f), Color.Red);
                    }

                    //Draw the Time (Year, Month, Day)
                    sb.DrawString(GameGlobal.gameFont, GameGlobal.player.TimeInGame.ToString(),
                        new Vector2(GameGlobal.GameWidth - 
                            GameGlobal.gameFont.MeasureString(GameGlobal.player.TimeInGame.ToString()).X - 10, GameGlobal.GameHeight / 50f), Color.White);                   
                    
                    //info panel divider
                    Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight * 0.7f),
                        new Vector2(GameGlobal.GameWidth, GameGlobal.GameHeight * 0.7f), 1, Color.DarkGray);

                    //draw customer highlight if applicable
                    if (cIndex >= 0)
                    {
                        if (!activeCustomers[cIndex].InShelf)
                        {
                            sb.Draw(chighlight, new Vector2(activeCustomers[cIndex].Position.X - (chighlight.Width - activeCustomers[cIndex].SpriteSheet.Width) / 2,
                                activeCustomers[cIndex].Position.Y - (chighlight.Height - activeCustomers[cIndex].SpriteSheet.Height) / 2), Color.White);
                        }
                    }

                    sb.End();

                    //draw GUI buttons
                    timeBtns.drawRadioBtns(sb);

                    #endregion

                    //draw all the customers in the store
                    foreach (Customer c in activeCustomers)
                    {
                        c.drawCustomer(sb);
                    }
                }
            }

            switch (selectionType)
            {
                case 0:
                default:

                    break;
                case 1:
                    rip.drawInfoPanel(sb, gt);
                    break;
                case 2:

                    break;
                case 3:
                    cd.CustDisDraw(sb, gt);
                    break;
            }
            
        }


        /// <summary>
        /// Get if the mouse is within the layout on the screen
        /// </summary>
        /// <returns>True is the mouse is in the layout, false if not.</returns>
        private bool MouseWithinGrid()
        {
            if (GameGlobal.InputControl.CurrentMouseState.X > startCoord.X &&
                GameGlobal.InputControl.CurrentMouseState.X < (startCoord.X + layoutWidth) &&
                GameGlobal.InputControl.CurrentMouseState.Y > startCoord.Y &&
                GameGlobal.InputControl.CurrentMouseState.Y < (startCoord.Y + layoutHeight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Customer CreateCustomer(GraphicsDevice gd, SpriteBatch sb, bool type)
        {
            Customer c = new Customer(rand.Next(50, 30000), Graphics.GlobalGfx.createCustomerTexture(gd, sb),
                new Vector2(startCoord.X + (GameGlobal.TILESIZE * GameGlobal.player.PlayerStore.DoorPosition.X),
                startCoord.Y + (GameGlobal.TILESIZE * GameGlobal.player.PlayerStore.DoorPosition.Y)), startCoord);

            c.initCustomer(type);

            c.Thoughts = "";

            return c;
        }
    }
}
