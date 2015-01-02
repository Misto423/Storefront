using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Storefront.Rooms
{
    public abstract class DisplayRoom : Room
    {
        private List<Database.ShelfItem> inv;
        private int maxWeight;
        private int maxCap;
        private int browseSpaces;
        private int curBs;

        public DisplayRoom()
            : base()
        {
            inv = new List<Database.ShelfItem>();
            browseSpaces = 0;
            curBs = 0;
        }

        public DisplayRoom(string roomId, int upkeep)
            : base(roomId, upkeep)
        {
            inv = new List<Database.ShelfItem>();
            browseSpaces = 0;
            curBs = 0;
        }

        #region Attributes
        public List<Database.ShelfItem> Inventory
        {
            get { return inv; }
            set { inv = value; }
        }

        public int CurrentCapacity
        {
            get { return inv.Count; }
        }

        public int MaximumCapacity
        {
            get { return maxCap; }
            set { maxCap = value; }
        }

        public float CurrentWeight
        {
            get
            {
                float w = 0;
                foreach (Database.ShelfItem si in inv)
                {
                    w += si.Weight;
                }
                return w;
            }
        }

        public int MaximumWeight
        {
            get { return maxWeight; }
            set { maxWeight = value; }
        }

        public int BrowseSpacesMaximum
        {
            get
            {
                Vector2 roomLocation = this.RoomLocation;
                Vector2 roomSize = this.TileDimensions;
                browseSpaces = 0;
                //check sides (not diagonals) of shelf
                //check left side
                for (int y = (int)roomLocation.Y; y < (int)roomLocation.Y + roomSize.Y; y++)
                {
                    //if the y index causes out of bounds, stop loop
                    if (y >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }
                    //if the x index is out of bounds, dont check left side.
                    if (roomLocation.X - 1 < 0) { break; }

                    if (GameLogic.GameGlobal.player.PlayerStore.BuildingArray[y, (int)roomLocation.X - 1] == true)
                    {
                        browseSpaces++;
                    }
                }
                //check right side
                for (int y = (int)roomLocation.Y; y < (int)roomLocation.Y + roomSize.Y; y++)
                {
                    //if the y index causes out of bounds, stop loop
                    if (y >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }
                    //if the x index is out of bounds, dont check right side.
                    if (roomLocation.X + 1 >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { break; }

                    if (GameLogic.GameGlobal.player.PlayerStore.BuildingArray[y, (int)roomLocation.X + (int)roomSize.X] == true)
                    {
                        browseSpaces++;
                    }
                }
                //check top side
                for (int x = (int)roomLocation.X; x < (int)roomLocation.X + roomSize.X; x++)
                {
                    //if the x index causes out of bounds, stop loop
                    if (x >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { break; }
                    //if the y index is out of bounds, dont check top side.
                    if (roomLocation.Y - 1 < 0) { break; }

                    if (GameLogic.GameGlobal.player.PlayerStore.BuildingArray[(int)roomLocation.Y - 1, x] == true)
                    {
                        browseSpaces++;
                    }
                }
                //check bottom side
                for (int x = (int)roomLocation.X; x < (int)roomLocation.X + roomSize.X; x++)
                {
                    //if the x index causes out of bounds, stop loop
                    if (x >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { continue; }
                    //if the y index is out of bounds, dont check top side.
                    if (roomLocation.Y + 1 >= GameLogic.GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }

                    if (GameLogic.GameGlobal.player.PlayerStore.BuildingArray[(int)roomLocation.Y + (int)roomSize.Y, x] == true)
                    {
                        browseSpaces++;
                    }
                }
                return browseSpaces;
            }
        }
        public int CurrentUsedBrowseSpaces
        {
            get { return curBs; }
            set { curBs = value; }
        }
        #endregion

        /// <summary>
        /// Gets the types of items that can be displayed on this shelf.
        /// </summary>
        /// <returns>A list of int values coresponding to the types of items.</returns>
        public abstract List<int> GetDisplayTypes();

        public abstract void UpdateTexture();

        public override int RoomType()
        {
            return 1;
        }
    }
}
