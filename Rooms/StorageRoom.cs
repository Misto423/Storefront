using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms
{
    public enum StoreageType
    {
        General,
        AnimalFood,
        Food,
        Alcohol
    }

    public abstract class StorageRoom : Room
    {
        private int storeInc;
        private StoreageType storeType;

        public StorageRoom() : base()
        {

        }

        public StorageRoom(string roomId, int upkeep)
            : base(roomId, upkeep)
        {

        }

        #region Attributes

        public StoreageType Type
        {
            get { return storeType; }
            set { storeType = value; }
        }

        public int StorageAmount
        {
            get { return storeInc; }
            set { storeInc = value; }
        }

        #endregion

        public override int RoomType()
        {
            return 2;
        }
    }
}
