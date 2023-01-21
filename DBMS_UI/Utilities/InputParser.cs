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
            Console.WriteLine("Welcome to .NETSQL\n");
            while (true)
            {
                Console.Write("Enter a command: ");
                string userinput = Console.ReadLine();

                // To Do : Implement Trim
                // To Do : Make a function that gets commonly used file parts - First row, name etc.

                // CreateTable Sample2(Id:int, Name:string, BirthDate:date default “01.01.2022”) // Works
                // Insert INTO Sample2 (Id,Name) VALUES (1,“Ivan”) // Works
                // Insert INTO Sample2 (Id,Name) VALUES (2,“Petar”) // Works
                // Insert INTO Sample2 (Id,Name,BirthDate) VALUES (3,“Georgi”,"02.02.2022") // Works
                // Insert INTO Sample2 (BirthDate,Id,Name) VALUES ("03.03.2022",4,"Meesho") // Works
                // Insert INTO Sample2 (BirthDate,Id,Name) VALUES ("04.04.2022",5,"Spas") // Works
                // Insert INTO Sample2 (BirthDate,Id,Name) VALUES ("06.09.1999",69,"Evala")// Works

                // CreateTable SampleJoin(Id:int, AvgGrade:int, ClassName:string) // Works
                // INSERT INTO SampleJoin (Id,AvgGrade,ClassName) VALUES (1,5.50,"Class A") // Works

                // Select Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" // Works
                // Select Name, BirthDate FROM Sample WHERE Id <> 5 AND ( BirthDate > "01.01.2000" OR Name = "Ivan" ) // Works
                // Select Name, BirthDate FROM Sample WHERE Id <> 3 AND BirthDate > "01.01.2002" // Works
                // Select Distinct * FROM Sample WHERE Id < 5 OR BirthDate > "01.01.2004" // Works
                // Select * FROM Sample WHERE Id < 5 OR BirthDate > "01.01.2004"
                // Select Name, Id FROM Sample // Works
                // Select Id, Name FROM Sample // Works
                // Select Name, Dupe FROM Sample // Works

                //Select Distinct Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" // Works
                //Select Distinct * FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" // Works
                //Select Distinct Name, BirthDate FROM Sample // Works
                //Select Distinct * FROM Sample // Works
                //Select Distinct * FROM Sample ORDER BY Name DESC // Works
                //Select Name, BirthDate FROM Sample ORDER BY Id DESC // Works
                //Select Distinct Name, BirthDate FROM Sample ORDER BY Id DESC // Works
                //Select Distinct * FROM Sample ORDER BY Id DESC // Works
                //Select Distinct * FROM Sample ORDER BY BirthDate DESC // Works

                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" ORDER BY Name // Works
                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" ORDER BY Name ASC // Works
                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > "01.01.2000" ORDER BY Name DESC // Works
                //SELECT DISTINCT * FROM Sample WHERE Id <> 5 AND BirthDate > "05.04.2003" ORDER BY Id DESC // Works

                //DELETE FROM Sample WHERE Id = 8 OR Name = "Petar" // Works
                //DELETE FROM SampleCopy WHERE Name = "Petar" OR Id > 8 // Works

                //CREATEINDEX bd_index ON Sample2 (BirthDate) // Works
                //DROPINDEX bd_index ON Sample (BirthDate) // Works
                //CREATEINDEX id_index ON Sample (Id) // Works

                // Insert INTO Sample (Id,Name) VALUES (70,"Test")

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
                        //FileManager.GetTableInfo(splitinput[1]);
                        break;
                    case "SELECT":
                        Commands.Select(splitinput[1]);
                        break;
                    case "INSERT":
                        Commands.Insert(splitinput[1]);
                        break;
                    case "HELP":
                        Console.WriteLine("Available Commands: CREATETABLE, DROPTABLE, LISTTABLES, TABLEINFO, SELECT, INSERT, DELETE");
                        break;
                    case "DELETE":
                        Commands.Delete(splitinput[1]);
                        break;
                    case "CREATEINDEX":
                        FileManager.CreateIndex(splitinput[1]);
                        break;
                    case "DROPINDEX":
                        FileManager.DeleteIndex(splitinput[1]);
                        break;
                    case "TEST":
                        Console.WriteLine("Nothing put in Test Section");
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
