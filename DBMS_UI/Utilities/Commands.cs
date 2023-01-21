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
        static public void CreateTable(string input)
        {
            //Създава нова таблица по подадено име и списък от имената и типовете на съставящите я колони
            //Трябва да има възможност за задаване на стойности по подразбиране или автоматично генерирани стойности\

            var trimmedinput = TableUtils.Split(input, new char[] {'(', ')'});

            var colattributes = TableUtils.Split(trimmedinput[1], ',');

            var cols = new ImpLinkedList<ColElement>();           

            for (int i = 0; i < colattributes.Length; i++)
            {
                //colattributes[i] = colattributes[i].TrimStart();
                colattributes[i] = TableUtils.TrimStart(colattributes[i]);

                var col = TableUtils.Split(colattributes[i], new char[] { ':', ' '});
                Type type;

                switch (TableUtils.ToUpper(col[1]))
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
                if (col.Length > 2 && TableUtils.ToUpper(col[2]) == "DEFAULT")
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
        static public string[] Select(string input)
        {         
            int index = 0;
            var splitinput = TableUtils.Split(input, ' ');
            int flag = 1;
            for (int i = 0; i < splitinput.Length; i++)
            {
                if (flag == 1)
                {
                    if (TableUtils.ToUpper(splitinput[i]) == "FROM")
                    {
                        index = i;
                        flag++;
                        continue;
                    }
                    //splitinput[i] = splitinput[i].TrimEnd(',');
                    splitinput[i] = TableUtils.TrimEnd(splitinput[i],',');
                }
                else
                {
                    if (TableUtils.ToUpper(splitinput[i]) == "WHERE")
                    {
                        flag++;
                        break;
                    }

                    if(TableUtils.ToUpper(splitinput[i]) == "ORDER")
                    {
                        flag = 4;
                        break;
                    }
                }
            }
           
            //Name, DateBirth
            var inputcols = TableUtils.Slice(splitinput, 0, index);

            string[]? conditions = null;

            if (flag == 3)
            {
                //Id <> 5 AND DateBirth > “01.01.2000”
                conditions = TableUtils.Slice(splitinput, index + 3);
            }

            if (flag == 4)
            {
                conditions = TableUtils.Slice(splitinput, index + 2);
            }

             return FileManager.SelectInTable(splitinput[index+1], inputcols, conditions);                 
        }
        static public string Insert(string input)
        {
            // INTO Sample (Id,Name) VALUES (1,“Иван”)
            var splitinput = TableUtils.Split(input, ' ');
            
            if (TableUtils.ToUpper(splitinput[0]) != "INTO")
            {
                MessageBox.Show("Invalid Input, Into expected");
                return null;
            }

            if (TableUtils.ToUpper(splitinput[3]) != "VALUES")
            {
                MessageBox.Show("Invalid Input, Values expected");
                return null;
            }
            //var selectedcols = TableUtils.Split(splitinput[2].TrimStart('(').TrimEnd(')'), ',');
            //var values = TableUtils.Split(splitinput[4].TrimStart('(').TrimEnd(')'), ',');
            var trimmedsplitinput1 = TableUtils.TrimEnd(TableUtils.TrimStart(splitinput[2], '('),')');
            var selectedcols = TableUtils.Split(trimmedsplitinput1, ',');

            var trimmedsplitinput2 = TableUtils.TrimEnd(TableUtils.TrimStart(splitinput[4], '('), ')');         
            var values = TableUtils.Split(trimmedsplitinput2, ',');

            if (selectedcols.Length != values.Length)
            {
                MessageBox.Show("Incorrect amount of inputs");
                return null;
            }

            return FileManager.InsertInTable(splitinput[1], selectedcols, values);        
        }       
        static public string Delete(string input)
        {
            // FROM table_name WHERE condition
            var splitinput = TableUtils.Split(input,' ',4);

            if (TableUtils.ToUpper(splitinput[0]) != "FROM")
            {
                Console.WriteLine("FROM not found");
                return null;
            }
            if(TableUtils.ToUpper(splitinput[2]) != "WHERE")
            {
                Console.WriteLine("WHERE not found");
                return null;
            }
            return FileManager.DeleteInTable(splitinput[1], splitinput[3]);
        }

    }

}
