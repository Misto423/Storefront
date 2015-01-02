using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.BarRooms
{
    public class PrivateRoom : CustomerRoom
    {
        //constructors
        public PrivateRoom() : base() 
        {

        }

        public PrivateRoom(string name, int upkeepCost)
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
            //chance to steal info/items
            throw new NotImplementedException();
        }
    }
}
