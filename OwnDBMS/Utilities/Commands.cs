﻿using OwnDBMS.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            var cols = new List<ColElement>();           

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
                    cols.Add(col1);
                }
                else
                {
                    ColElement col1 = new ColElement(col[0], type);
                    cols.Add(col1);
                }                    
            }     
            
            tables.Add(new Table(cols, trimmedinput[0]));
        }
        static public void DropTable(string Name)
        {        
            foreach(Table t in tables)
            {
                if (t.Name == Name)
                {
                    tables.Remove(t);
                    break;
                }
            }
            // Do not return null in case of the Table not existing 
            return;
        }
        static public void ListTables()
        {
            if (tables.Count == 0)
            {
                Console.WriteLine("\nThere are no available Tables\n");
                return;
            }
            else
            {
                Console.WriteLine("\nThe Available Tables are:\n");
                foreach(Table t in tables)
                    Console.Write(t.Name + '\t');
                Console.WriteLine("\n");
            }
                
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
                    if (inputcols.Contains(table.Cols[i].GetName()))
                        temptable.Cols.Add(table.Cols[i]);

                List<ColElement> values = new();

                for (int k = 0; k < table.Rows.Count; k++)
                {
                    for (int i = 0; i < temptable.Cols.Count; i++)
                    {
                        for (int u = 0; u < table.Rows[k].Values.Count; u++)
                        {
                            if (table.Rows[k].Values[u].GetName() == temptable.Cols[i].GetName())
                                values.Add(table.Rows[k].Values[u]);
                        }
                    }
                    temptable.Rows.Add(new RowElement(values));
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
            var cols = new List<ColElement>();
            // Id   Name    Date

            // INTO Sample (Id,Name) VALUES (1,“Иван”)
            // Cols = Id = 1; Name = Ivan


            // INTO Sample (Name,Id) VALUES ("Ivan",1)

            for (int i = 0; i < table.Cols.Count; i++)
            {
                for (int k = 0; k < selectedcols.Length; k++)
                {
                    if (selectedcols[k] != table.Cols[i].GetName())
                    {
                        validcol = false;
                    }
                    else
                    {
                        ColElement col = new ColElement(table.Cols[i].GetName(), table.Cols[i].GetType(), values[k]);
                        cols.Add(col);
                        validcol = true;
                        break;
                    }

                }
                if (validcol == false)
                {
                    if (table.Cols[i].DefaultValue != null)
                    {
                        ColElement col = new ColElement(table.Cols[i].GetName(), table.Cols[i].GetType(), table.Cols[i].DefaultValue);
                        cols.Add(col);
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input");
                        return;
                    }                    
                }

            }
            table.Rows.Add(new RowElement(cols));
            Console.WriteLine("\nEntry Added\n");
        }
        static void PrintTable(Table table)
        {
            for (int i = 0; i < table.Cols.Count; i++)
            {
                Console.Write(table.Cols[i].GetName() + '\t');
            }
            Console.WriteLine();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int k = 0; k < table.Rows[i].Values.Count; k++)
                    {

                        Console.Write(table.Rows[i].Values[k].Data + "\t");

                    }
                    Console.WriteLine();
                }
            }
        }
    }

}
