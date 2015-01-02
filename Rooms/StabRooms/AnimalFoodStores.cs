using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.StabRooms
{
    public class AnimalFoodStores : StorageRoom
    {
        //constructors
        public AnimalFoodStores() : base() 
        {
            Type = StoreageType.AnimalFood;
            GameLogic.GameGlobal.player.StorageData.MaximumAnimalFoodSpace += 20;
        }

        public AnimalFoodStores(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            Type = StoreageType.AnimalFood;
            GameLogic.GameGlobal.player.StorageData.MaximumAnimalFoodSpace += 20;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //store food for animals
            throw new NotImplementedException();
        }
    }
}
