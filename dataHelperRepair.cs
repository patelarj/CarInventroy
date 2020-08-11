using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Assingment_2
{
    public class dataHelperRepair
    {
        private SqlConnection _sqconn;
        private SqlDataAdapter _sadapter;
        private SqlCommandBuilder _scmdbuilder;
        private DataSet _sdataset;
        private DataTable _repair;

        // Fill the data set and connect to database 
        public dataHelperRepair()
        {
            string cs = GetConnectionString("CarrepairMdf");
            string query = " SELECT * FROM repair";

            _sqconn = new SqlConnection(cs);
            _sadapter = new SqlDataAdapter(query, _sqconn);
            _scmdbuilder = new SqlCommandBuilder(_sadapter);

            FillDataSet();
        }

        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();

            return config["ConnectionStrings:" + connectionStringName];
        }

        private void FillDataSet()
        {
            _sdataset = new DataSet();

            _sadapter.Fill(_sdataset);

            _repair = _sdataset.Tables[0];

            DataColumn[] pk = new DataColumn[1];

            pk[0] = _repair.Columns["id"];
            _repair.PrimaryKey = pk;

        }

        // print the repair table using dataset 
        public void pritRepair()
        {
            FillDataSet();
            Console.WriteLine("------------------------REPAIR TABLE-----------------------------------------");
            Console.WriteLine("id     Inventory-id      What to Repair    \n" +
                              "----   ---------        -----------------   ");
            foreach (DataRow row in _repair.Rows)
            {
                Console.WriteLine($"{row["id"],0}{row["inventoryID"],10} {row["whatToRepair"],20} ");
            }

        }

        // insert the new repair entry
        // check if the inventory id is in the inventory table
        // check if the inventory id is in the repair table

        public void insertRepar(int inventoryId, string whatToRepair)
        {
            bool isInventoryIdinInventory = isInventoryIdInInventory(inventoryId);
            bool inventoryIdinRepair = isInventortIdInRepair(inventoryId);

            if (isInventoryIdinInventory)
            {
                if (inventoryIdinRepair) {

                    Console.WriteLine("There is alredy existing record for inventory " + inventoryId + " Please Update this Record");
                }
                else
                {
                    DataRow newRow = _repair.NewRow();
                    newRow["id"] = 0;

                newRow["inventoryID"] = inventoryId;
                newRow["whatToRepair"] = whatToRepair;


                _repair.Rows.Add(newRow);
                _sadapter.InsertCommand = _scmdbuilder.GetInsertCommand();

                _sadapter.Update(_repair);

                FillDataSet();
                pritRepair();
                }
            }
            else
            {
                Console.WriteLine("Please Enter the inventoryID from the Inventory Inventory table");
            }
            
        }
        
        // Update the current entry in the repair table
        // Check if the user input id is in te repair table 
        public void updateRepair(int id, string whatToRepair)
        {
            DataRow row = _repair.Rows.Find(id);
            if (row == null)
            {
                Console.WriteLine("-----------------------------------\n" +
                                  "This id is not avilable in repair table\n" +
                                 "-------------------------------------");

            }
            else
            {
                row["whatToRepair"] = whatToRepair;

                _sadapter.UpdateCommand = _scmdbuilder.GetUpdateCommand();
                _sadapter.Update(_repair);

                FillDataSet();
            }
            pritRepair();

        }

        // Chek if the inventory id is in inventory table and return the true and false
        public bool isInventoryIdInInventory(int id)
        {
           
            string cs = GetConnectionString("CarrepairMdf");
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string query = "SELECT id FROM inventory WHERE id = " + id + "";

            SqlCommand cummand = new SqlCommand(query, conn);
            var read = cummand.ExecuteReader();

            if (read.Read())
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return  false;
            }
        }

        // check if the inventory id in repair table and repair true and false
        public bool isInventortIdInRepair(int id)
        {
           
            string cs = GetConnectionString("CarrepairMdf");
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string query = "SELECT inventoryID FROM repair WHERE inventoryID = " + id + "";
            SqlCommand cummand = new SqlCommand(query, conn);
            var read = cummand.ExecuteReader();

            if (read.Read())
            {
                conn.Close();
                return  true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        public void deletRepair(int id)
        {
            DataRow row = _repair.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("-------------WARNING--------------\n" +
                                  "This id is not in the inventory list\n" +
                                 "-------------------------------------");
            }
            else
            {
                row.Delete();
                _sadapter.DeleteCommand = _scmdbuilder.GetDeleteCommand();
                _sadapter.Update(_repair);
                FillDataSet();
            }
         

        }
    }
}
