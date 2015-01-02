using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Database
{
    public class FinanceReport
    {
        private int spentonItems, spentonWages;
        private int revenue, profit;
        private string mostprofitable, mostpopular;
        private int avgCustomer;
        private int satisfaction;

        #region Attributes

        public int SpentOnItems
        {
            get { return spentonItems; }
            set { spentonItems = value; }
        }

        public int SpentOnWages
        {
            get { return spentonWages; }
            set { spentonWages = value; }
        }

        public int Revenue
        {
            get { return revenue; }
            set { revenue = value; }
        }

        public int Profit
        {
            get { return profit; }
            set { profit = value; }
        }

        public string MostProfitableItem
        {
            get { return mostprofitable; }
            set { mostprofitable = value; }
        }

        public string MostPopularItem
        {
            get { return mostpopular; }
            set { mostpopular = value; }
        }

        public int AverageCustomers
        {
            get { return avgCustomer; }
            set { avgCustomer = value; }
        }

        public int CustomerSatisfaction
        {
            get { return satisfaction; }
            set { satisfaction = value; }
        }

        #endregion

        public FinanceReport()
        {

        }
    }
}
