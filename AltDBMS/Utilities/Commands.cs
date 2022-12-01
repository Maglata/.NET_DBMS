using OwnDBMS.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DBMSPain.Utilities;

namespace OwnDBMS.Utilities
{
    public class Commands
    {
        private static List<Table> tables = new List<Table>();

        static public void CreateTable(string input)
        {
            //Създава нова таблица по подадено име и списък от имената и типовете на съставящите я колони
            //Трябва да има възможност за задаване на стойности по подразбиране или автоматично генерирани стойности\

            var trimmedinput = input.Split('(', ')');

            var colattributes = trimmedinput[1].Split(',');

            var cols = new ObjectLinkedList<ColElement>();           

            for (int i = 0; i < colattributes.Length; i++)
            {            
                // To Do: TrimStart()
                colattributes[i] = colattributes[i].TrimStart();

                var col = colattributes[i].Split(':', ' ');
                Type type;

                switch (col[1].ToUpper())
                {
                    case "INT":
                        type = typeof(int);
                        break;
                    case "STRING":
                        type = typeof(string);
                        break;
                    case "DATE":
                        type = typeof(DateTime);
                        break;
                        // Check for a null return 
                    default:
                        return;
                }
                // To Do: Fool proof for typos 
                if (col.Length > 2 && col[2].ToUpper() == "DEFAULT")
                {
                    ColElement col1 = new ColElement(col[0], type);
                    col1.SetDefaultData(col[3]);
                    cols.AddLast(col1);
                }
                else
                {
                    ColElement col1 = new ColElement(col[0], type);
                    cols.AddLast(col1);
                }                    
            }

            FileManager.CreateTableFile(new Table(cols, trimmedinput[0]));
        }     
        static public void TableInfo(string input)
        {
            Table table = null;
            // Should Display a Table based entirely on the Select Clause
            foreach (Table t in tables)
            {
                if (t.Name == input)
                {
                    table = t;
                    break;
                }

            }
            if (table == null)
            {
                Console.WriteLine("Table doesn't exit");
                return;
            }

            PrintTable(table);
            Console.WriteLine($"\nEntries in the table:{table.Rows.Count}\n");

        }
        static public void Select(string input)
        {
            //Select
            //Name, DateBirth FROM Sample WHERE Id <> 5 AND DateBirth > “01.01.2000”            
            Table table = null;
            Table temptable = new Table();
            int index = 0;
            var splitinput = input.Split(' ');
            int flag = 1;

            for (int i = 0; i < splitinput.Length; i++)
            {
                if (flag == 1)
                {
                    if (splitinput[i].ToUpper() == "FROM")
                    {
                        index = i;
                        flag++;
                        continue;
                    }
                    splitinput[i] = splitinput[i].TrimEnd(',');                  
                }
                else
                {
                    if (splitinput[i].ToUpper() == "WHERE")
                    {
                        flag++;
                        break;
                    }
                       
                }           
            }
            foreach (Table t in tables)
            {
                if (t.Name == splitinput[index + 1])
                {
                    table = t;
                    break;
                }
            }
            if (table == null)
            {
                Console.WriteLine("Table doesn't exit");
                return;
            }

            //Name, DateBirth
            var inputcols = TableUtils.Slice(splitinput, 0, index);

            if (inputcols[0] == "*")
                temptable = table;
            else
            {
                for (int i = 0; i < inputcols.Length; i++)
                    if (!table.GetColNames().Contains(inputcols[i]))
                    {
                        Console.WriteLine($"{inputcols[i]} is not available in the given Table");
                        return;
                    }

                for (int i = 0; i < table.Cols.Count; i++)
                    if (inputcols.Contains(table.Cols.ElementAt(i).Value.GetName()))
                        temptable.Cols.AddLast(table.Cols.ElementAt(i).Value);

                ObjectLinkedList<ColElement> values = new();

                for (int k = 0; k < table.Rows.Count; k++)
                {
                    for (int i = 0; i < temptable.Cols.Count; i++)
                    {
                        for (int u = 0; u < table.Rows.ElementAt(k).Value.Values.Count; u++)
                        {
                            if (table.Rows.ElementAt(k).Value.Values.ElementAt(u).Value.GetName() == temptable.Cols.ElementAt(i).Value.GetName())
                                values.AddLast(table.Rows.ElementAt(k).Value.Values.ElementAt(u).Value);
                        }
                    }
                    temptable.Rows.AddLast(new RowElement(values));
                    values = new();
                }
            }                                                  
            PrintTable(temptable);

            if (flag == 3)
            {
                Console.WriteLine("Error. WHERE not found");
                //Id <> 5 AND DateBirth > “01.01.2000”
                var conditions = TableUtils.Slice(splitinput, index + 3);
                return;
            }
        }
        static public void Insert(string input)
        {
            // INTO Sample (Id,Name) VALUES (1,“Иван”)
            var splitinput = input.Split(' ');

            Table table = null;
            
            if (splitinput[0].ToUpper() != "INTO")
            {
                Console.WriteLine("Invalid Input, Into expected");
                return;
            }

            if (splitinput[3].ToUpper() != "VALUES")
            {
                Console.WriteLine("Invalid Input, Values expected");
                return;
            }

            foreach (Table t in tables)
            {
                if (t.Name == splitinput[1])
                {
                    table = t;
                    break;
                }

            }
            if (table == null)
            {
                Console.WriteLine("Table doesn't exit");
                return;
            }

            // [2] = cols [4] - values
            var selectedcols = splitinput[2].TrimStart('(').TrimEnd(')').Split(',');
            var values = splitinput[4].TrimStart('(').TrimEnd(')').Split(',');

            if(selectedcols.Length != values.Length)
            {
                Console.WriteLine("Incorrect amount of inputs");
                return;
            }     
            bool validcol = true;
            var cols = new ObjectLinkedList<ColElement>();
            // Id   Name    Date

            // INTO Sample (Id,Name) VALUES (1,“Иван”)
            // Cols = Id = 1; Name = Ivan


            // INTO Sample (Name,Id) VALUES ("Ivan",1)

            for (int i = 0; i < table.Cols.Count; i++)
            {
                for (int k = 0; k < selectedcols.Length; k++)
                {
                    if (selectedcols[k] != table.Cols.ElementAt(i).Value.GetName())
                    {
                        validcol = false;
                    }
                    else
                    {
                        ColElement col = new ColElement(table.Cols.ElementAt(i).Value.GetName(), table.Cols.ElementAt(i).Value.GetType(), values[k]);
                        cols.AddLast(col);
                        validcol = true;
                        break;
                    }

                }
                if (validcol == false)
                {
                    if (table.Cols.ElementAt(i).Value.DefaultValue != null)
                    {
                        ColElement col = new ColElement(table.Cols.ElementAt(i).Value.GetName(), table.Cols.ElementAt(i).Value.GetType(), table.Cols.ElementAt(i).Value.DefaultValue);
                        cols.AddLast(col);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input");
                        return;
                    }                    
                }

            }
            table.Rows.AddLast(new RowElement(cols));
            Console.WriteLine("\nEntry Added\n");
        }
        static void PrintTable(Table table)
        {
            for (int i = 0; i < table.Cols.Count; i++)
            {
                Console.Write(table.Cols.ElementAt(i).Value.GetName() + '\t');
            }
            Console.WriteLine();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int k = 0; k < table.Rows.ElementAt(i).Value.Values.Count; k++)
                    {

                        Console.Write(table.Rows.ElementAt(i).Value.Values.ElementAt(k).Value.Data + "\t");

                    }
                    Console.WriteLine();
                }
            }
        }
    }

}
