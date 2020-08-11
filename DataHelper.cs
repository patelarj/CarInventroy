using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Timers;

namespace Assingment_2
{
   public class DataHelper
    {
        private SqlConnection _sqconn;
        private SqlDataAdapter _sadapter;
        private SqlCommandBuilder _scmdbuilder;
        private DataSet _sdataset;
        private DataTable _vehicle;

        // Connection for data base

        public DataHelper()                                                         
        {
            string cs = GetConnectionString("CarrepairMdf");
            string query = "SELECT * FROM vehicle"; 

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

        // Create the data set and fill with the value and assing the table _vehical
        private void FillDataSet()                                            
        {
            _sdataset = new DataSet();

            _sadapter.Fill(_sdataset);
            _vehicle = _sdataset.Tables[0];
            

            DataColumn[] pk = new DataColumn[1];
            
            pk[0] = _vehicle.Columns["id"];
            _vehicle.PrimaryKey = pk;
           
                   }

        // Print the vehical data base using _vehical dataset
        public void PrintCars()                                                  
        {
            FillDataSet();
            Console.WriteLine("------------------------VEHICLE TABLE-----------------------------------------");
            Console.WriteLine("id     Make              Model          Year           Condition    \n" +
                             "----   ---------        ----------    ----------       -----------        ");
            foreach (DataRow row in _vehicle.Rows) 
            {
                
                Console.WriteLine($"{row["id"],0} {row["Make"],10} {row["Model"], 20} {row["Years"], 10} {row["Condition"], 15}");
                
            }
        }
        
        // insert new entry to vehical table by creating new new row and updat the datbase 
        public void InsertVehicle(string Make, string Model, int year, bool condi)
        {
           
            DataRow newRow = _vehicle.NewRow();
            newRow["id"] = 0;
            newRow["Make"] = Make;
            newRow["Model"] = Model;
            newRow["Years"] = year;
            if (condi)
            {
                newRow["Condition"] = "new";
            }
            else
            {
                newRow["Condition"] = "used";
            }
            _vehicle.Rows.Add(newRow);
            _sadapter.InsertCommand = _scmdbuilder.GetInsertCommand();
            _sadapter.Update(_vehicle);

            FillDataSet();
            PrintCars();
        }
        
        // update exixting vehical data in vehical table 
        // cheks if the provided id is in the table
        // if so update the table
        public void updateVehicle(int id, string Make, string Model, int year, bool condi)
        {
            
            DataRow row = _vehicle.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("-----------------------------------\n" +
                                  "This id is not in the vehical list\n" +
                                 "-------------------------------------");

            }
            else
            {
                row["Make"] = Make;
                row["Model"] = Model;
                row["Years"] = year;
                if (condi)
                {
                    row["Condition"] = "new";
                }
                else
                {
                    row["Condition"] = "used";
                }

                _sadapter.UpdateCommand = _scmdbuilder.GetUpdateCommand();
                _sadapter.Update(_vehicle);

                FillDataSet();
            }
            PrintCars();
            
        }
        // Delet the data for the vehical table
        //Check if provided id is avilable in the vehical table 
        // if avialble first it will delet accociat data from the inventory 
        // delete the data form vehical
        public void deletVehicle(int id)
        {
            DataRow row = _vehicle.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("-------------WARNING--------------\n" +
                                  "This id is not in the vehical list\n" +
                                 "-------------------------------------");
            }
            else
            {
                string cs = GetConnectionString("CarrepairMdf");
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string query_1 = "SELECT id FROM inventory WHERE vehicleID =" + id + " ";
                SqlCommand cummand = new SqlCommand(query_1, conn);
                var read = cummand.ExecuteReader();
                if (read.Read())
                {
                    int inventoryId = (int)read[0];
                   // Console.WriteLine(inventoryId);
                   dataHelperInventory one = new dataHelperInventory();
                    one.deletInventory(inventoryId);
                    conn.Close();
                }
                 row.Delete();
                _sadapter.DeleteCommand = _scmdbuilder.GetDeleteCommand();
                _sadapter.Update(_vehicle);

                FillDataSet();
            }
         

        }


    }
}
