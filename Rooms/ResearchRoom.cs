using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms
{
    public abstract class ResearchRoom : Room
    {
        private List<GameLogic.Research> research;

        public ResearchRoom()
            : base()
        {
            research = new List<GameLogic.Research>();
        }


        public ResearchRoom(string roomId, int upKeep)
            : base(roomId, upKeep)
        {
            research = new List<GameLogic.Research>();
        }

        #region Attributes

        public List<GameLogic.Research> ResearchList
        {
            get { return research; }
            set { research = value; }
        }

        #endregion

        public override int RoomType()
        {
            return 5;
        }
    }
}
