using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Stores
{
    public class EquipmentStore : Store //inherit store class
    {
        private Texture2D rackEmpty;
        private Texture2D weaponRackHalf, weaponRackFull;
        private Texture2D armorRackHalf, armorRackFull;

        //constructor
        public EquipmentStore(string name, byte layout)
        {
            //initializes overall store
            base.initStore(name, layout);        
        }

        #region Accessors

        public Texture2D WeaponRackHalf
        {
            get { return weaponRackHalf; }
        }

        public Texture2D WeaponRackFull
        {
            get { return weaponRackFull; }
        }

        public Texture2D ArmorRackHalf
        {
            get { return armorRackHalf; }
        }

        public Texture2D ArmorRackFull
        {
            get { return armorRackFull; }
        }

        public Texture2D EmptyRack
        {
            get { return rackEmpty; }
        }

        #endregion

        public override void loadStore(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            weaponRackFull = cm.Load<Texture2D>("RoomTextures/EquipmentStore/WeaponRackFull");
            weaponRackHalf = cm.Load<Texture2D>("RoomTextures/EquipmentStore/WeaponRackHalf");
            armorRackHalf = cm.Load<Texture2D>("RoomTextures/EquipmentStore/ArmorRackHalf");
            armorRackFull = cm.Load<Texture2D>("RoomTextures/EquipmentStore/ArmorRackFull");
            rackEmpty = cm.Load<Texture2D>("RoomTextures/EquipmentStore/RackEmpty");
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
            return "Equipment Store";
        }
    }
}
