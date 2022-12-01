using OwnDBMS.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSPain.Utilities
{
    public static class FileManager
    {
        public static void CreateTableFile(Table table)
        {

            string _path = "../../../Tables";

            using (StreamWriter sw = File.CreateText($"{_path}/{table.Name}.txt"))
            {
                for (int i = 0; i < table.Cols.Count; i++)
                {
                    sw.Write(table.Cols.ElementAt(i).Value.GetName() + '\t');
                }
            }
        }

        public static void DeleteTableFile(string Name) 
        {
            string _path = "../../../Tables";

            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }
                

            File.Delete($"{_path}/{Name}.txt");
            Console.WriteLine($"\nRemoved Table {Name}\n");
        }

    }
}
