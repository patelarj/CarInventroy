using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Assingment_2
{
    class DisplayMenu
    {
        // Display the Menue 
        public int mainMenu()
        {

            Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            Console.WriteLine("1. to Modify vehicles");
            Console.WriteLine("2. to Modify Inventory");
            Console.WriteLine("3. to Modify repair");
            Console.WriteLine("4. to exit the program");
            Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            Console.Write("\nEnter your choice: ");
            
            return int.Parse(Console.ReadLine());

            
        }

        public int vehicleMenu(string input)
        {
            Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            Console.WriteLine("1. to list all "+input+"");
            Console.WriteLine("2. to add a new " + input + "");
            Console.WriteLine("3. to update " + input + "");
            Console.WriteLine("4. to delete " + input + "");
            Console.WriteLine("5. to return main menu");
            Console.WriteLine("-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-");
            Console.Write("\nEnter your choice: ");
            
            return int.Parse(Console.ReadLine());
        }

     
        //collect input from the user and validate the input
        public void opOnVehicle(int cases)
        {
            string make;
            string model;
            int condition=3;
            int year=0;
            int id=0;
            DataHelper one = new DataHelper();

            // For the Update and delete for the vehical collaction 
            // check if input is valid 
            if (cases == 3 || cases == 4) 
            {
                do
                {
                    Console.WriteLine("Enter the id of the Vehical");
                    try{
                        id = Int32.Parse(Console.ReadLine());
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("input is not valid");
                    }
                }
                while (id<=0);
                if(cases == 4)
                {
                   one.deletVehicle(id);
                }
            }
            // For the Update and insert for the vehical collaction 
            // check if input is valid 
            if (cases == 2 || cases ==3 )
            {
                do
                {
                    Console.WriteLine("Please Enter the Make of the Vehicle\n ");
                    make = Console.ReadLine();
                    Console.WriteLine("Please Enter the Mode of the Vehicle\n");
                    model = Console.ReadLine();
                }
                while (!isStringValid(make)||!isStringValid(model));
                do
                {
                    try
                    {
                        Console.WriteLine("Please Enter the Year of the Vehicl from 1800 to 2020\n");
                        year = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Please Enter valid input");
                    }
                }
                while (year<1800 || year>2021);
                do
                {
                    try
                    {
                        Console.WriteLine("Please Enter 1 for new 0 for the use \n");
                        condition = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Please Enter valdi input");
                    }
                }
                while (condition > 1 || condition < 0);
                bool con;
                if (condition == 1)
                {
                    con = true;
                }
                else
                {
                    con = false;
                }

                if (cases == 2)
                {
                    one.InsertVehicle(make, model, year, con);
                }
                if (cases == 3)
                {
                    one.updateVehicle(id, make, model, year, con);
                }
               

            }
           
        }

        // operation on the inventory table and collect the the input
        // validate the input 
        public void opOnInventory(int cases)

        {

            int vehicalId = 0;
            int numbeOnHand = 0;
            double price=0;
            double cost = 0;
            int id = 0;
            dataHelperInventory one = new dataHelperInventory();
            if (cases == 3 || cases == 4)
            {
                do
                {
                    try
                    {
                        Console.WriteLine("Enter the id of the Inventory");
                        id = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Pleae Enter Valid input");
                    }
                }
                while (id == 0);
                if (cases == 4)
                {
                    one.deletInventory(id);
                }
            }
            if (cases == 2 || cases == 3)
            {
                
                while (vehicalId == 0 || price == 0 || cost == 0)
                {
                    try
                    {
                        Console.WriteLine("Please Enter Vehical id from the vehical table List\n ");
                        vehicalId = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Please Enter the Number of the unit on hand \n");
                        numbeOnHand = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Please Enter price of the vehical\n");
                        price = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Please Enter Cost of the vehical \n");
                        cost = Double.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Enter the valid input");
                    }
                }
                if (cases == 2)
                {
                    one.insertInventory(vehicalId, numbeOnHand, price, cost);
                }
                if (cases == 3)
                {
                    one.updateInventory(id, numbeOnHand, price, cost);
                }

            }
        }

        // For the Update and insert for the repair collaction 
        // check if input is valid 
        public void opOnRepair(int cases)
        {
            string watToRepair;
            int inventoryId = 0;
            int id = 0;
            dataHelperRepair one = new dataHelperRepair();
            if (cases == 3 || cases == 4)
            {
                while (id == 0)
                {
                    try
                    {
                        Console.WriteLine("Enter the id of the repairList");
                        id = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Please Enter Valid input");
                    }
                }
                if (cases == 4)
                {
                    one.deletRepair(id);
                }
            }
            if (cases == 2 || cases == 3)
            {
                while (inventoryId == 0)
                {
                    try
                    {
                        Console.WriteLine("Please Enter inventory id\n ");
                        inventoryId = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Please Enter the Valid input");
                    }
                }
                do
                {
                    Console.WriteLine("Please Enter what repair? \n");
                    watToRepair = Console.ReadLine();
                    if (isStringValid(watToRepair))
                    {

                        if (cases == 2)
                        {
                            one.insertRepar(inventoryId, watToRepair);
                        }
                        if (cases == 3)
                        {
                            one.updateRepair(id, watToRepair);
                        }
                    }
                }
                while (!isStringValid(watToRepair));
            }
        }

        //Check  if the input string is if empty or null 
        public bool isStringValid(string inputstring)
        {
            if (String.IsNullOrEmpty(inputstring))
            {
                Console.WriteLine("--------------------------------------" +
                                    "Please Enter the Valid input" +
                                "--------------------------------------");
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
