using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.MultiRooms
{
    public class LargeShelfUnit : ShelfUnit
    {
        public static new int roomPrice = 2000;
        public static new int roomCounter = 0;
        public static new string roomName = "Lg Shelf Unit";
        //constructors
        public LargeShelfUnit() : base() 
        {
            MaximumCapacity = 20;
            MaximumWeight = 100;
        }

        public LargeShelfUnit(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 20;
            MaximumWeight = 100;
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
            return "Lg Shelf Unit";
        }
    }
}
