using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic
{
    public class StorageInfo
    {
        //storage vars
        private int foodStoresUsed, maxFoodStores;
        private int alcStoresUsed, maxAlcStores;
        private int aFoodStoresUsed, maxAFoodStores;
        private int genStorageUsed, maxGenStorage;

        public int FoodSpaceUsed
        {
            get { return foodStoresUsed; }
            set { foodStoresUsed = value; }
        }

        public int MaximumFoodSpace
        {
            get { return maxFoodStores; }
            set { maxFoodStores = value; }
        }

        public int AlcoholSpaceUsed
        {
            get { return alcStoresUsed; }
            set { alcStoresUsed = value; }
        }

        public int MaximumAlcoholSpace
        {
            get { return maxAlcStores; }
            set { maxAlcStores = value; }
        }

        public int AnimalFoodSpaceUsed
        {
            get { return aFoodStoresUsed; }
            set { aFoodStoresUsed = value; }
        }

        public int MaximumAnimalFoodSpace
        {
            get { return maxAFoodStores; }
            set { maxAFoodStores = value; }
        }

        public int GeneralStorageSpaceUsed
        {
            get { return genStorageUsed; }
            set { genStorageUsed = value; }
        }

        public int MaximumStorageSpace
        {
            get { return maxGenStorage; }
            set { maxGenStorage = value; }
        }
    }
}
