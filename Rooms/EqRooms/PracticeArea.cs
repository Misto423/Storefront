using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.EqRooms
{
    public class PracticeArea : ResearchRoom
    {
        public static int roomPrice = 7500;
        public static int roomCounter = 0;
        public static string roomName = "Training Ground";
        //constructors
        public PracticeArea() : base() 
        {

        }

        public PracticeArea(string name, int upkeepCost)
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
            //trigger more sales, passive ability, boosted if sparring partner is in room
            throw new NotImplementedException();
        }
    }
}
