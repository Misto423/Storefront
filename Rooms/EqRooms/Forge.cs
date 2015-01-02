using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.EqRooms
{
    public class Forge : CreationRoom
    {
        public static int roomPrice = 8000;
        public static int roomCounter = 0;
        public static string roomName = "Forge";
        //constructors
        public Forge() : base() 
        {

        }

        public Forge(string name, int upkeepCost)
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
            throw new NotImplementedException();
        }
    }
}
