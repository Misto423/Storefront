using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.GenRooms
{
    public class IceCase : DisplayRoom
    {
        private List<Database.ShelfItem> shelfInv;
        //constructors
        public IceCase() : base() 
        {
            MaximumCapacity = 20;
            MaximumWeight = 70;
        }

        public IceCase(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 20;
            MaximumWeight = 70;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //stores food items, prevents them from expiring
            throw new NotImplementedException();
        }

        public override void UpdateTexture()
        {
            throw new NotImplementedException();
        }

        public override List<int> GetDisplayTypes()
        {
            List<int> temp = new List<int>();
            temp.Add(5);
            return temp;
        }
    }
}
