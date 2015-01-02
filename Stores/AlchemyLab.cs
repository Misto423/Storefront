using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Stores
{
    public class AlchemyLab : Store
    {
        //constructor
        public AlchemyLab(string name, byte layout)
        {
            //initializes overall store
            base.initStore(name, layout);
            //init starting rooms here
            //StartingRooms = Stores.RoomDataFiller.alcRooms;
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
            return "Alchemy Lab";
        }
    }
}
