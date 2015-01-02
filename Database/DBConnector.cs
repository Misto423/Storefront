using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace Storefront.Database
{
    public class DBConnector
    {
        /// <summary>
        /// Connect to the database and get all data from it.
        /// </summary>
        /// <returns>A DataTable object storing the data.</returns>
        public DataTable GetItemsFromDB(string tableName)
        {
            DataSet data = new DataSet("Items");
            //get the file path to the database as a string
            string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\Database\\GameData.sdf";
            //connect to the database
            using (SqlCeConnection cntn = new SqlCeConnection("datasource=" + dbfile))
            {
                //create an adapter to pull all data from the table
                using (SqlCeDataAdapter adpt = new SqlCeDataAdapter("SELECT * FROM " + tableName, cntn))
                {
                    //put the data into a DataSet
                    adpt.Fill(data);
                }
                //close the conenction
                cntn.Close();
            }

            //fill the data from the Items table into a DataTable to return.
            DataTable itemTable = data.Tables[0];

            dbfile = "";
            data.Dispose();
            
            return itemTable;
        }

        /// <summary>
        /// Run a query and return data found by the query.
        /// </summary>
        /// <param name="Query">The query to run as a string.</param>
        /// <returns>A DataTable object containing the results of the query.</returns>
        public DataTable runQuery(string Query)
        {
            DataSet data = new DataSet("Items");
            //get the file path to the database as a string
            string dbfile = 
                new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + 
                "\\Database\\GameData.sdf";
            //connect to the database
            using (SqlCeConnection cntn = new SqlCeConnection("datasource=" + dbfile))
            {
                //create an adapter to pull all data from the table
                using (SqlCeDataAdapter adpt = new SqlCeDataAdapter(Query, cntn))
                {
                    //put the data into a DataSet
                    adpt.Fill(data);
                }
                //close the conenction
                cntn.Close();
            }

            //fill the data from the Items table into a DataTable to return.
            DataTable itemTable = data.Tables[0];

            dbfile = "";
            data.Dispose();

            return itemTable;
        }

        /// <summary>
        /// Updates the database, adding the amount that was purchased.
        /// </summary>
        /// <param name="itemName">The name of the item to update.</param>
        /// <param name="tableName">The name of the table to pull data from.</param>
        /// <param name="amount">The amount to add to the number in stock.</param>
        public void UpdateDatabaseInStock(string itemName, string tableName, int amount)
        {
            DataSet data = new DataSet("Items");
            int val;
            //get the file path to the database as a string
            string dbfile =
                new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName +
                "\\Database\\GameData.sdf";
            //connect to the database
            using (SqlCeConnection cntn = new SqlCeConnection("datasource=" + dbfile))
            {
                //create an adapter to pull all data from the table
                using (SqlCeDataAdapter adpt = new SqlCeDataAdapter
                    ("SELECT * FROM " + tableName + " WHERE Name LIKE '%" + itemName + "%'", cntn))
                {
                    //put the data into a DataSet
                    adpt.Fill(data);

                    cntn.Close();
                } 

                //fill the data from the Items table into a DataTable to return.
                DataTable itemTable = data.Tables[0];
                //pull out the row from the table.
                DataRow a = itemTable.Rows[0];
                //subtract one from the in stock value.
                val = (short)a.ItemArray[3] + amount;
                //pull out the item id of the item that is being purchased.
                int id = (int)a.ItemArray[0];
                //memory management, remove unused objects.
                dbfile = "";
                data.Dispose();
                itemTable.Dispose();
                //create the update command.
                SqlCeCommand cmd = new SqlCeCommand();
                //set the command connection.
                cmd.Connection = cntn;
                //reopen the connection.
                cntn.Open();
                //set the SQL update query.
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE " + tableName + " SET [In Stock] = @Value WHERE [ID] = @id";
                //Fill in parameters to the query.
                cmd.Parameters.AddWithValue("@Value", val);
                cmd.Parameters.AddWithValue("@ID", id);
                //run the query.
                cmd.ExecuteNonQuery();       
                //close the conenction
                cntn.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Used when customers buy items, subtracts one from the amount in stock.
        /// </summary>
        /// <param name="itemName">The name of the item to subtract from.</param>
        /// <param name="tableName">The name of the table to pull data from.</param>
        public void UpdateDatabaseInStock(string itemName, string tableName)
        {
            DataSet data = new DataSet("Items");
            int val;
            //get the file path to the database as a string
            string dbfile =
                new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName +
                "\\Database\\GameData.sdf";
            //connect to the database
            using (SqlCeConnection cntn = new SqlCeConnection("datasource=" + dbfile))
            {
                //create an adapter to pull all data from the table
                using (SqlCeDataAdapter adpt = new SqlCeDataAdapter
                    ("SELECT * FROM " + tableName + " WHERE Name LIKE '%" + itemName + "%'", cntn))
                {
                    //put the data into a DataSet
                    adpt.Fill(data);

                    cntn.Close();
                } 

                //fill the data from the Items table into a DataTable to return.
                DataTable itemTable = data.Tables[0];
                //pull out the row from the table.
                DataRow a = itemTable.Rows[0];
                //subtract one from the in stock value.
                val = (short)a.ItemArray[3] - 1;
                //pull out the item id of the item that is being purchased.
                int id = (int)a.ItemArray[0];
                //memory management, remove unused objects.
                dbfile = "";
                data.Dispose();
                itemTable.Dispose();
                //create the update command.
                SqlCeCommand cmd = new SqlCeCommand();
                //set the command connection.
                cmd.Connection = cntn;
                //reopen the connection.
                cntn.Open();
                //set the SQL update query.
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE " + tableName + " SET [In Stock] = @Value WHERE [ID] = @id";
                //Fill in parameters to the query.
                cmd.Parameters.AddWithValue("@Value", val);
                cmd.Parameters.AddWithValue("@ID", id);
                //run the query.
                cmd.ExecuteNonQuery();       
                //close the conenction
                cntn.Close();
                cmd.Dispose();
            }
        }
    }
}
