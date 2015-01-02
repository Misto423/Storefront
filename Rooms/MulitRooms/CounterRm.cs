using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Rooms.MultiRooms
{
    public class CounterRm : Room
    {
        //constructors
        public CounterRm() : base() 
        {

        }

        public CounterRm(string name, int upkeepCost)
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

        public override int RoomType()
        {
            return 8;
        }

        public override string ToString()
        {
            return "Counter";
        }
    }
}
