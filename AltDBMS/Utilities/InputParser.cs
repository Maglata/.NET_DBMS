using DBMSPain.Utilities;
using OwnDBMS.Structures;
using System;
using System.Xml.Linq;

namespace OwnDBMS.Utilities
{
    internal class InputParser
    {
        public void RUN()
        {
            Console.WriteLine("Welcome to the Pain Simulator\n");
            while (true)
            {
                Console.Write("Enter a command: ");
                string userinput = Console.ReadLine();

                // CreateTable Sample(Id:int, Name:string, BirthDate:date default “01.01.2022”)
                // Insert INTO Sample (Id,Name) VALUES (1,“Ivan”)
                // Insert INTO Sample (Id,Name) VALUES (2,“Petar”)
                // Insert INTO Sample (Id,Name,BirthDate) VALUES (3,“Georgi”,"02.02.2022")
                // Insert INTO Sample (BirthDate,Id,Name) VALUES ("03.03.2022",4,"Meesho")

                // Select Name, BirthDate FROM Sample WHERE Id <> 5 AND DateBirth > “01.01.2000”
                // Select * FROM Sample WHERE Id <> 5 AND DateBirth > “01.01.2000”
                // Select Name, Id FROM Sample
                // Select Id, Name FROM Sample
                // Select Name, Dupe FROM Sample

                var splitinput = TableUtils.Split(userinput,' ', 2);      
                
                switch (TableUtils.ToUpper(splitinput[0]))
                {
                    case "CREATETABLE":                     
                        Commands.CreateTable(splitinput[1]);                        
                        break;
                    case "DROPTABLE":
                        FileManager.DeleteTableFile(splitinput[1]);
                        break;
                    case "LISTTABLES":
                        FileManager.GetTableNames();
                        break;
                    case "TABLEINFO":
                        FileManager.GetTableInfo(splitinput[1]);
                        break;
                    case "SELECT":
                        Commands.Select(splitinput[1]);
                        break;
                    case "INSERT":
                        Commands.Insert(splitinput[1]);
                        break;
                    case "HELP":
                        Console.WriteLine("Available Commands: CREATETABLE, DROPTABLE, LISTTABLES, TABLEINFO, SELECT, INSERT");
                        break;
                    case "STOP":
                        Console.WriteLine("Ai Chao");
                        return;                    
                    default:
                        Console.WriteLine("Invalid Command. If you want to exit type STOP");
                        break;
                }
            }
        }
    }
}
