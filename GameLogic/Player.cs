using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Storefront.GameLogic
{
    //enum of all possible races the player can be
    //possible allow player to unlock certain races?
    public enum PRace
    {
        Human,
        Elf,
        Gnome,
        Dwarf,
        Goblin,
        Undead
    }

    public class Player
    {
        private string name; //name of the player
        private PRace race; //race of the player
        private bool gender; //true for male, false for female
        private byte strStat, intStat, awrStat, perStat, barStat, specStat, matStat; //vars to hold all stats of player
        private byte[] startingVals = new byte[7];
        private byte unspentPoints; //current unspent points, to be displayed to user & trigger buttons enabled/disabled
        private Stores.Store playerStore; //the store the player is running
        private Vector2 playerPosition; //the player's position in the store.
        private long availableMoney = 10000;
        private StorageInfo stores;
        private int satisfaction;
        private Chronology timeInGame;
        

        #region Attributes

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public PRace PlayerRace
        {
            get { return race; }
            set { race = value; }
        }
        public bool Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public byte Strength
        {
            get { return strStat; }
            set { strStat = value; }
        }
        public byte Intelligence
        {
            get { return intStat; }
            set { intStat = value; }
        }
        public byte Awareness
        {
            get { return awrStat; }
            set { awrStat = value; }
        }
        public byte Persuasion
        {
            get { return perStat; }
            set { perStat = value; }
        }
        public byte Barter
        {
            get { return barStat; }
            set { barStat = value; }
        }
        public byte Maintenance
        {
            get { return matStat; }
            set { matStat = value; }
        }
        public byte StoreStat
        {
            get { return specStat; }
            set { specStat = value; }
        }
        public byte AvailablePoints
        {
            get { return unspentPoints; }
            set { unspentPoints = value; }
        }
        public Stores.Store PlayerStore
        {
            get { return playerStore; }
            set { playerStore = value; }
        }
        /// <summary>
        /// Based on the store of the player, gets the table of items associated with that store.
        /// </summary>
        public string TableName
        {
            get
            {
                switch (playerStore.ToString())
                {
                    case "Alchemy Lab":
                        return "AlcStoreItems";
                    case "Bar":
                        return "BarStoreItems";
                    case "Equipment Store":
                        return "EqStoreItems";
                    case "General Store":
                        return "GenStoreItems";
                    case "Inn":
                        return "InnStoreItems";
                    case "Stables":
                        return "StStoreItems";
                    default:
                        return "Error";
                }
            }
        }
        public byte[] StartingStats
        {
            get { return startingVals; }
        }
        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }
        public StorageInfo StorageData
        {
            get { return stores; }
            set { stores = value; }
        }
        public int Satisfaction
        {
            get { return satisfaction; }
            set { satisfaction = value; }
        }
        public long CurrentMoney
        {
            get { return availableMoney; }
            set { availableMoney = value; }
        }

        public Chronology TimeInGame
        {
            get { return timeInGame; }
        }

        #endregion

        /// <summary>
        /// Call when a new player is created.
        /// </summary>
        public void initPlayer()
        {
            //set time to the beginning of the game for a new player.
            timeInGame = new Chronology();
            //set initial points
            AvailablePoints = 10;
            //set initial stats based on race & gender.
            switch (race)
            {
                case PRace.Human:
                    Strength = 2;
                    Intelligence = 2;
                    Awareness = 2;
                    Persuasion = 2;
                    Maintenance = 2;
                    StoreStat = 2;
                    Barter = 2;
                    for (int index = 0; index < 7; index++)
                    {
                        startingVals[index] = 2;
                    }
                    break;
                case PRace.Elf:
                    Strength = 1;
                    Intelligence = 3;
                    Awareness = 3;
                    Persuasion = 2;
                    Barter = 2;
                    Maintenance = 1;
                    StoreStat = 2;
                    //set starting vals array (for stats selection)
                    startingVals[0] = 1;
                    startingVals[1] = 3;
                    startingVals[2] = 3;
                    startingVals[3] = 2;
                    startingVals[4] = 2;
                    startingVals[5] = 1;
                    startingVals[6] = 2;
                    break;
                case PRace.Dwarf:
                    Strength = 4;
                    Intelligence = 1;
                    Awareness = 2;
                    Persuasion = 1;
                    Barter = 1;
                    Maintenance = 3;
                    StoreStat = 2;
                    //set starting vals array (for stats selection)
                    startingVals[0] = 4;
                    startingVals[1] = 1;
                    startingVals[2] = 2;
                    startingVals[3] = 1;
                    startingVals[4] = 1;
                    startingVals[5] = 3;
                    startingVals[6] = 2;
                    break;
                case PRace.Gnome:
                    Strength = 1;
                    Intelligence = 2;
                    Awareness = 2;
                    Persuasion = 3;
                    Barter = 3;
                    Maintenance = 1;
                    StoreStat = 2;
                    //set starting vals array (for stats selection)
                    startingVals[0] = 1;
                    startingVals[1] = 2;
                    startingVals[2] = 2;
                    startingVals[3] = 3;
                    startingVals[4] = 3;
                    startingVals[5] = 1;
                    startingVals[6] = 2;
                    break;
                case PRace.Undead:
                    Strength = 2;
                    Intelligence = 4;
                    Awareness = 1;
                    Persuasion = 3;
                    Barter = 2;
                    Maintenance = 0;
                    StoreStat = 2;
                    //set starting vals array (for stats selection)
                    startingVals[0] = 2;
                    startingVals[1] = 4;
                    startingVals[2] = 1;
                    startingVals[3] = 3;
                    startingVals[4] = 2;
                    startingVals[5] = 0;
                    startingVals[6] = 2;
                    break;
                case PRace.Goblin:
                    Strength = 2;
                    Intelligence = 1;
                    Awareness = 2;
                    Persuasion = 1;
                    Barter = 3;
                    Maintenance = 3;
                    StoreStat = 2;
                    //set starting vals array (for stats selection)
                    startingVals[0] = 2;
                    startingVals[1] = 1;
                    startingVals[2] = 2;
                    startingVals[3] = 1;
                    startingVals[4] = 3;
                    startingVals[5] = 3;
                    startingVals[6] = 2;
                    break;
            }

            if (gender == true)
            {
                Strength += 1;
                Maintenance += 1;
                startingVals[0] += 1;
                startingVals[5] += 1;
            }
            else
            {
                Awareness += 1;
                Persuasion += 1;
                startingVals[2] += 1;
                startingVals[3] += 1;
            }

            stores = new StorageInfo();
            satisfaction = 50;
        }
    }
}
