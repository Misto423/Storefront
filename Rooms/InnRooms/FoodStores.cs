using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.InnRooms
{
    public class FoodStores : StorageRoom
    {
        //constructors
        public FoodStores() : base() 
        {
            Type = StoreageType.Food;
            GameLogic.GameGlobal.player.StorageData.MaximumFoodSpace += 20;
        }

        public FoodStores(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            Type = StoreageType.Food;
            GameLogic.GameGlobal.player.StorageData.MaximumFoodSpace += 20;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //store food
            throw new NotImplementedException();
        }
    }
}
