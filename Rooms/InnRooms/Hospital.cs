using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.InnRooms
{
    public class Hospital : ResearchRoom
    {
        //constructors
        public Hospital() : base() 
        {

        }

        public Hospital(string name, int upkeepCost)
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
            //provides various forms of healing, better with doctor or surgeon staff
            throw new NotImplementedException();
        }
    }
}
