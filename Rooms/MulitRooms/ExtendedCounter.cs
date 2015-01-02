using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.MultiRooms
{
    public class ExtendedCounter : DisplayRoom
    {
        public static int roomPrice = 250;
        public static int roomCounter = 0;
        public static string roomName = "Extended Counter";
        //constructors
        public ExtendedCounter() : base() 
        {
            MaximumCapacity = 10;
            MaximumWeight = 45;
        }

        public ExtendedCounter(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 10;
            MaximumWeight = 45;
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
            temp.Add(3); temp.Add(4);
            return temp;
        }

        public override string ToString()
        {
            return "Extended Counter";
        }
    }
}
