using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.BarRooms
{
    public class WineCellar : StorageRoom
    {
        //constructors
        public WineCellar() : base() 
        {
            Type = StoreageType.Alcohol;
            GameLogic.GameGlobal.player.StorageData.MaximumAlcoholSpace += 20;
        }

        public WineCellar(string name, int upkeepCost)
            : base(name, upkeepCost)
        {
            Type = StoreageType.Alcohol;
            GameLogic.GameGlobal.player.StorageData.MaximumAlcoholSpace += 20;
        }

        public override void initRoom(Microsoft.Xna.Framework.Content.ContentManager cm)
        {

        }

        /// <summary>
        /// Function that, when clicked or perhaps time based, triggers an action specific to room/piece.
        /// </summary>
        public override void performFunction()
        {
            //store alcholic beverages
            throw new NotImplementedException();
        }
    }
}
