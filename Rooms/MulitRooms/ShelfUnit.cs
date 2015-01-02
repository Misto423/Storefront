using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Rooms.MultiRooms
{
    public class ShelfUnit : DisplayRoom
    {
        public static int roomPrice = 1000;
        public static int roomCounter = 0;
        public static string roomName = "Shelf Unit";
        //constructors
        public ShelfUnit() : base() 
        {
            MaximumCapacity = 10;
            MaximumWeight = 50;
        }

        public ShelfUnit(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 10;
            MaximumWeight = 50;
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
            temp.Add(3); temp.Add(4); temp.Add(5);
            return temp;
        }

        public override string ToString()
        {
            return "Shelf Unit";
        }
    }
}
