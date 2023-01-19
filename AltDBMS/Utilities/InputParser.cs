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

                // CreateTable Sample(Id:int, Name:string, BirthDate:date default “01.01.2022”)
                // Insert INTO Sample (Id,Name) VALUES (1,“Ivan”)
                // Insert INTO Sample (Id,Name) VALUES (2,“Petar”)
                // Insert INTO Sample (Id,Name,BirthDate) VALUES (3,“Georgi”,"02.02.2022")
                // Insert INTO Sample (BirthDate,Id,Name) VALUES ("03.03.2022",4,"Meesho")
                // Insert INTO Sample (BirthDate,Id,Name) VALUES ("04.04.2022",5,"Spas")
                // Insert INTO Sample (BirthDate,Id,Name) VALUES ("06.09.1999",69,"Evala")

                // CreateTable SampleJoin(Id:int, AvgGrade:int, ClassName:string)
                // INSERT INTO SampleJoin (Id,AvgGrade,ClassName) VALUES (1,5.50,"Class A")

                // Select Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000”
                // Select Name, BirthDate FROM Sample WHERE Id <> 5 AND ( BirthDate > “01.01.2000” OR Name = "Ivan" )
                // Select Name, BirthDate FROM Sample WHERE Id <> 3 AND BirthDate > "01.01.2002"
                // Select Distinct * FROM Sample WHERE Id < 5 OR BirthDate > “01.01.2004”
                // Select * FROM Sample WHERE Id < 5 OR BirthDate > “01.01.2004”
                // Select Name, Id FROM Sample
                // Select Id, Name FROM Sample
                // Select Name, Dupe FROM Sample

                //Select Distinct Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000”
                //Select Distinct * FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000”
                //Select Distinct Name, BirthDate FROM Sample
                //Select Distinct * FROM Sample
                //Select Distinct * FROM Sample ORDER BY Name DESC
                //Select Name, BirthDate FROM Sample ORDER BY Id DESC // Works
                //Select Distinct Name, BirthDate FROM Sample ORDER BY Id DESC // Works
                //Select Distinct * FROM Sample ORDER BY Id DESC
                //Select Distinct * FROM Sample ORDER BY BirthDate DESC // Works

                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000” ORDER BY Name
                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000” ORDER BY Name ASC
                //SELECT DISTINCT Name, BirthDate FROM Sample WHERE Id <> 5 AND BirthDate > “01.01.2000” ORDER BY Name DESC
                //SELECT DISTINCT * FROM Sample WHERE Id <> 5 AND BirthDate > “05.04.2003” ORDER BY Id DESC

                //DELETE FROM Sample WHERE Id > 8 OR Name = "Petar"

                //CREATEINDEX bd_index ON Sample (BirthDate)
                //DROPINDEX bd_index ON Sample (BirthDate)
                //CREATEINDEX id_index ON Sample (Id)

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
