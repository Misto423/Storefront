using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.StabRooms
{
    public class VetRoom : ResearchRoom
    {
        //constructors
        public VetRoom() : base() 
        {

        }

        public VetRoom(string name, int upkeepCost)
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
            //heal animals
            throw new NotImplementedException();
        }
    }
}
