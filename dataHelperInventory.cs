using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Assingment_2
{
    public class dataHelperInventory
    {
        private SqlConnection _sqconn;
        private SqlDataAdapter _sadapter;
        private SqlCommandBuilder _scmdbuilder;
        private DataSet _sdataset;
        private DataTable _inventory;


        // Execute the query and connect ot database  
        public dataHelperInventory()
        {
            string cs = GetConnectionString("CarrepairMdf");
            string query = " SELECT * FROM inventory";

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

        // fill the dataset and assing the value _inventory
        private void FillDataSet()
        {
            _sdataset = new DataSet();

            _sadapter.Fill(_sdataset);

            _inventory = _sdataset.Tables[0];

            DataColumn[] pk = new DataColumn[1];

            pk[0] = _inventory.Columns["id"];
            _inventory.PrimaryKey = pk;

        }

        // Print the inventory using data set 
        public void pritInventory()
        {
            FillDataSet();
            Console.WriteLine("------------------------iNVENTORY TABLE-----------------------------------------");
            Console.WriteLine("id     vehicle-id         Number on Hand          Price           Cost    \n" +
                              "----   ---------        -----------------       ----------       -----------        ");
            foreach (DataRow row in _inventory.Rows)
            {
                Console.WriteLine($"{row["id"],5}{row["vehicleID"],10} {row["numberOnHand"],20} {row["price"],20} {row["cost"],15}");
            }

        }

        // insert new data to inventory table 
        // check if vehicalID is in the vehical table
        // check if vehicalID is already in the inventory table 
        // update the inventory

        public void insertInventory(int vehicleid, int numberOnHand, double price, double cost)
        {


            bool vehicleidIsAvilable = findvehicalid(vehicleid);
            bool isVehicalidInInventory = isVehicalIdininventory(vehicleid);
            if (vehicleidIsAvilable )
            {

                if (isVehicalidInInventory)
                {
                    Console.WriteLine("---------------------------------WARNING------------------------------------\n" +
                                    "VehicalID is already in the  inventory table please update Existing Entry insted\n" +
                                    "-------------------------------------------------------------------------------");
                }
                else
                {
                    
                        DataRow newRow = _inventory.NewRow();
                        newRow["id"] = 0;

                        newRow["vehicleID"] = vehicleid;
                        newRow["numberOnHand"] = numberOnHand;
                        newRow["price"] = price;
                        newRow["cost"] = cost;

                        _inventory.Rows.Add(newRow);
                        _sadapter.InsertCommand = _scmdbuilder.GetInsertCommand();

                        _sadapter.Update(_inventory);

                        FillDataSet();
                        pritInventory();
                    

                }
            }
            else
            {
                
                    Console.WriteLine("---------------------WARNING-------------------\n" +
                                    "Plese Enter the Vehical ID from the Vehicl Table \n" +
                                    "----------------------------------------------- ");
                

            }

        }

        // Update the inventory table
        // check if user input id is avilable in inventory table 
        public void updateInventory(int id, int numberOnHand, double price, double cost)
        {
            


                DataRow row = _inventory.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("-----------WARNING-------------\n" +
                                  "This id is not in the vehical list\n" +
                                 "-------------------------------------");

            }
            else
            {
                row["numberOnHand"] = numberOnHand;
                row["price"] = price;
                row["cost"] = cost;

                _sadapter.UpdateCommand = _scmdbuilder.GetUpdateCommand();
                _sadapter.Update(_inventory);

                FillDataSet();
            }
                pritInventory();
            

            

        }

        // Delete the data from the inventory 
        // check if id is avilable in the inventery data table

        public void deletInventory(int id)
        {
            DataRow row = _inventory.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("-------------WARNING--------------\n" +
                                  "This id is not in the inventory list\n" +
                                 "-------------------------------------");
            }
            else
            {
                string cs = GetConnectionString("CarrepairMdf");
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();
                string query = "SELECT id FROM repair WHERE inventoryID = " + id + "";
               SqlCommand cummand = new SqlCommand(query, conn);
                var read = cummand.ExecuteReader();
                if (read.Read())
                {
                    int repairid = (int)read[0];
                    dataHelperRepair one = new dataHelperRepair();
                    one.deletRepair(repairid);
                    conn.Close();
                }
                
                row.Delete();
                _sadapter.DeleteCommand = _scmdbuilder.GetDeleteCommand();
                _sadapter.Update(_inventory);
                FillDataSet();
            }
       

        }

        // method to check if vehical id is in vehical table 
        public bool findvehicalid(int id)
        {
            
            string cs = GetConnectionString("CarrepairMdf");
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string query = "SELECT id FROM vehicle WHERE id = " + id + "";

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
                return  false;
            }
            
        }

        //Method to check if vehicalID is in the inventory table 

        public bool isVehicalIdininventory(int id)
        {
            
            string cs = GetConnectionString("CarrepairMdf");
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            string query = "SELECT vehicleID FROM inventory WHERE vehicleID = " + id + "";
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
    }
   
}
