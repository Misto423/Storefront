using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storefront.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Storefront.GameLogic.NewGameStuff
{
    public class InitialPiecePlacement
    {
        private SFBuildButton[] startingBtns;
        private SFButton acceptBtn;
        private SFButton backBtn;
        private SFDialogBox startGameCheck;
        private bool triggerDialog = false;
        private int curRotationDeg = 0;
        private float curRotationRad = 0;
        private int layoutWidth, layoutHeight;
        private Vector2 layoutCoord = new Vector2();
        private Vector2 startCoord = new Vector2();
        private Vector2 origin;
        private int xPos = 0;
        private int selectedIndex = -1;
        private List<RoomPlacements> tempRoomList;

        public void initIPP(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            //clear out any previously placed buildings
            tempRoomList = new List<RoomPlacements>();
            //create the buttons
            startingBtns = new SFBuildButton[5];
            xPos = 0;
            for (int index = 0; index < startingBtns.Length; index++)
            {
                startingBtns[index] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                    "Free!", Stores.RoomDataFiller.eqRoomNames[index], Stores.RoomDataFiller.eqRooms[index]);
                xPos += (int)(GameLogic.GameGlobal.GameWidth / 5f);
            }
            //create accept button
            acceptBtn = new SFButton(new Rectangle(GameGlobal.GameWidth - (GameGlobal.GameWidth / 10), 0, 
                                        GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 10), 
                                     Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, 
                                     Graphics.GlobalGfx.btnActive, "Accept", false, true, true);
            //create back button
            backBtn = new SFButton(new Rectangle(0, 0, GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 10),
                         Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                         Graphics.GlobalGfx.btnActive, "Back", false, true, true);
            //create the dialog box
            startGameCheck = new SFDialogBox(Rectangle.Empty, Graphics.GlobalGfx.dialogTexture, 
                "Are you sure you are ready to start the game, initial character setup cannot be redone past this point!", DefaultChoices.Yes_No);

            //Get layout dimensions
            layoutWidth = GameGlobal.player.PlayerStore.BuildingArray.GetLength(1) * GameGlobal.TILESIZE;
            layoutHeight = GameGlobal.player.PlayerStore.BuildingArray.GetLength(0) * GameGlobal.TILESIZE;
            startCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
            startCoord.Y = (GameGlobal.GameWidth / 3.5f) - (layoutHeight / 2);
        }

        public void unloadIPP()
        {
            foreach (SFBuildButton sbb in startingBtns)
            {
                sbb.unloadBuildBtn();
            }

            acceptBtn.unloadBtn();
            backBtn.unloadBtn();
            layoutCoord = Vector2.Zero;
            startCoord = Vector2.Zero;
            origin = Vector2.Zero;
            tempRoomList.RemoveRange(0, tempRoomList.Count);
        }

        public void updateIPP(out bool? stateSwitch)
        {
            if (GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
            { //if a building is selected.
                if (selectedIndex > -1)
                {
                    //if the mouse is inside the layout grid
                    if (MouseWithinGrid())
                    { //Try to place the building in the array
                        if (!TryPlaceRoom(new Vector2(GameGlobal.InputControl.CurrentMouseState.X, GameGlobal.InputControl.CurrentMouseState.Y)))
                        {
                            //show an error that building can't be placed there.
                        }
                        else
                        {
                            //disable the button since room is already placed.
                            startingBtns[selectedIndex].IsEnabled = false;
                            //reset index to prevent placing multiple rooms.
                            selectedIndex = -1;
                        }
                    }
                    else
                    { //reset the selected index if the mouse is clicked out of the bounds of layout
                        selectedIndex = -1;
                    }
                }
                else
                { //allow picking up building
                    if (MouseWithinGrid())
                    {
                        int xCell = (int)(GameGlobal.InputControl.CurrentMouseState.X - startCoord.X) / 32;
                        int yCell = (int)(GameGlobal.InputControl.CurrentMouseState.Y - startCoord.Y) / 32;
                        //loop through the room placement list to see if the click occured within the bounds of a room
                        //then allow player to move it and reorganize list.
                        foreach (RoomPlacements rp in tempRoomList)
                        {
                            //if you click within the bounds of a room, allow if to be moved
                            if (xCell >= rp.position.X && xCell < (rp.position.X + rp.size.X) &&
                                yCell >= rp.position.Y && yCell < (rp.position.Y + rp.size.Y))
                            {
                                //get the index of that room to replace it.
                                selectedIndex = rp.index;
                                break;
                            }
                        }
                    }
                }
            }

            //rotation code
            if (GameGlobal.InputControl.IsNewPress(MouseBtns.RightClick))
            {
                //if a building is selected, rotate it on a right click
                if (selectedIndex > -1)
                {
                    RotateRoom();
                }
            }


            for (int index = 0; index < startingBtns.Length; index++)
            {
                startingBtns[index].updateBtn();
                //if the button is enabled (not placed)
                if (startingBtns[index].IsEnabled)
                {
                    if (startingBtns[index].isDown()) //button is clicked
                    {
                        //set the selected index to hold the item after the update frame.
                        selectedIndex = index;
                        //reset rotation
                        curRotationDeg = 0;
                    }
                }
            }
            stateSwitch = false;
            //update the accept button and trigger the dialog if clicked.
            acceptBtn.updateButton();
            if (acceptBtn.isDown())
            {
                triggerDialog = true;
            }

            backBtn.updateButton();
            if (backBtn.isDown())
            {
                stateSwitch = null;
            }

            //show dialog box to start game
            if (triggerDialog)
            {
                startGameCheck.updateDialogBox();
                int c = startGameCheck.Selection;
                if (c == 1)
                {
                    triggerDialog = false;
                }
                else if (c == 0)
                {
                    //create the actual rooms
                    createRoomObjects();
                    stateSwitch = true;
                }
            }
        }

        public void drawIPP(SpriteBatch sb, GameTime gt)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            //Draw the layout Preview
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

            //draw the selected room over the mouse, snapping to the grid when over the map
            #region Placing Rooms
            if (selectedIndex > -1)
            {
                int snapX, snapY;
                //get the origin of the sprite for rotation

                //use normal texture dimensions 
                origin = new Vector2(Stores.RoomDataFiller.eqRooms[selectedIndex].Width / 2,
                    Stores.RoomDataFiller.eqRooms[selectedIndex].Height / 2);


                if (MouseWithinGrid())
                {
                    //snap to the nearest grid space.
                    if ((curRotationDeg == 90 || curRotationDeg == 270) && origin.X != origin.Y)
                    { //adjust the snap for non-square pieces.
                        snapX = ((GameGlobal.InputControl.CurrentMouseState.X + 16) & ~31) - ((int)startCoord.X % 32) + (int)origin.Y;
                        snapY = ((GameGlobal.InputControl.CurrentMouseState.Y + 16) & ~31) + ((int)startCoord.Y % 32) - (int)origin.X;
                    }
                    else
                    {
                        snapX = ((GameGlobal.InputControl.CurrentMouseState.X + 16) & ~31) - ((int)startCoord.X % 32) + (int)origin.X;
                        snapY = ((GameGlobal.InputControl.CurrentMouseState.Y + 16) & ~31) + ((int)startCoord.Y % 32) - (int)origin.Y;
                    }
                }
                else
                {
                    //follow the mouse
                    snapX = GameGlobal.InputControl.CurrentMouseState.X;
                    snapY = GameGlobal.InputControl.CurrentMouseState.Y;
                }
                //follows the mouse and draws at 50% transparency
                sb.Draw(Stores.RoomDataFiller.eqRooms[selectedIndex], new Vector2(snapX, snapY),
                    null, Color.White * 0.5f, curRotationRad, origin, 1.0f, SpriteEffects.None, 0);
            }
            #endregion

            //Draw the rooms in the layout
            #region Draw Buildings
            //loop through the list containing temp building data and draw them in the layout
            foreach (RoomPlacements rp in tempRoomList)
            {
                //get the origin of the sprite for rotation

                origin = new Vector2(Stores.RoomDataFiller.eqRooms[rp.index].Width / 2,
                    Stores.RoomDataFiller.eqRooms[rp.index].Height / 2);

                //draw
                if ((rp.rotDeg == 90 || rp.rotDeg == 270) && origin.X != origin.Y)
                {
                    sb.Draw(Stores.RoomDataFiller.eqRooms[rp.index],
                            new Vector2(rp.position.X * 32 + startCoord.X + origin.Y, rp.position.Y * 32 + startCoord.Y + origin.X), null,
                            Color.White, rp.rotRad, origin, 1.0f, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(Stores.RoomDataFiller.eqRooms[rp.index],
                            new Vector2(rp.position.X * 32 + startCoord.X + origin.X, rp.position.Y * 32 + startCoord.Y + origin.Y), null,
                            Color.White, rp.rotRad, origin, 1.0f, SpriteEffects.None, 0);
                }
            }
            #endregion

            //Draw Menu Line
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 1.45f),
                new Vector2(GameGlobal.GameWidth, GameGlobal.GameHeight / 1.45f), 1, Color.Silver);

            sb.End(); //end drawing for this class.

            //draw the building buttons
            for (int index = 0; index < startingBtns.Length; index++)
            {
                startingBtns[index].drawBtn(sb);
            }
            //draw buttons
            acceptBtn.drawButton(sb, true);
            backBtn.drawButton(sb, true);
            //if the accept button has been clicked, draw dialog
            if (triggerDialog)
            {
                startGameCheck.drawDialogBox(sb, gt);
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

        /// <summary>
        /// Checks to see if the room can be placed in the grid and doesn't collide with other rooms
        /// </summary>
        /// <param name="position">The mouse position when a click occurs in the grid.</param>
        /// <returns>True if the object can be placed, false if it cannot.</returns>
        private bool TryPlaceRoom(Vector2 position)
        {
            int xPos, yPos;
            int cellWidth, cellHeight;
            //get origin again
            origin = new Vector2(Stores.RoomDataFiller.eqRooms[selectedIndex].Width / 2,
                                Stores.RoomDataFiller.eqRooms[selectedIndex].Height / 2);
            //get the cells of the layout
            xPos = (int)(position.X - startCoord.X) / 32;
            if ((curRotationDeg == 90 || curRotationDeg == 270) && origin.X != origin.Y)
            {
                //since mouse will be on the bottom half, need to subtract the width of the room minus 1
                yPos = (int)(position.Y - startCoord.Y) / 32 - (Stores.RoomDataFiller.eqRooms[selectedIndex].Width / 32 - 1);
                if (yPos < 0)
                {
                    //return if the piece is attempting to be placed out of bounds
                    return false;
                }
            }
            else
            {
                yPos = (int)(position.Y - startCoord.Y) / 32;
            }
            //get the cell width and height
            if ((curRotationDeg == 90 || curRotationDeg == 270) && origin.X != origin.Y)
            {
                //flip if rotation is 90 or 270
                cellWidth = Stores.RoomDataFiller.eqRooms[selectedIndex].Height / 32;
                cellHeight = Stores.RoomDataFiller.eqRooms[selectedIndex].Width / 32;
            }
            else
            {
                cellWidth = Stores.RoomDataFiller.eqRooms[selectedIndex].Width / 32;
                cellHeight = Stores.RoomDataFiller.eqRooms[selectedIndex].Height / 32;
            }

            //check that all the cells that the room will occupy are empty
            for (int xIndex = xPos; xIndex < (xPos + cellWidth); xIndex++)
            {
                for (int yIndex = yPos; yIndex < (yPos + cellHeight); yIndex++)
                {
                    if (xIndex >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(1))
                    {
                        return false;
                    }
                    if (GameGlobal.player.PlayerStore.BuildingArray[yIndex, xIndex] == false ||
                        GameGlobal.player.PlayerStore.BuildingArray[yIndex, xIndex] == null)
                    { // a space that cannot be built on or that already contains a room is found
                        // return false that the new item cannot be placed there.
                        return false;
                    }
                }
            }

            //if the code reaches this point, the object can be placed
            //set the elements of the spaces to false to signify the object is in those cells
            for (int xIndex = xPos; xIndex < (xPos + cellWidth); xIndex++)
            {
                for (int yIndex = yPos; yIndex < (yPos + cellHeight); yIndex++)
                {
                    GameGlobal.player.PlayerStore.BuildingArray[yIndex, xIndex] = false;
                }
            }

            //check to see if room is already in the layout
            //if it is, remove the old one when new one is placed.
            foreach (RoomPlacements rp in tempRoomList)
            {
                if (rp.index == selectedIndex)
                {
                    //reset array area to true since building is moved
                    for (int xIndex = (int)rp.position.X; xIndex < (rp.position.X + rp.size.X); xIndex++)
                    {
                        for (int yIndex = (int)rp.position.Y; yIndex < (rp.position.Y + rp.size.Y); yIndex++)
                        {
                            GameGlobal.player.PlayerStore.BuildingArray[yIndex, xIndex] = true;
                        }
                    }
                    tempRoomList.Remove(rp);
                    break;
                }
            }
            //place building
            //selected index used to determine the building
            RoomPlacements temp = new RoomPlacements();
            temp.position = new Vector2(xPos, yPos);
            temp.size = new Vector2(cellWidth, cellHeight);
            temp.index = selectedIndex;
            temp.rotRad = curRotationRad;
            temp.rotDeg = curRotationDeg;
            tempRoomList.Add(temp);

            //reset the rotation
            curRotationDeg = 0;
            curRotationRad = 0;

            return true;
        }

        /// <summary>
        /// Called when accept button is clicked to make the room objects from the placements.
        /// </summary>
        private void createRoomObjects()
        {
            Rooms.Room temp;
            //create the rooms based on which store is selected.
            switch (GameGlobal.player.PlayerStore.ToString())
            {
                case "Equipment Store":
                    foreach (RoomPlacements rp in tempRoomList)
                    {
                        if (rp.index == 0)
                        {
                            temp = new Rooms.MultiRooms.OfficeRm("Store Office", 2000);
                        }
                        else if (rp.index == 1)
                        {
                            temp = new Rooms.MultiRooms.CounterRm("Main Counter", 2000);
                        }
                        else if (rp.index == 2)
                        {
                            Rooms.MultiRooms.ShelfUnit.roomCounter++;
                            temp = new Rooms.MultiRooms.ShelfUnit("Shelf " + Rooms.MultiRooms.ShelfUnit.roomCounter, 2000);
                        }
                        else if (rp.index == 3)
                        {
                            Rooms.EqRooms.WeaponRack.roomCounter++;
                            temp = new Rooms.EqRooms.WeaponRack("Weapon Rack " + Rooms.EqRooms.WeaponRack.roomCounter, 2000);
                        }
                        else 
                        {
                            Rooms.EqRooms.ArmorRack.roomCounter++;
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack " + Rooms.EqRooms.ArmorRack.roomCounter, 2000);
                        }
                        addRoomToStore(temp, rp);
                    }

                    break;
            }
        }

        private void addRoomToStore(Rooms.Room temp, RoomPlacements rp)
        {
            temp.RoomLocation = rp.position;
            temp.Rotation = rp.rotRad;
            temp.RotationInDegrees = rp.rotDeg;
            temp.TileDimensions = rp.size;
            temp.RoomTexture = Stores.RoomDataFiller.eqRooms[rp.index];
            GameGlobal.player.PlayerStore.RoomList.Add(temp);
        }

        /// <summary>
        /// Handles rotation of the texture that is selected.
        /// </summary>
        private void RotateRoom()
        {
            switch (curRotationDeg)
            {
                case 0:
                    curRotationRad = (float)Math.PI / 2f;
                    curRotationDeg = 90;
                    break;
                case 90:
                    curRotationRad = (float)Math.PI;
                    curRotationDeg = 180;
                    break;
                case 180:
                    curRotationRad = (float)(3 * Math.PI) / 2f;
                    curRotationDeg = 270;
                    break;
                case 270:
                default:
                    curRotationRad = 0;
                    curRotationDeg = 0;
                    break;
            }
        }
    }
}
