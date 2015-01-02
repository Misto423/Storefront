using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.MultiRooms
{

    public class ExtraLargeShelfUnit : ShelfUnit
    {
        public static new int roomPrice = 3500;
        public static new int roomCounter = 0;
        public static new string roomName = "XL Shelf Unit";
        //constructors
        public ExtraLargeShelfUnit() : base()
        {
            MaximumCapacity = 35;
            MaximumWeight = 150;
        }

        public ExtraLargeShelfUnit(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 35;
            MaximumWeight = 150;
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
            return "XL Shelf Unit";
        }
    }
}
