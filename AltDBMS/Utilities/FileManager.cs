using DBMSPain.Structures;
using OwnDBMS.Structures;
using OwnDBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            if (File.Exists($"{_path}/{table.Name}.txt"))
            {
                Console.WriteLine("A Table with that Name already exists.");
                return;
            }

            using (StreamWriter sw = File.CreateText($"{_path}/{table.Name}.txt"))
            {
                for (int i = 0; i < table.Cols.Count; i++)
                {
                    // CreateTable Sample(Id:int, Name:string, BirthDate:date default “01.01.2022”)


                    string defaultvalue = string.Empty;

                    if (table.Cols.ElementAt(i).Value.GetDefaultValue() != null)
                        defaultvalue = $" {table.Cols.ElementAt(i).Value.GetDefaultValue()}";

                    if (table.Cols.ElementAt(i).NextNode == null)
                        sw.Write(table.Cols.ElementAt(i).Value.GetName() + $":{table.Cols.ElementAt(i).Value.GetType()}" + defaultvalue);
                    else
                        sw.Write(table.Cols.ElementAt(i).Value.GetName() + $":{table.Cols.ElementAt(i).Value.GetType()}" + defaultvalue + '\t');
                }
            }
            Console.WriteLine("\nTable Created\n");
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
        public static void GetTableInfo(string Name)
        {
            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            var lines = File.ReadAllLines($"{_path}/{Name}.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
            }

            Console.WriteLine($"The Entries in the Table are {lines.Length - 1}");

            FileInfo info = new FileInfo($"{_path}/{Name}.txt");

            Console.WriteLine($"File Size is : {info.Length} bytes");
        }
        public static void InsertInTable(string Name, string[] selectedcols, string[] selectedvalues)
        {
            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            string[] collines;

            using (StreamReader sr = new StreamReader($"{_path}/{Name}.txt"))
            {
                collines = TableUtils.Split(sr.ReadLine(), '\t');
            }

            var tablecols = new ImpLinkedList<ColElement>();

            for (int i = 0; i < collines.Length; i++)
            {
                var colvalues = TableUtils.Split(collines[i], ':');

                Type type = null;

                switch (colvalues[1])
                {
                    case "System.Int32":
                        type = typeof(int);
                        break;
                    case "System.String":
                        type = typeof(string);
                        break;
                    case "System.DateTime":
                        type = typeof(DateTime);
                        break;
                }
                if (TableUtils.Split(colvalues[1], ' ').Length != 1)
                {
                    var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');
                    {
                        tablecols.AddLast(new ColElement(colvalues[0], type, coldefaultvalue[1]));
                        tablecols.ElementAt(i).Value.SetDefaultData(coldefaultvalue[1]);
                    }
                }
                else
                    tablecols.AddLast(new ColElement(colvalues[0], type));
            }

            bool validcol = true;
            var rowvalues = new ImpLinkedList<ColElement>();

            for (int i = 0; i < tablecols.Count; i++)
            {
                for (int k = 0; k < selectedcols.Length; k++)
                {
                    if (selectedcols[k] != tablecols.ElementAt(i).Value.GetName())
                    {
                        validcol = false;
                    }
                    else
                    {
                        ColElement col = new ColElement(tablecols.ElementAt(i).Value.GetName(), tablecols.ElementAt(i).Value.GetType(), selectedvalues[k]); ;
                        rowvalues.AddLast(col);
                        validcol = true;
                        break;
                    }

                }
                if (validcol == false)
                {
                    if (tablecols.ElementAt(i).Value.DefaultValue != null)
                    {
                        ColElement col = new ColElement(tablecols.ElementAt(i).Value.GetName(), tablecols.ElementAt(i).Value.GetType(), tablecols.ElementAt(i).Value.DefaultValue);
                        rowvalues.AddLast(col);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input");
                        return;
                    }
                }

            }

            using StreamWriter sw = File.AppendText($"{_path}/{Name}.txt");
            {
                sw.WriteLine();
                for (int i = 0; i < rowvalues.Count; i++)
                {
                    if (rowvalues.ElementAt(i).NextNode == null)
                        sw.Write(rowvalues.ElementAt(i).Value.Data);
                    else
                        sw.Write(rowvalues.ElementAt(i).Value.Data + "\t");

                }

                Console.WriteLine("\nEntry Added\n");
            }

        }
        public static void SelectInTable(string Name, string[] inputcols, string[]? conditions = null)
        {
            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }
            if(conditions == null)
            {
                if (inputcols[0] == "*")
                {
                    var lines = File.ReadAllLines($"{_path}/{Name}.txt");

                    for (int i = 0; i < lines.Length; i++)
                        Console.WriteLine(lines[i]);
                }
                else
                {
                    string[] collines;                  
                    using (StreamReader sr = new StreamReader($"{_path}/{Name}.txt"))
                    {
                        collines = TableUtils.Split(sr.ReadLine(), '\t');
                    }

                    for (int i = 0; i < collines.Length; i++)
                    {
                        var colvalues = TableUtils.Split(collines[i], ':');

                        collines[i] = colvalues[0];

                        //Type type = null;

                        //switch (colvalues[1])
                        //{
                        //    case "System.Int32":
                        //        type = typeof(int);
                        //        break;
                        //    case "System.String":
                        //        type = typeof(string);
                        //        break;
                        //    case "System.DateTime":
                        //        type = typeof(DateTime);
                        //        break;
                        //}
                        //if (TableUtils.Split(colvalues[1], ' ').Length != 1)
                        //{
                        //    var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');
                        //    {
                        //        tablecols.AddLast(new ColElement(colvalues[0], type, coldefaultvalue[1]));
                        //        tablecols.ElementAt(i).Value.SetDefaultData(coldefaultvalue[1]);
                        //    }
                        //}
                        //else
                        //    tablecols.AddLast(new ColElement(colvalues[0], type));
                    }

                    int[] indexes = new int[inputcols.Length];

                    for (int i = 0; i < inputcols.Length; i++)
                    {                  
                        if (!TableUtils.Contains(collines, inputcols[i]))
                        {                         
                            Console.WriteLine($"{inputcols[i]} is not available in the given Table");
                            return;
                        }

                        for (int k = 0; k < collines.Length; k++)
                        {
                            if (inputcols[i] == collines[k])
                            {
                                indexes[i] = k;
                                break;
                            }
                        }
                    }

                    var lines = File.ReadAllLines($"{_path}/{Name}.txt");

                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = TableUtils.Split(lines[i], '\t');

                        for (int k = 0; k < indexes.Length; k++)
                        {
                            Console.Write(line[indexes[k]] + '\t'); 
                        }

                        Console.WriteLine();
                    }
                }
            }
            else
            {
                // AND NOT OR
                var tokens = TokenParser.CreateTokens(conditions);
                //Id
                //<>
                //5
                //AND 
                //DateBirth
                //> 
                //“01.01.2000”

                //Id <> 5 AND DateBirth > “01.01.2000”
                var polishtokens = TokenParser.PolishNT(tokens);

                if (inputcols[0] == "*")
                {
                    var lines = File.ReadAllLines($"{_path}/{Name}.txt");                  
                }
            }
            
        }
    }
}
