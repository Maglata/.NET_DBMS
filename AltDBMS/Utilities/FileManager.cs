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
        private static string _path = "../../../Tables";
        public static void CreateTableFile(Table table)
        {
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

            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }
                

            File.Delete($"{_path}/{Name}.txt");
            Console.WriteLine($"\nRemoved Table {Name}\n");
        }

        public static int TableFilesCount() 
        {
            return Directory.GetFiles(_path).Length;
        }

        public static void GetTableNames()
        {
            if (FileManager.TableFilesCount() == 0)
            {
                Console.WriteLine("\nThere are no available Tables\n");
                return;
            }

            Console.WriteLine("\nThe Available Tables are:\n");

            var files = Directory.GetFiles(_path);

            for (int i = 0; i < files.Length; i++)
                Console.WriteLine(Path.GetFileNameWithoutExtension(files[i]));
            Console.WriteLine();
        }

    }
}
