using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms
{
    public abstract class CustomerRoom : Room
    {
        private int curClean, maxClean;
        private int curAppeal, maxAppeal;

        public CustomerRoom()
            : base()
        {

        }

        public CustomerRoom(string roomId, int upKeep)
            : base(roomId, upKeep)
        {

        }

        #region Attributes

        public int CurrentCleanliness
        {
            get { return curClean; }
            set { curClean = value; }
        }

        public int MaximumCleanliness
        {
            get { return maxClean; }
            set { maxClean = value; }
        }

        public int CurrentAppeal
        {
            get { return curAppeal; }
            set { curAppeal = value; }
        }

        public int MaximumAppeal
        {
            get { return maxAppeal; }
            set { maxAppeal = value; }
        }

        #endregion

        public override int RoomType()
        {
            return 4;
        }
    }
}
