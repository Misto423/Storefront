using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Stores
{
    public class Stables : Store
    {
        //constructor
        public Stables(string name, byte layout)
        {
            //initializes overall store
            base.initStore(name, layout);
            //init starting rooms here
            //StartingRooms = Stores.RoomDataFiller.stRooms;
            //Stores.RoomDataFiller.unloadStartingRooms();
        }

        public override void loadStore(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            throw new NotImplementedException();
        }

        public override void updateStore(GameTime gt)
        {
            throw new NotImplementedException();
        }

        public override void drawStore(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Stables";
        }
    }
}
