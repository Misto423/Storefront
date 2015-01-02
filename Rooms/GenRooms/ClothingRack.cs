using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.GenRooms
{
    public class ClothingRack : DisplayRoom
    {
        private List<Database.ShelfItem> shelfInv;
        //constructors
        public ClothingRack() : base() 
        {
            MaximumCapacity = 15;
            MaximumWeight = 45;
        }

        public ClothingRack(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 15;
            MaximumWeight = 45;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

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
            temp.Add(10);
            return temp;
        }
    }
}
