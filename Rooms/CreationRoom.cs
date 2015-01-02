using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms
{
    public abstract class CreationRoom : Room
    {
        private List<Database.Item> create;
        private byte lev;

        public CreationRoom()
            : base()
        {
            create = new List<Database.Item>();
            lev = 1;
        }

        public CreationRoom(string roomId, int upKeep)
            : base(roomId, upKeep)
        {
            create = new List<Database.Item>();
            lev = 1;
        }

        #region Attributes

        public List<Database.Item> CreatableItems
        {
            get { return create; }
            set { create = value; }
        }

        public byte Level
        {
            get { return lev; }
            set { lev = value; }
        }

        #endregion

        public override int RoomType()
        {
            return 3;
        }
    }
}
