using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.MultiRooms
{
    public class StoreRoom : StorageRoom
    {
        public static int roomPrice = 1000;
        public static int roomCounter = 0;
        public static string roomName = "Storeroom";
        //constructors
        public StoreRoom() : base() 
        {
            Type = StoreageType.General;
            GameLogic.GameGlobal.player.StorageData.MaximumStorageSpace += 20;
        }

        public StoreRoom(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            Type = StoreageType.General;
            GameLogic.GameGlobal.player.StorageData.MaximumStorageSpace += 20;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Storeroom";
        }
    }
}
