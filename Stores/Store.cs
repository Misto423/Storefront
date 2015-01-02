using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storefront.Stores
{
    public abstract class Store
    {
        private string name; //name of the store
        private List<Rooms.Room> roomList;  //unordered list of rooms
        private List<Rooms.Room> startingRooms; //the rooms the player can initially place.
        //whether an object is in that location
        //Rooms will store their own actual location
        //and check to see whether they are adajcent to a walking space
        //(empty space) denoted by false.  True if occupied
        private Layout buildingSpaces;
        //for second floor
        private Layout buildingSpaces2;

        //security value - integer value determining how safe the store is.
        //affected by number of guard, and guards' rating, as well as the player's
        //strength stat.  secChance is the customers' chance at a successful steal.
        private byte security, secChance;

        #region Attributes
        /// <summary>
        /// Gets or Sets the name of the store.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Gets or sets the current list of rooms placed in the store.
        /// </summary>
        public List<Rooms.Room> RoomList
        {
            get { return roomList; }
            set { roomList = value; }
        }
        /// <summary>
        /// Gets or sets the starting rooms available to place.
        /// </summary>
        public List<Rooms.Room> StartingRooms
        {
            get { return startingRooms; }
            set { startingRooms = value; }
        }
        /// <summary>
        /// Gets or sets the array of building spaces (occupied or not)
        /// </summary>
        public bool?[,] BuildingArray
        {
            get { return buildingSpaces.StoreLayout; }
            set { buildingSpaces.StoreLayout = value; }
        }
        public Vector2 DoorPosition
        {
            get { return buildingSpaces.DoorPos; }
        }

        public byte SecurityValue
        {
            get { return calcSecurity(); }
        }

        public byte StealChance
        {
            get { return calcStealChance(); }
        }

        #endregion

        //override for each sub class to set starting rooms and any other
        //store specific logic.
        /// <summary>
        /// Initializes the player's store
        /// </summary>
        /// <param name="name">Name of the store.</param>
        public void initStore(string name, byte layout)
        {
            //set store name
            this.name = name;
            //allocate space for room list
            roomList = new List<Rooms.Room>();
            //init layout
            buildingSpaces = new Layout(layout);
            buildingSpaces2 = new Layout(layout);
        }

        private byte calcSecurity()
        {
            byte temp = 15;
            temp -= (byte)(GameLogic.GameGlobal.player.Strength / 3);
            return temp;
        }
        // Make sure to add in code for number of guards in here, once employees are figured out.
        private byte calcStealChance()
        {
            byte temp = 50;
            temp -= (byte)(GameLogic.GameGlobal.player.Strength / 2);
            return temp;
        }
        
        //abstract methods
        /// <summary>
        /// Load any assets needed by the store
        /// </summary>
        public abstract void loadStore(Microsoft.Xna.Framework.Content.ContentManager cm);
        /// <summary>
        /// Update the current store
        /// </summary>
        /// <param name="gt">The current time of the game.</param>
        public abstract void updateStore(GameTime gt);
        /// <summary>
        /// Draw the current store.
        /// </summary>
        /// <param name="sb">Spritebatch to handle drawing items.</param>
        public abstract void drawStore(SpriteBatch sb);
        /// <summary>
        /// Gets the type of the store as a string.
        /// </summary>
        /// <returns>A string for the type of store.</returns>
        public abstract override string ToString();
    }
}
