using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.EqRooms
{
    public class LargeArmorRack : ArmorRack
    {
        public static new int roomPrice = 5000;
        public static new int roomCounter = 0;
        public static new string roomName = "Lg Armor Rack";
        //constructors
        public LargeArmorRack() : base() 
        {
            MaximumCapacity = 20;
            MaximumWeight = 500;
        }

        public LargeArmorRack(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 20;
            MaximumWeight = 500;
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
    }
}
