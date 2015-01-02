using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Storefront.Stores
{
    public static class RoomDataFiller
    {
        public static List<Texture2D> eqRooms, genRooms, alcRooms, stRooms, innRooms, barRooms;
        public static List<string> eqRoomNames;

        public static void loadRoomTextures(ContentManager cm)
        {
            //setup lists
            eqRooms = new List<Texture2D>();
            genRooms = new List<Texture2D>();
            alcRooms = new List<Texture2D>();
            stRooms = new List<Texture2D>();
            innRooms = new List<Texture2D>();
            barRooms = new List<Texture2D>();

            eqRoomNames = new List<string>();
            //load ALL room textures here, then unload them once a store is selected
            Texture2D temp;
            //load office piece and place in all lists
            temp = cm.Load<Texture2D>(@"RoomTextures/Global/Office");
            eqRooms.Add(temp);
            genRooms.Add(temp);
            alcRooms.Add(temp);
            stRooms.Add(temp);
            innRooms.Add(temp);
            barRooms.Add(temp);

            eqRoomNames.Add("Office");
            //--------------------------
            //load counter texture
            temp = cm.Load<Texture2D>(@"RoomTextures/Global/Desk");
            eqRooms.Add(temp);
            genRooms.Add(temp);
            alcRooms.Add(temp);
            stRooms.Add(temp);
            innRooms.Add(temp);
            barRooms.Add(temp);

            eqRoomNames.Add("Counter");
            //--------------------------
            //load Shelf Unit texture
            temp = cm.Load<Texture2D>(@"RoomTextures/TestPieces/ShelfUnitTest");
            eqRooms.Add(temp);
            genRooms.Add(temp);
            genRooms.Add(temp);
            alcRooms.Add(temp);
            stRooms.Add(temp);
            innRooms.Add(temp);
            barRooms.Add(temp);

            eqRoomNames.Add("Shelf Unit");
            //eq rooms
            //load Weapon Rack texture
            temp = cm.Load<Texture2D>(@"RoomTextures/EquipmentStore/RackEmpty");
            eqRooms.Add(temp);

            eqRoomNames.Add("Weapon Rack");
            //load Armor Rack texture
            temp = cm.Load<Texture2D>(@"RoomTextures/EquipmentStore/RackEmpty");
            eqRooms.Add(temp);

            eqRoomNames.Add("Armor Rack");

            //general rooms

        }

        public static void unloadUnusedTextures()
        {
            //eq rooms
            foreach (Texture2D t2d in eqRooms)
            {
                t2d.Dispose();
            }
            eqRooms.RemoveRange(0, eqRooms.Count);

            //st rooms
            foreach (Texture2D t2d in stRooms)
            {
                t2d.Dispose();
            }
            stRooms.RemoveRange(0, stRooms.Count);

            //bar rooms
            foreach (Texture2D t2d in barRooms)
            {
                t2d.Dispose();
            }
            barRooms.RemoveRange(0, barRooms.Count);

            //alc rooms
            foreach (Texture2D t2d in alcRooms)
            {
                t2d.Dispose();
            }
            alcRooms.RemoveRange(0, alcRooms.Count);

            //inn rooms
            foreach (Texture2D t2d in innRooms)
            {
                t2d.Dispose();
            }
            innRooms.RemoveRange(0, innRooms.Count);

            //gen rooms
            foreach (Texture2D t2d in genRooms)
            {
                t2d.Dispose();
            }
            genRooms.RemoveRange(0, genRooms.Count);
        }
    }
}
