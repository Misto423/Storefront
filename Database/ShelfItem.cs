using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Database
{
    public class ShelfItem
    {
        private string name;
        private int onShelf;
        private int cost;
        private float weight;
        private int curDemand;

        public ShelfItem()
        {

        }

        public ShelfItem(string name, int onShelf, int cost, float weight, int curD)
        {
            this.name = name;
            this.onShelf = onShelf;
            this.cost = cost;
            this.weight = weight;
            this.curDemand = curD;
        }

        #region Attributes

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public int OnShelf
        {
            get { return onShelf; }
            set { onShelf = value; }
        }

        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public int CurrentDemand
        {
            get { return curDemand; }
            set { curDemand = value; }
        }

        #endregion
    }
}
