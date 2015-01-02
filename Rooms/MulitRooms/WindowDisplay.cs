using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.MultiRooms
{
    //eq, gen, alch only
    public class WindowDisplay : DisplayRoom
    {
        public static int roomPrice = 450;
        public static int roomCounter = 0;
        public static string roomName = "Window Display";
        //constructors
        public WindowDisplay() : base() 
        {
            MaximumCapacity = 5;
            MaximumWeight = 65;
        }

        public WindowDisplay(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 5;
            MaximumWeight = 65;
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
            //add more types here later
            temp.Add(2);
            return temp;
        }

        public override string ToString()
        {
            return "Window Display";
        }
    }
}
