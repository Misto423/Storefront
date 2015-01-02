using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.GenRooms
{
    public class ButcherRoom : CreationRoom
    {
        //constructors
        public ButcherRoom() : base() 
        {

        }

        public ButcherRoom(string name, int upkeepCost)
            : base(name, upkeepCost)
        {

        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //allows user to make more food items, set trigger to reflect this
            throw new NotImplementedException();
        }
    }
}
