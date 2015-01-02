using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Rooms.EqRooms
{
    public class WeaponRack : DisplayRoom
    {
        public static int roomPrice = 2000;
        public static int roomCounter = 0;
        public static string roomName = "Weapon Rack";
        //constructors
        public WeaponRack() : base() 
        {
            MaximumCapacity = 15;
            MaximumWeight = 250;
        }

        public WeaponRack(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            MaximumCapacity = 15;
            MaximumWeight = 250;
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
            if (this.CurrentCapacity == 0)
            {
                this.RoomTexture = ((Stores.EquipmentStore)(GameLogic.GameGlobal.player.PlayerStore)).EmptyRack;
            }
            else if (this.CurrentCapacity <= (0.5 * this.MaximumCapacity))
            {
                this.RoomTexture = ((Stores.EquipmentStore)(GameLogic.GameGlobal.player.PlayerStore)).WeaponRackHalf;
            }
            else
            {
                this.RoomTexture = ((Stores.EquipmentStore)(GameLogic.GameGlobal.player.PlayerStore)).WeaponRackFull;
            }
        }

        public override List<int> GetDisplayTypes()
        {
            List<int> temp = new List<int>();
            temp.Add(1);
            return temp;
        }
    }
}
