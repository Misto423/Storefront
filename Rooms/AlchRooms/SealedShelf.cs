using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.AlchRooms
{
    public class SealedShelf : DisplayRoom
    {
        //what is stored on the shelf.
        List<Database.ShelfItem> shelfInv;
                //constructors
        public SealedShelf() : base() 
        {
            MaximumCapacity = 15;
            MaximumWeight = 90;
        }

        public SealedShelf(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 15;
            MaximumWeight = 90;
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

        public override void UpdateTexture()
        {
            throw new NotImplementedException();
        }

        public override List<int> GetDisplayTypes()
        {
            List<int> temp = new List<int>();
            temp.Add(7);
            return temp;
        }
    }
}
