using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.GenRooms
{
    public class LargeClothingRack : ClothingRack
    {
        //constructors
        public LargeClothingRack() : base() 
        {
            MaximumCapacity = 25;
            MaximumWeight = 75;
        }

        public LargeClothingRack(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 25;
            MaximumWeight = 75;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //open the menu for clothing rack
            base.performFunction();
            throw new NotImplementedException();
        }
    }
}
