using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Storefront.Rooms
{
    public abstract class Room
    {
        private string roomID; // custom name of the room;
        private string description; //description of the room, used for tooltips.
        private Texture2D roomTex; // the texture of the room.
        //0 - up
        //90 - right
        //180 - down
        //270 - left
        private float rotation, rotationDeg;
        private byte doorPos; //the side of the room a door is on (for room pieces), 0 if no door
        private Vector2 tileSize;  //dimensions of room/piece
        private Vector2 loc; //location in the array
        //gameplay vars
        private int upkeepCost; //the cost per week of upkeep
        private int maxDurability;
        private int currentDurability;

        //constructors
        public Room()
        {
            roomID = "";
            tileSize = new Vector2(0, 0);
            upkeepCost = 0;
            MaximumDurability = 100;
            CurrentDurability = 100;
        }

        public Room(string roomId, int upkeepCost)
        {
            this.roomID = roomId;
            tileSize = new Vector2(0, 0);
            this.upkeepCost = upkeepCost;
            MaximumDurability = 100;
            CurrentDurability = 100;
        }

        #region Attributes
        public string RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }
        public string RoomDescription
        {
            get { return description; }
            set { description = value; }
        }
        public Texture2D RoomTexture
        {
            get { return roomTex; }
            set { roomTex = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float RotationInDegrees
        {
            get { return rotationDeg; }
            set { rotationDeg = value; }
        }
        public byte DoorPosition
        {
            get { return doorPos; }
            set { doorPos = value; }
        }
        public Vector2 TileDimensions
        {
            get { return tileSize; }
            set { tileSize = value; }
        }
        public Vector2 RoomLocation
        {
            get { return loc; }
            set { loc = value; }
        }
        public int UpkeepCost
        {
            get { return upkeepCost; }
            set { upkeepCost = value; }
        }
        public int CurrentDurability
        {
            get { return currentDurability; }
            set { currentDurability = value; }
        }
        public int MaximumDurability
        {
            get { return maxDurability; }
            set { maxDurability = value; }
        }
        #endregion

        /// <summary>
        /// perform a function specific to the room or object
        /// </summary>
        public abstract void performFunction();

        /// <summary>
        /// Creates the room and loads all assets needed for it.
        /// </summary>
        public abstract void initRoom(ContentManager cm);

        /// <summary>
        /// Returns the type of the room.
        /// </summary>
        /// <returns>An integer corresponding to type of store.</returns>
        public abstract int RoomType();

        /// <summary>
        /// function to subtract the upkeep cost from your money total
        /// </summary>
        /// <param name="money">The player's current money.</param>
        /// <returns></returns>
        public int subtractCost(int money)
        {
            money -= upkeepCost;
            return money;
        }

        /// <summary>
        /// Rotates a piece, useful when placing pieces/rooms.
        /// </summary>
        //public void rotateRoom()
        //{
        //    if (Rotation == 0)
        //    {
        //        Rotation = 90;
        //        swapVals();
        //    }
        //    else if (Rotation == 90)
        //    {
        //        Rotation = 180;
        //        swapVals();
        //    }
        //    else if (Rotation == 180)
        //    {
        //        Rotation = 270;
        //        swapVals();
        //    }
        //    else if (Rotation == 270)
        //    {
        //        Rotation = 0;
        //        swapVals();
        //    }
        //    else //if, for some reason, rotation is not on one of 4 directions, draw as facing up
        //    {
        //        Rotation = 0;
        //    }
        //}

        //private void swapVals()
        //{
        //    float temp;
        //    temp = tileSize.Y;
        //    tileSize.Y = tileSize.X;
        //    tileSize.X = temp;
        //}

        ///// <summary>
        ///// Determines if the player is in the bounds of the room
        ///// </summary>
        ///// <returns>True if player is in room, false if not.</returns>
        //public bool isPlayerInBounds()
        //{
        //    if (GameLogic.GameGlobal.player.PlayerPosition.X >= this.RoomLocation.X &&
        //        GameLogic.GameGlobal.player.PlayerPosition.X < this.RoomLocation.X * GameLogic.GameGlobal.TILESIZE &&
        //        GameLogic.GameGlobal.player.PlayerPosition.Y >= this.RoomLocation.Y &&
        //        GameLogic.GameGlobal.player.PlayerPosition.Y < this.RoomLocation.Y * GameLogic.GameGlobal.TILESIZE)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
