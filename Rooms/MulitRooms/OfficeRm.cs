using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Rooms.MultiRooms
{
    public class OfficeRm : Room
    {
        //constructors
        public OfficeRm() : base() 
        {
            //set dimensions and initial rotation to up
            TileDimensions = new Microsoft.Xna.Framework.Vector2(1, 1);
            Rotation = 0;
        }

        public OfficeRm(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            //set dimensions and initial rotation to up
            TileDimensions = new Microsoft.Xna.Framework.Vector2(1, 1);
            Rotation = 0;
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
            return 7;
        }

        public override string ToString()
        {
            return "Office";
        }
    }
}
