using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.BarRooms
{
    public class BarRm : IdleRoom
    {
        //constructors
        public BarRm() : base() 
        {

        }

        public BarRm(string name, int upkeepCost)
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
            //Bar for serving drinks
            throw new NotImplementedException();
        }
    }
}
