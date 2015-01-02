using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms
{
    public abstract class IdleRoom : Room
    {
        //stat being the amount it increases a specific stat, such as income.
        public int curStat, maxStat;

        public IdleRoom()
            : base()
        {

        }

        public IdleRoom(string roomId, int upKeep)
            : base(roomId, upKeep)
        {

        }

        #region Attributes

        public int CurrentChange
        {
            get { return curStat; }
            set { curStat = value; }
        }

        public int MaximumChange
        {
            get { return maxStat; }
            set { maxStat = value; }
        }

        #endregion

        public override int RoomType()
        {
            return 6;
        }
    }
}
