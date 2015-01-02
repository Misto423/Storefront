using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic.MainGamePlay
{
    public struct Order
    {
        public string name;
        public int amtToOrder;
        public byte weeksLeft;
        public int totalCost;
    }

    //handles comparisons of order by name
    class compareNames : IComparer<Order>
    {
        int IComparer<Order>.Compare(Order x, Order y)
        {
            return x.name.CompareTo(y.name);
        }
    }

    public class OrderHandler
    {
        private List<Order> weeksOrder = new List<Order>();
        private Database.DBConnector dbc = new Database.DBConnector();

        /// <summary>
        /// Call at the end of every week.
        /// </summary>
        public void placeOrder()
        {
            for (int index = 0; index < weeksOrder.Count; index++)
            {
                //decrease money by totalCost.
                Order temp = weeksOrder[index];
                GameGlobal.player.CurrentMoney -= temp.totalCost;
                //subtract one from the weeks remaining on the order.
                temp.weeksLeft--;
                //update database with number in stock
                dbc.UpdateDatabaseInStock(temp.name, GameGlobal.player.TableName, temp.amtToOrder);
                weeksOrder[index] = temp;
            }

            //remove all the orders that are no longer recurring.
            weeksOrder.RemoveAll(delegate(Order o) { return o.weeksLeft <= 0; });
        }

        /// <summary>
        /// Searches for the item by name in the order list.
        /// </summary>
        /// <param name="itemName">The name of the item to search for.</param>
        /// <returns>The index of the item in the list. Negative if not found.</returns>
        public int itemSearch(string itemName)
        {
            IComparer<Order> ico = new compareNames();
            weeksOrder.Sort(ico);
            Order temp = new Order();
            temp.name = itemName;
            int index = weeksOrder.BinarySearch(temp, ico);
            return index;
        }

        /// <summary>
        /// Searches for an order of the same item in the order list.
        /// </summary>
        /// <param name="itemName">The order of the item to search for.</param>
        /// <returns>The index of the item in the list. Negative if not found.</returns>
        public int itemSearch(Order order)
        {
            IComparer<Order> ico = new compareNames();
            weeksOrder.Sort(ico);
            int index = weeksOrder.BinarySearch(order, ico);
            return index;
        }

        public bool overOrder(int index, Order order)
        {
            if (weeksOrder[index].amtToOrder + order.amtToOrder <= 255)
            {
                return true;
            }
            return false;
        }

        public int getTotalCost(Order order, int index)
        {
            return weeksOrder[index].totalCost + order.totalCost;
        }

        public void addItemToOrder(Order order, int index)
        {
            if (index >= 0)
            {
                Order modify = weeksOrder[index];
                modify.amtToOrder += order.amtToOrder;
                modify.totalCost += order.totalCost;
                weeksOrder[index] = modify;
            }
            else
            {
                weeksOrder.Add(order);
            }
        }

        public void removeItemFromOrder(Order order, int index)
        {
            if (index >= 0)
            {
                weeksOrder.RemoveAt(index);
            }
        }
    }
}
