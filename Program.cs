using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace Assingment_2
{
    class Program
    {
        static void Main(string[] args)
        {

            DisplayMenu display = new DisplayMenu();                                    // class for the Dispay all the Menue and user input testing
            DataHelper one = new DataHelper();                                          // Class for vehical table handel all the vehical table manipulation 
            dataHelperInventory inventory = new dataHelperInventory();                  //Class for inventory table handel all the inventory table data manipulation
            dataHelperRepair repair = new dataHelperRepair();                           //Class for repair table handel all the repair table data manioulation
            do
            {
                int choice = display.mainMenu();
                bool menu = true;
                switch (choice)
                {
                    case 1:                                                                 // Case for the vehical
                        
                        do
                        {
                            String input = "vehicle";
                            
                           choice = display.vehicleMenu(input);
                            switch (choice)
                            {
                                case 1:
                                    one.PrintCars();
                                    break;
                                case 2:
                                    one.PrintCars();
                                    display.opOnVehicle(choice);
                                    break;
                                case 3:
                                    one.PrintCars();
                                    display.opOnVehicle(choice);                           
                                    break;
                                case 4:
                                    one.PrintCars();
                                    display.opOnVehicle(choice);
                                    one.PrintCars();
                                    break;
                                case 5:
                                    menu = false;
                                    break;
                            }
                        }
                        while (menu);
                        break;
                    case 2:                                                                                        // case for the Inventory     
                        
                        do
                        {
                            string input = "Inventory";

                            choice = display.vehicleMenu(input);
                            //choice = display.inventoryMenu();
                            switch (choice)
                            {
                                case 1:
                                    inventory.pritInventory();
                                    break;
                                case 2:
                                    inventory.pritInventory();
                                    display.opOnInventory(choice);
                                    break;
                                case 3:
                                    inventory.pritInventory();
                                    Console.WriteLine("-----------------------------WARNING------------------------\n" +
                                                      "Update will not update vehicalID to avoid duplicate record\n" +
                                                       "--------------------------------------------------------------");
                                    display.opOnInventory(choice);
                                    break;
                                case 4:
                                    inventory.pritInventory();
                                    display.opOnInventory(choice);
                                    inventory.pritInventory();
                                    break;
                                case 5:
                                    menu = false;
                                    break;
                            }
                        }
                        while (menu);
                        break;
                    case 3:                                                                                            // case for the repair 
                        do
                        {
                            string input = "Repair";

                            choice = display.vehicleMenu(input);
                            switch (choice)
                            {
                                case 1:
                                    repair.pritRepair();
                                    break;
                                case 2:
                                    repair.pritRepair();
                                    display.opOnRepair(choice);
                                    break;
                                case 3:
                                    repair.pritRepair();
                                    Console.WriteLine("-----------------------------WARNING------------------------\n" +
                                                      "Update will not update inventoryID to avoid duplicate record\n" +
                                                       "--------------------------------------------------------------");
                                    display.opOnRepair(choice);
                                    break;
                                case 4:
                                    repair.pritRepair();
                                    display.opOnRepair(choice);
                                    repair.pritRepair();
                                    break;
                                case 5:
                                    menu = false;
                                    break;
                            }
                        }
                        while (menu);
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                }

            } while (true);
      
        }

    }
}
