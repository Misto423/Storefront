using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storefront.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.GameLogic.OfficeMenuClasses
{
    public class ExpandView : SubMenuView
    {
        private SFBuildButton[] buildBtns;
        private SFButton acceptBtn;
        private SFButton backBtn;
        private int curRotationDeg = 0;
        private float curRotationRad = 0;
        private int layoutWidth, layoutHeight;
        private Vector2 layoutCoord = new Vector2();
        private Vector2 startCoord = new Vector2();
        private Vector2 origin;
        private int xPos = 0;
        private int selectedIndex = -1;
        private List<RoomPlacements> tempRoomList;
        private SFButton leftBtn, rightBtn;
        private int startIndexScroll = 0;
        private bool newRoom = false;
        private int selectedRoomIndex = -1;

        public override void InitView(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            //clear out any previously placed buildings
            tempRoomList = new List<RoomPlacements>();
            //create the buttons
            buildBtns = new SFBuildButton[13];
            xPos = 0;
            #region  Create Build Buttons
            buildBtns[0] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.ExtendedCounter.roomPrice.ToString(),
                Rooms.MultiRooms.ExtendedCounter.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[1] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.ShelfUnit.roomPrice.ToString(),
                Rooms.MultiRooms.ShelfUnit.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/ShelfUnitTest"));
            buildBtns[2] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.LargeShelfUnit.roomPrice.ToString(),
                Rooms.MultiRooms.LargeShelfUnit.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[3] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.ExtraLargeShelfUnit.roomPrice.ToString(),
                Rooms.MultiRooms.ExtraLargeShelfUnit.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[4] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.StoreRoom.roomPrice.ToString(),
                Rooms.MultiRooms.StoreRoom.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[5] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.MultiRooms.WindowDisplay.roomPrice.ToString(),
                Rooms.MultiRooms.WindowDisplay.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[6] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.ArmorStand.roomPrice.ToString(),
                Rooms.EqRooms.ArmorStand.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[7] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.ArmorRack.roomPrice.ToString(),
                Rooms.EqRooms.ArmorRack.roomName, cm.Load<Texture2D>(@"RoomTextures/EquipmentStore/RackEmpty"));
            buildBtns[8] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.LargeArmorRack.roomPrice.ToString(),
                Rooms.EqRooms.LargeArmorRack.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[9] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.WeaponRack.roomPrice.ToString(),
                Rooms.EqRooms.WeaponRack.roomName, cm.Load<Texture2D>(@"RoomTextures/EquipmentStore/RackEmpty"));
            buildBtns[10] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.LargeWeaponRack.roomPrice.ToString(),
                Rooms.EqRooms.LargeWeaponRack.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[11] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.Forge.roomPrice.ToString(),
                Rooms.EqRooms.Forge.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            buildBtns[12] = new SFBuildButton(new Vector2(xPos, GameGlobal.GameHeight / 1.45f), true,
                Rooms.EqRooms.PracticeArea.roomPrice.ToString(),
                Rooms.EqRooms.PracticeArea.roomName, cm.Load<Texture2D>(@"RoomTextures/TestPieces/CounterTest"));
            #endregion
            //create accept button
            acceptBtn = new SFButton(new Rectangle(GameGlobal.GameWidth - (GameGlobal.GameWidth / 10), 0,
                                        GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 10),
                                     Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                                     Graphics.GlobalGfx.btnActive, "Accept", false, true, true);
            //create back button
            backBtn = new SFButton(new Rectangle(GameGlobal.GameWidth - (GameGlobal.GameWidth / 10), GameGlobal.GameHeight / 10, GameGlobal.GameWidth / 10, GameGlobal.GameHeight / 10),
                         Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover,
                         Graphics.GlobalGfx.btnActive, "Back", false, true, true);

            //create left and right scroll buttons
            leftBtn = new SFButton(new Rectangle(0, (int)(GameGlobal.GameHeight / 1.45f - GameGlobal.GameWidth / 20), GameGlobal.GameWidth / 20, GameGlobal.GameWidth / 20),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, "<", false, true, true);
            rightBtn = new SFButton(new Rectangle(GameGlobal.GameWidth - GameGlobal.GameWidth / 20, (int)(GameGlobal.GameHeight / 1.45f - GameGlobal.GameWidth / 20),
                GameGlobal.GameWidth / 20, GameGlobal.GameWidth / 20),
                Graphics.GlobalGfx.btnDef, Graphics.GlobalGfx.btnHover, Graphics.GlobalGfx.btnActive, ">", false, true, true);

            //Get layout dimensions
            layoutWidth = GameGlobal.player.PlayerStore.BuildingArray.GetLength(1) * GameGlobal.TILESIZE;
            layoutHeight = GameGlobal.player.PlayerStore.BuildingArray.GetLength(0) * GameGlobal.TILESIZE;
            startCoord.X = (GameGlobal.GameWidth / 2) - (layoutWidth / 2);
            startCoord.Y = (GameGlobal.GameWidth / 3.5f) - (layoutHeight / 2);

        }

        public override void LoadView()
        {

        }

        public override void UnloadView()
        {
            foreach (SFBuildButton sbb in buildBtns)
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

        public override void UpdateView(GameTime gt, out bool? stateSwitch)
        {
            //enable/disable those that can be purchased
            foreach (SFBuildButton bb in buildBtns)
            {
                if (GameGlobal.player.CurrentMoney >= Int32.Parse(bb.ButtonText))
                {
                    bb.IsEnabled = true;
                }
                else
                {
                    bb.IsEnabled = false;
                }
            }

            if (selectedIndex > -1 && selectedRoomIndex > -1)
            {
                selectedRoomIndex = -1;
                selectedIndex = -1;
            }

            if (GameGlobal.InputControl.IsNewPress(MouseBtns.LeftClick))
            { //if a building is selected.
                if (selectedIndex > -1 || selectedRoomIndex > -1)
                {
                    //if the mouse is inside the layout grid
                    if (MouseWithinGrid())
                    { //Try to place the building in the array
                        if (!TryPlaceRoom(new Vector2(GameGlobal.InputControl.CurrentMouseState.X, GameGlobal.InputControl.CurrentMouseState.Y)))
                        {
                            //show an error that building can't be placed there.
                            //update money
                            if (selectedIndex > -1)
                            {
                                GameGlobal.player.CurrentMoney += Int32.Parse(buildBtns[selectedIndex].ButtonText);
                            }
                            selectedIndex = -1;
                            selectedRoomIndex = -1;
                        }
                        else
                        {
                            //reset index to prevent placing multiple rooms.
                            selectedIndex = -1;
                            selectedRoomIndex = -1;
                        }
                    }
                    else
                    { //reset the selected index if the mouse is clicked out of the bounds of layout
                        if (selectedIndex > -1)
                        {
                            GameGlobal.player.CurrentMoney += Int32.Parse(buildBtns[selectedIndex].ButtonText);
                        }
                        selectedIndex = -1;
                        selectedRoomIndex = -1;
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
                            //if you click within the bounds of a room, allow it to be moved
                            if (xCell >= rp.position.X && xCell < (rp.position.X + rp.size.X) &&
                                yCell >= rp.position.Y && yCell < (rp.position.Y + rp.size.Y))
                            {
                                //get the index of that room to replace it.
                                selectedIndex = rp.index;
                                selectedRoomIndex = -1;
                                newRoom = false;
                                break;
                            }
                        }

                        for (int r = 0; r < GameGlobal.player.PlayerStore.RoomList.Count; r++)
                        {
                            //if you click within the bounds of a room, allow it to be moved
                            if (xCell >= GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.X && 
                                xCell < (GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.X + GameGlobal.player.PlayerStore.RoomList[r].TileDimensions.X) &&
                                yCell >= GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.Y && 
                                yCell < (GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.Y + GameGlobal.player.PlayerStore.RoomList[r].TileDimensions.Y))
                            {
                                //get the index of that room to replace it.
                                selectedIndex = -1;
                                selectedRoomIndex = r;
                                newRoom = false;
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

                if (selectedRoomIndex > -1)
                {
                    RotateRoom();
                }
            }


            for (int index = 0; index < buildBtns.Length; index++)
            {
                buildBtns[index].updateBtn();
                //if the button is enabled (not placed)
                if (buildBtns[index].IsEnabled)
                {
                    if (buildBtns[index].isDown()) //button is clicked
                    {
                        //set the selected index to hold the item after the update frame.
                        selectedIndex = index;
                        newRoom = true;
                        //update money
                        GameGlobal.player.CurrentMoney -= Int32.Parse(buildBtns[selectedIndex].ButtonText);
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
                //create the actual rooms
                createRoomObjects();
                stateSwitch = true;
            }

            backBtn.updateButton();
            if (backBtn.isDown())
            {
                stateSwitch = true;
            }

            leftBtn.updateButton();
            if (leftBtn.isDown())
            {
                if (startIndexScroll > 0)
                {
                    startIndexScroll--;
                }
            }

            rightBtn.updateButton();
            if (rightBtn.isDown())
            {
                if (startIndexScroll < buildBtns.Length - 5)
                {
                    startIndexScroll++;
                }
            }
        }

        public override void DrawView(GameTime gt, SpriteBatch sb)
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
                origin = new Vector2(buildBtns[selectedIndex].BtnTexture.Width / 2,
                    buildBtns[selectedIndex].BtnTexture.Height / 2);


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
                sb.Draw(buildBtns[selectedIndex].BtnTexture, new Vector2(snapX, snapY),
                    null, Color.White * 0.5f, curRotationRad, origin, 1.0f, SpriteEffects.None, 0);
            }

            if (selectedRoomIndex > -1)
            {
                int snapX, snapY;
                //get the origin of the sprite for rotation

                //use normal texture dimensions 
                origin = new Vector2(GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Width / 2,
                    GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Height / 2);

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
                sb.Draw(GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture, new Vector2(snapX, snapY),
                    null, Color.White * 0.5f, curRotationRad, origin, 1.0f, SpriteEffects.None, 0);
            }
            #endregion

            //Draw the rooms in the layout
            #region Draw Buildings
            //loop through existing rooms and display them on the layout
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
            //loop through the list containing temp building data and draw them in the layout
            foreach (RoomPlacements rp in tempRoomList)
            {
                //get the origin of the sprite for rotation

                origin = new Vector2(buildBtns[rp.index].BtnTexture.Width / 2,
                    buildBtns[rp.index].BtnTexture.Height / 2);

                //draw
                if ((rp.rotDeg == 90 || rp.rotDeg == 270) && origin.X != origin.Y)
                {
                    sb.Draw(buildBtns[rp.index].BtnTexture,
                            new Vector2(rp.position.X * 32 + startCoord.X + origin.Y, rp.position.Y * 32 + startCoord.Y + origin.X), null,
                            Color.White, rp.rotRad, origin, 1.0f, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(buildBtns[rp.index].BtnTexture,
                            new Vector2(rp.position.X * 32 + startCoord.X + origin.X, rp.position.Y * 32 + startCoord.Y + origin.Y), null,
                            Color.White, rp.rotRad, origin, 1.0f, SpriteEffects.None, 0);
                }
            }
            #endregion

            //Draw Menu Line
            Graphics.SimpleDraw.drawLine(sb, new Vector2(0, GameGlobal.GameHeight / 1.45f),
                new Vector2(GameGlobal.GameWidth, GameGlobal.GameHeight / 1.45f), 1, Color.Silver);


            //draw money
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

            sb.End(); //end drawing for this class.

            //draw the building buttons
            for (int index = startIndexScroll; index < startIndexScroll + 5; index++)
            {
                buildBtns[index].Position = new Vector2(xPos + ((index - startIndexScroll) * (int)(GameLogic.GameGlobal.GameWidth / 5f)), buildBtns[index].Position.Y);
                buildBtns[index].drawBtn(sb);
            }

            //draw buttons
            acceptBtn.drawButton(sb, true);
            backBtn.drawButton(sb, true);
            leftBtn.drawButton(sb, true);
            rightBtn.drawButton(sb, true);
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
            int xPos = 0, yPos = 0;
            int cellWidth = 0, cellHeight = 0;

            #region New Rooms Placement
            if (selectedIndex > -1)
            {
                //get origin again
                origin = new Vector2(buildBtns[selectedIndex].BtnTexture.Width / 2,
                                    buildBtns[selectedIndex].BtnTexture.Height / 2);
                //get the cells of the layout
                xPos = (int)(position.X - startCoord.X) / 32;
                if ((curRotationDeg == 90 || curRotationDeg == 270) && origin.X != origin.Y)
                {
                    //since mouse will be on the bottom half, need to subtract the width of the room minus 1
                    yPos = (int)(position.Y - startCoord.Y) / 32 - (buildBtns[selectedIndex].BtnTexture.Width / 32 - 1);
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
                    cellWidth = buildBtns[selectedIndex].BtnTexture.Height / 32;
                    cellHeight = buildBtns[selectedIndex].BtnTexture.Width / 32;
                }
                else
                {
                    cellWidth = buildBtns[selectedIndex].BtnTexture.Width / 32;
                    cellHeight = buildBtns[selectedIndex].BtnTexture.Height / 32;
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
            }
            #endregion

            #region Moving other Rooms
            if (selectedRoomIndex > -1)
            {
                //get origin again
                origin = new Vector2(GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Width / 2,
                                    GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Height / 2);
                //get the cells of the layout
                xPos = (int)(position.X - startCoord.X) / 32;
                if ((curRotationDeg == 90 || curRotationDeg == 270) && origin.X != origin.Y)
                {
                    //since mouse will be on the bottom half, need to subtract the width of the room minus 1
                    yPos = (int)(position.Y - startCoord.Y) / 32 - (GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Width / 32 - 1);
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
                    cellWidth = GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Height / 32;
                    cellHeight = GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Width / 32;
                }
                else
                {
                    cellWidth = GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Width / 32;
                    cellHeight = GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomTexture.Height / 32;
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
            }
            #endregion

            //check to see if room is already in the layout
            //if it is, remove the old one when new one is placed.
            if (!newRoom)
            {
                if (selectedIndex > -1)
                {
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
                }

                if (selectedRoomIndex > -1)
                {
                    for (int r = 0; r < GameGlobal.player.PlayerStore.RoomList.Count; r++)
                    {
                        if (r == selectedRoomIndex)
                        {
                            //reset array area to true since building is moved
                            for (int xIndex = (int)GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.X; 
                                xIndex < (GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.X + GameGlobal.player.PlayerStore.RoomList[r].TileDimensions.X); 
                                xIndex++)
                            {
                                for (int yIndex = (int)GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.Y;
                                    yIndex < (GameGlobal.player.PlayerStore.RoomList[r].RoomLocation.Y + GameGlobal.player.PlayerStore.RoomList[r].TileDimensions.Y);
                                    yIndex++)
                                {
                                    GameGlobal.player.PlayerStore.BuildingArray[yIndex, xIndex] = true;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            //place building
            //selected index used to determine the building
            if (selectedIndex > -1)
            {
                RoomPlacements temp = new RoomPlacements();
                temp.position = new Vector2(xPos, yPos);
                temp.size = new Vector2(cellWidth, cellHeight);
                temp.index = selectedIndex;
                temp.rotRad = curRotationRad;
                temp.rotDeg = curRotationDeg;
                tempRoomList.Add(temp);
            }
            if (selectedRoomIndex > -1)
            {
                GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RoomLocation = new Vector2(xPos, yPos);
                GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].TileDimensions = new Vector2(cellWidth, cellHeight);
                GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].Rotation = curRotationRad;
                GameGlobal.player.PlayerStore.RoomList[selectedRoomIndex].RotationInDegrees = curRotationDeg;
            }

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
                            Rooms.MultiRooms.ShelfUnit.roomCounter++;
                            temp = new Rooms.MultiRooms.ShelfUnit("Shelf " + Rooms.MultiRooms.ShelfUnit.roomCounter, 2000);
                        }
                        else if (rp.index == 2)
                        {
                            temp = new Rooms.MultiRooms.ShelfUnit("Shelf 1", 2000);
                        }
                        else if (rp.index == 3)
                        {
                            temp = new Rooms.EqRooms.WeaponRack("Weapon Rack 1", 2000);
                        }
                        else if (rp.index == 4)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else if (rp.index == 5)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else if (rp.index == 6)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else if (rp.index == 7)
                        {
                            Rooms.EqRooms.ArmorRack.roomCounter++;
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack " + Rooms.EqRooms.ArmorRack.roomCounter, 2000);
                        }
                        else if (rp.index == 8)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else if (rp.index == 9)
                        {
                            Rooms.EqRooms.WeaponRack.roomCounter++;
                            temp = new Rooms.EqRooms.WeaponRack("Weapon Rack " + Rooms.EqRooms.WeaponRack.roomCounter, 2000);
                        }
                        else if (rp.index == 10)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else if (rp.index == 11)
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        else
                        {
                            temp = new Rooms.EqRooms.ArmorRack("Armor Rack 1", 2000);
                        }
                        addRoomToStore(temp, rp);
                        //subtract money from player
                        //GameGlobal.player.CurrentMoney -= rp.price;
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
            temp.RoomTexture = buildBtns[rp.index].BtnTexture;
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
