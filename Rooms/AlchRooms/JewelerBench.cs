﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Rooms.AlchRooms
{
    public class JewelerBench : CreationRoom
    {
                //constructors
        public JewelerBench() : base() 
        {

        }

        public JewelerBench(string name, int upkeepCost)
            : base(name, upkeepCost)
        {

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
    }
}
