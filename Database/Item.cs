using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Database
{
    public class Item
    {
        private string name;
        private int PPU;
        private short inStock;
        private byte curDemand;
        private byte projDemand;
        private int avgCPrice;
        private int yourPrice;
        private short soldWeek;
        private int soldYear;
        private short boughtWeek;
        private int type;
        private float weight;

        public Item()
        {

        }

        public Item(Object[] row)
        {
            name = row[1].ToString();
            PPU = (int)row[2];
            inStock = (short)row[3];
            curDemand = Byte.Parse(row[4].ToString().Substring(0, 2));
            projDemand = Byte.Parse(row[4].ToString().Substring(3, 2));
            avgCPrice = (int)row[5];
            yourPrice = (int)row[6];
            soldWeek = (short)row[7];
            soldYear = (int)row[8];
            boughtWeek = (short)row[9];
            weight = (float)row[10];
            type = (int)row[14];
        }

        public Item(string name, int PPU, short inStock, byte curD, byte projD,
            int cPrice, int yPrice, short soldWk, int soldYr, short boughtWk, int type, float weight)
        {
            this.name = name;
            this.PPU = PPU;
            this.inStock = inStock;
            this.curDemand = curD;
            this.projDemand = projD;
            this.avgCPrice = cPrice;
            this.yourPrice = yPrice;
            this.soldWeek = soldWk;
            this.soldYear = soldYr;
            this.boughtWeek = boughtWk;
            this.type = type;
            this.weight = weight;
        }
    }
}
