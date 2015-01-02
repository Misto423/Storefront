using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.EqRooms
{
    public class ArmorStand : DisplayRoom
    {
        public static int roomPrice = 800;
        public static int roomCounter = 0;
        public static string roomName = "Armor Stand";
        //constructors
        public ArmorStand() : base() 
        {
            MaximumCapacity = 5;
            MaximumWeight = 100;
        }

        public ArmorStand(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 5;
            MaximumWeight = 100;
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
            temp.Add(2);
            return temp;
        }
    }
}
