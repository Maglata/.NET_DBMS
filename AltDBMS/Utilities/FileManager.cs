﻿using DBMSPain.Structures;
using OwnDBMS.Structures;
using OwnDBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBMSPain.Utilities
{
    public static class FileManager
    {
        public static string _tablepath = "../../../Tables";
        public static string _indexespath = "../../../Indexes";
        private static string _wintablepath = "../../../../AltDBMS/Tables";
        private static ImpLinkedList<ulong> rowhash = null;
        private static bool distinctflag = false;
        private static int orderbyflag = 0;
        private static int ordercolindex = -1;
        private static Type ordercoltype = null;
        public static void CreateTableFile(Table table)
        {
            if (File.Exists($"{_tablepath}/{table.Name}.txt"))
            {
                Console.WriteLine("A Table with that Name already exists.");
                return;
            }

            using (StreamWriter sw = File.CreateText($"{_tablepath}/{table.Name}.txt"))
            {
                for (int i = 0; i < table.Cols.Count; i++)
                {
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

            if (!File.Exists($"{_tablepath}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            File.Delete($"{_tablepath}/{Name}.txt");
            Console.WriteLine($"\nRemoved Table {Name}\n");
        }
        public static void CreateIndex(string input)
        {
            //bd_index ON Sample (BirthDate)

            var splitinput = TableUtils.Split(input, ' ');

            if (TableUtils.ToUpper(splitinput[1]) != "ON")
            {
                Console.WriteLine("ON not detected");
                return;
            }

            string filepath = $"{_tablepath}/{splitinput[2]}.txt";

            if (!File.Exists(filepath))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            splitinput[3] = splitinput[3].Trim('(', ')');

            // Get the column information in the Table

            string[] collines;

            using (StreamReader sr = new StreamReader($"{_tablepath}/{splitinput[2]}.txt"))
            {
                collines = TableUtils.Split(sr.ReadLine(), '\t');
            }

            bool foundcol = false;
            int colindex = -1;

            // Checks if the column is present in the selected table and raises a flag if it is
            for (int i = 0; i < collines.Length; i++)
                if (splitinput[3] == TableUtils.Split(collines[i], ':')[0])
                {
                    foundcol = true;
                    colindex = i;
                    break;
                }
            
            if(!foundcol)
            {
                Console.WriteLine("Indexing Error: Column Name not found");
                return;
            }

            // Check if there is an existing index and returns if it does
            if (File.Exists($"{_indexespath}/{splitinput[2]}_{splitinput[3]}_{splitinput[0]}.txt"))
            {
                Console.WriteLine("Indexing Error: The Index already exists");
                return;
            }

            // Saves the index 
            using (StreamWriter sw = File.CreateText($"{_indexespath}/{splitinput[2]}_{splitinput[3]}_{splitinput[0]}.txt"))
            {  
                using (StreamReader sr = new StreamReader($"{_tablepath}/{splitinput[2]}.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        // Writes directly into the new file the index selecte while splitting each line
                        sw.WriteLine(TableUtils.Split(sr.ReadLine(), '\t')[colindex]);
                    }
                }
            }

            Console.WriteLine("Index Created");
        }
        public static void DeleteIndex(string input)
        {
            //bd_index ON Sample (BirthDate)

            var splitinput = TableUtils.Split(input, ' ');

            if (TableUtils.ToUpper(splitinput[1]) != "ON")
            {
                Console.WriteLine("ON not detected");
                return;
            }

            splitinput[3] = splitinput[3].Trim('(', ')');

            if (File.Exists($"{_indexespath}/{splitinput[2]}_{splitinput[3]}_{splitinput[0]}.txt"))
            {
                File.Delete($"{_indexespath}/{splitinput[2]}_{splitinput[3]}_{splitinput[0]}.txt");
                Console.WriteLine("Index Deleted");
                return;
            }

            Console.WriteLine("Index not Found");
        }
        public static int TableFilesCount()
        {
            return Directory.GetFiles(_wintablepath).Length;
        }
        public static void GetTableNames()
        {
            if (FileManager.TableFilesCount() == 0)
            {
                Console.WriteLine("\nThere are no available Tables\n");
                return;
            }

            Console.WriteLine("\nThe Available Tables are:\n");

            var files = Directory.GetFiles(_wintablepath);

            for (int i = 0; i < files.Length; i++)
                Console.WriteLine(Path.GetFileNameWithoutExtension(files[i]));
            Console.WriteLine();
        }
        public static void GetTableInfo(string Name)
        {
            if (!File.Exists($"{_tablepath}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            var lines = File.ReadAllLines($"{_tablepath}/{Name}.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
            }

            Console.WriteLine($"The Entries in the Table are {lines.Length - 1}");

            FileInfo info = new FileInfo($"{_tablepath}/{Name}.txt");

            Console.WriteLine($"File Size is : {info.Length} bytes");
        }
        public static void InsertInTable(string Name, string[] selectedcols, string[] selectedvalues)
        {
            if (!File.Exists($"{_tablepath}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            string[] collines;

            using (StreamReader sr = new StreamReader($"{_tablepath}/{Name}.txt"))
            {
                collines = TableUtils.Split(sr.ReadLine(), '\t');
            }

            var tablecols = new ImpLinkedList<ColElement>();

            for (int i = 0; i < collines.Length; i++)
            {
                var colvalues = TableUtils.Split(collines[i], ':');
                var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');

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
                if (coldefaultvalue.Length != 1)
                {
                    tablecols.AddLast(new ColElement(colvalues[0], type, coldefaultvalue[1]));
                    tablecols.ElementAt(i).Value.SetDefaultData(coldefaultvalue[1]);
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

            using (StreamWriter sw = File.AppendText($"{_tablepath}/{Name}.txt"))
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

            UpdateTableIndexes(Name);

        }
        public static void SelectInTable(string Name, string[] inputvalues, string[]? conditions = null)
        {
            string filepath = $"{_wintablepath}/{Name}.txt";

            if (!File.Exists(filepath))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            distinctflag = false;
            // flag = 0 -> No order by
            // flag = 1 -> order by ASC
            // flag = -1 -> order by DESC
            orderbyflag = 0;
            string[] inputcols;
            string ordercolname = string.Empty;
            ordercoltype = null;
            // Order By Check
            if (conditions != null)
            {
                if (TableUtils.ToUpper(conditions[conditions.Length - 3]) == "ORDER" || TableUtils.ToUpper(conditions[conditions.Length - 3]) == "BY")
                {
                    orderbyflag = 1;
                    // 0 = ASC(Default) || 1 = DESC             
                    // Checks if ASC OR DESC is available
                    if (TableUtils.ToUpper(conditions[conditions.Length - 1]) != "DESC" && TableUtils.ToUpper(conditions[conditions.Length - 1]) != "ASC")
                    {
                        ordercolname = conditions[conditions.Length - 1];
                        conditions = TableUtils.Slice(conditions, 0, conditions.Length - 3);                       
                        if (conditions.Length == 0)
                            conditions = null;
                    }
                    else
                    {
                        ordercolname = conditions[conditions.Length - 2];
                        if (TableUtils.ToUpper(conditions[conditions.Length - 1]) == "DESC")
                            orderbyflag = -1;
                        conditions = TableUtils.Slice(conditions, 0, conditions.Length - 4);
                        if (conditions.Length == 0)
                            conditions = null;
                    }
                }
            } 
            
            // Distinct Check
            if (TableUtils.ToUpper(inputvalues[0]) == "DISTINCT")
            {
                distinctflag = true;
                inputcols = new string[inputvalues.Length - 1];
                for (int i = 0; i < inputcols.Length; i++)
                {
                    inputcols[i] = inputvalues[i+1];
                }
            }
            else
            {
                inputcols = inputvalues;
            }

            string[] collines;
            using (StreamReader sr = new StreamReader(filepath))
            {
                collines = TableUtils.Split(sr.ReadLine(), '\t');
            }

            string[] colnames = new string[collines.Length];
            string[] coltypes = new string[collines.Length];
            for (int i = 0; i < collines.Length; i++)
            {
                var colvalues = TableUtils.Split(collines[i], ':');              

                colnames[i] = colvalues[0];
                coltypes[i] = TableUtils.Split(colvalues[1], ' ')[0];
            }

            // Check for Order by Col Index
            ordercolindex = -1;
            if (!TableUtils.Contains(colnames, ordercolname) && orderbyflag != 0)
            {
                Console.WriteLine($"{ordercolname} is not available in the given Table");
                return;
            }
            else
            {
                for (int k = 0; k < colnames.Length; k++)
                {
                    if (ordercolname == colnames[k])
                    {
                        ordercolindex = k;
                        switch (coltypes[k])
                        {
                            case "System.Int32":
                                ordercoltype = typeof(int);
                                break;
                            case "System.String":
                                ordercoltype = typeof(string);
                                break;
                            case "System.DateTime":
                                ordercoltype = typeof(DateTime);
                                break;
                        }
                        break;
                    }
                }
            }  
            
            int[] indexes;
            if (inputcols[0] == "*")
            {
                indexes = new int[colnames.Length];

                for (int i = 0; i < colnames.Length; i++)
                    indexes[i] = i;
            }
            else
            {
                indexes = new int[inputcols.Length];

                for (int i = 0; i < inputcols.Length; i++)
                {
                    if (!TableUtils.Contains(colnames, inputcols[i]))
                    {
                        Console.WriteLine($"{inputcols[i]} is not available in the given Table");
                        return;
                    }

                    for (int k = 0; k < colnames.Length; k++)
                    {
                        if (inputcols[i] == colnames[k])
                        {
                            indexes[i] = k;
                            break;
                        }
                    }
                }
            }
            rowhash = null;

            // Select..
            if (conditions == null)
            {
                Select(filepath, indexes, inputcols[0]);
            }
            else // Select... Where
            {

                if(!ContainIndexes(Name, conditions))
                {

                }

                SelectWhere(filepath, collines, indexes, inputcols[0], conditions);
            }


            
        } 
        private static bool ContainIndexes(string Name, string[] conditions)
        {
            var tokens = TokenParser.CreateTokens(conditions);
            var polishtokens = TokenParser.PolishNT(tokens);

            var filenames = Directory.GetFiles(_indexespath);
            List<string> indexes = new List<string>();

            for (int i = 0; i < polishtokens.Count; i++)
            {
                if (polishtokens[i].type == Token.Type.CONDITION)
                {
                    var colname = TableUtils.Split(polishtokens[i].Value, ' ')[0];

                    for (int k = 0; k < filenames.Length; k++)
                    {
                        // using the Path command we get the full file name without extensions and paths
                        var splitname = TableUtils.Split(Path.GetFileNameWithoutExtension(filenames[k]), '_', 3);
                        // Checking if there is a file that matches both the name and the type
                        if (splitname[0] == Name)
                            if (splitname[1] == colname)
                            {
                                indexes.Add(Path.GetFileName(filenames[k]));
                                break;
                            }
                    }

                }
            }

            if (indexes.Count != 0)
            {
                SelectWhereIndex(Name, indexes, polishtokens);
                return true;
            }
            else
                return false;
        }
        private static void SelectWhereIndex(string Name,List<string> indexes, List<Token> polishtokens)
        {
            List<int> rowindx = new List<int>();
            int rowcounter = 0;
            for (int i = 0; i < indexes.Count; i++)
            {
                using(StreamReader sr = new StreamReader(_indexespath + indexes[i]))
                {
                    sr.ReadLine();
                    while(!sr.EndOfStream) 
                    {
                        rowcounter++;
                        //if(CheckExpressionIndex(sr.ReadLine(), polishtokens))
                        //    rowindx.Add(rowcounter);
                    }
                }
            }

        }
        //private static bool CheckExpressionIndex(string rowvalue,List<Token> polishtokens)
        //{
        //    // Make a new List since the original one is sent by reference
        //    var resulttokens = new List<Token>();

        //    for (int i = 0; i < polishtokens.Count; i++)
        //        resulttokens.Add(polishtokens[i]);


        //    for (int i = 0; i < resulttokens.Count; i++)
        //    {
        //        if (resulttokens[i].type == Token.Type.CONDITION)
        //        {
        //            var condition = TableUtils.Split(resulttokens[i].Value, ' ');

        //            int index = 0;
        //            for (int k = 0; k < colnames.Length; k++)
        //            {
        //                if (colnames[k] == condition[0])
        //                {
        //                    index = k;
        //                    break;
        //                }
        //            }
        //            var value = rowvalues[index];
        //            bool flag = false;
        //            switch (condition[1])
        //            {
        //                case "<>": // !=
        //                    {
        //                        flag = value != condition[2];
        //                    }
        //                    break;
        //                case ">":
        //                    {
        //                        if (tablecols.ElementAt(index).Value.GetType() == typeof(System.DateTime))
        //                        {
        //                            // To Do : Trim
        //                            value = value.Trim('"');
        //                            condition[2] = condition[2].Trim('"');
        //                            flag = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(condition[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //                        }
        //                        else
        //                            flag = int.Parse(value) > int.Parse(condition[2]);
        //                    }
        //                    break;
        //                case "<":
        //                    {
        //                        if (tablecols.ElementAt(index).Value.GetType() == typeof(DateTime))
        //                        {
        //                            value = value.Trim('"');
        //                            condition[2] = condition[2].Trim('"');
        //                            flag = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(condition[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //                        }
        //                        else
        //                            flag = int.Parse(value) < int.Parse(condition[2]);
        //                    }
        //                    break;
        //                case "=":
        //                    {
        //                        flag = value == condition[2];
        //                    }
        //                    break;
        //                default: Console.WriteLine("Invalid Expression"); break;
        //            }
        //            resulttokens[i].Value = flag.ToString();
        //        }
        //    }
        //    var tree = TableUtils.CreateTree(resulttokens);

        //    var result = Evaluate(tree);

        //    return result;
        //}
        private static void Distinct(string row, int rowindex, string[]? rowvalues, int[] indexes, string inputcol,ref ImpLinkedList<ulong> rowhash, ref ImpLinkedList<string> rows, ref ImpLinkedList<int> selectedindexes)
        {          
            if(rows != null)
            {
                if (!TableUtils.Contains(rowhash, UniqueHash(row)))
                {
                    rowhash?.AddLast(UniqueHash(row));
                    string rowline = string.Empty;
                    if (inputcol == "*")
                    {
                        for (int i = 0; i < rowvalues.Length; i++)
                        {
                            rowline += rowvalues[i] + "\t";                        
                        }
                            
                        rows.AddLast(rowline);
                        selectedindexes.AddLast(rowindex);
                    }
                    else
                    {
                        for (int k = 0; k < indexes.Length; k++)
                        {
                            rowline += rowvalues[indexes[k]] + '\t';
                        }
                            
                        rows.AddLast(rowline);
                        selectedindexes.AddLast(rowindex);
                    }
                }
                else
                {

                }
            }
            else
            {
                if (!TableUtils.Contains(rowhash, UniqueHash(row)))
                {
                    rowhash?.AddLast(UniqueHash(row));

                    if (inputcol == "*")
                        for (int i = 0; i < rowvalues.Length; i++)
                            Console.Write(rowvalues[i] + "\t");
                    else
                        for (int k = 0; k < indexes.Length; k++)
                            Console.Write(rowvalues[indexes[k]] + '\t');
                    Console.WriteLine();
                }
            }    
        }       
        private static void Select(string filepath, int[] indexes,string inputcol)
        {
            var lines = File.ReadAllLines(filepath);

            // Checks if Order by has Been Selected
            if (orderbyflag != 0)
            {
                ImpLinkedList<int> selectedindexes = new ImpLinkedList<int>();
                ImpLinkedList<string> rows = new ImpLinkedList<string>();

                if (distinctflag)
                {
                    rowhash = new ImpLinkedList<ulong>();
                    for (int i = 1; i < lines.Length; i++)
                    {
                       Distinct(lines[i],i-1, TableUtils.Split(lines[i], '\t'), indexes, inputcol, ref rowhash, ref rows,ref selectedindexes);
                    }
                }
                else
                {
                    if (inputcol == "*")
                    {
                        for (int i = 1; i < lines.Length; i++)
                            rows.AddLast(lines[i]);
                    }
                    else
                    {

                        for (int i = 1; i < lines.Length; i++)
                        {
                            var line = TableUtils.Split(lines[i], '\t');
                            string rowline = string.Empty;

                            for (int k = 0; k < indexes.Length; k++)
                                rowline += line[indexes[k]] + '\t';

                            rows.AddLast(rowline);
                        }
                    }

                }

                TableUtils.Sort(ordercoltype, TableUtils.Slice(lines,1,lines.Length),rows, ordercolindex, selectedindexes, orderbyflag);

                for (int i = 0; i < rows.Count; i++)
                {
                    Console.WriteLine(rows.ElementAt(i).Value);
                }

            }
            else
            {
                // Checks if Distinct has been Selected
                if (distinctflag)
                {
                    ImpLinkedList<string> a = null;
                    ImpLinkedList<int> b = null;
                    rowhash = new ImpLinkedList<ulong>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        Distinct(lines[i],i,TableUtils.Split(lines[i], '\t'), indexes, inputcol, ref rowhash, ref a, ref b);
                    }
                }
                else
                {
                    if (inputcol == "*")
                    {

                        for (int i = 0; i < lines.Length; i++)
                            Console.WriteLine(lines[i]);
                    }
                    else
                    {

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
            }     
        }
        private static void SelectWhere(string filepath, string[] collines,int[] indexes,string inputcol, string[] conditions)
        {
            var tablecols = new ImpLinkedList<ColElement>();
            ImpLinkedList<string> rows = new ImpLinkedList<string>();
            ImpLinkedList<int> selectedindexes = new ImpLinkedList<int>();

            for (int i = 0; i < collines.Length; i++)
            {
                var colvalues = TableUtils.Split(collines[i], ':');
                var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');
                Type type = null;

                switch (coldefaultvalue[0])
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
                if (coldefaultvalue.Length != 1)
                {
                    tablecols.AddLast(new ColElement(colvalues[0], type, coldefaultvalue[1]));
                    tablecols.ElementAt(i).Value.SetDefaultData(coldefaultvalue[1]);
                }
                else
                    tablecols.AddLast(new ColElement(colvalues[0], type));
            }

            using (StreamReader sr = new StreamReader(filepath))
            {
                sr.ReadLine();

                rowhash = new ImpLinkedList<ulong>();
                var rowindex = 0;
                var selectedindex = 0;
                while (!sr.EndOfStream)
                {
                    var tokens = TokenParser.CreateTokens(conditions);
                    var polishtokens = TokenParser.PolishNT(tokens);
                    var row = sr.ReadLine();
                    var rowvalues = TableUtils.Split(row, '\t');
                    if (CheckExpression(rowvalues, polishtokens, tablecols))
                    {
                        if (orderbyflag != 0)
                        {
                            if (distinctflag == true)
                            {
                                Distinct(row, rowindex, rowvalues, indexes, inputcol, ref rowhash, ref rows, ref selectedindexes);
                            }
                            else
                            {
                                string rowline = string.Empty;

                                if (inputcol == "*")
                                {
                                    for (int i = 0; i < rowvalues.Length; i++)
                                        rowline += rowvalues[i] + "\t";
                                    rows.AddLast(rowline);
                                    selectedindexes.AddLast(selectedindex);
                                    selectedindex++;
                                }
                                else
                                {
                                    for (int k = 0; k < indexes.Length; k++)
                                        rowline += rowvalues[indexes[k]] + '\t';
                                    rows.AddLast(rowline);
                                    selectedindexes.AddLast(selectedindex);
                                    selectedindex++;
                                }
                            }
                        }
                        else
                        {

                            if (distinctflag == true)
                            {
                                ImpLinkedList<string> a = null;
                                ImpLinkedList<int> b = null;

                                Distinct(row, rowindex, rowvalues, indexes, inputcol, ref rowhash, ref a, ref b);
                            }
                            else
                            {
                                if (inputcol == "*")
                                    for (int i = 0; i < rowvalues.Length; i++)
                                        Console.Write(rowvalues[i] + "\t");
                                else
                                    for (int k = 0; k < indexes.Length; k++)
                                        Console.Write(rowvalues[indexes[k]] + '\t');
                                Console.WriteLine();
                            }
                        }
                    }
                    else
                        selectedindex++;
                    rowindex++;
                }
            }

            if (orderbyflag != 0)
            {                        
                TableUtils.Sort(ordercoltype, TableUtils.Slice(File.ReadAllLines(filepath), 1, File.ReadAllLines(filepath).Length), rows, ordercolindex, selectedindexes, orderbyflag);

                for (int i = 0; i < rows.Count; i++)
                {
                    Console.WriteLine(rows.ElementAt(i).Value);
                }
            }

        }
        public static void DeleteInTable(string Name, string? expression)
        {
            if (!File.Exists($"{_tablepath}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            if (expression == null)
                Console.WriteLine("No Conditions provided");
            else
            {
                string[] collines;
                using (StreamReader sr = new StreamReader($"{_tablepath}/{Name}.txt"))
                {
                    collines = TableUtils.Split(sr.ReadLine(), '\t');
                }

                var tablecols = new ImpLinkedList<ColElement>();

                for (int i = 0; i < collines.Length; i++)
                {
                    var colvalues = TableUtils.Split(collines[i], ':');
                    var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');
                    Type type = null;

                    switch (coldefaultvalue[0])
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
                    if (coldefaultvalue.Length != 1)
                    {
                        tablecols.AddLast(new ColElement(colvalues[0], type, coldefaultvalue[1]));
                        tablecols.ElementAt(i).Value.SetDefaultData(coldefaultvalue[1]);
                    }
                    else
                        tablecols.AddLast(new ColElement(colvalues[0], type));
                }

                var conditions = TableUtils.Split(expression, ' ');

                int deletednumber = 0;
                using (StreamReader sr = new StreamReader($"{_tablepath}/{Name}.txt"))
                {                   
                    using(StreamWriter sw = new StreamWriter($"{_tablepath}/{Name}temp.txt"))
                    {
                        sw.WriteLine(sr.ReadLine());
                        while (!sr.EndOfStream)
                        {
                            var tokens = TokenParser.CreateTokens(conditions);
                            var polishtokens = TokenParser.PolishNT(tokens);

                            var row = sr.ReadLine();
                            var rowvalues = TableUtils.Split(row, '\t');
                            if (CheckExpression(rowvalues, polishtokens, tablecols))
                            {
                                deletednumber++;
                            }
                            else
                            {
                                sw.WriteLine(row);
                            }
                        }                        
                    }                   
                }
                File.Replace($"{_tablepath}/{Name}temp.txt", $"{_tablepath}/{Name}.txt", null);
                Console.WriteLine($"Deleted {deletednumber} entries");
            }

            UpdateTableIndexes(Name);
        }
        private static bool CheckExpression(string[] rowvalues, List<Token> tokens, ImpLinkedList<ColElement> tablecols)
        {
            string[] colnames = new string[tablecols.Count];

            // Make a new List since the original one is sent by reference
            var resulttokens = new List<Token>();

            for (int i = 0; i < tokens.Count; i++)
                resulttokens.Add(tokens[i]);
           
            for (int i = 0; i < tablecols.Count; i++)
                colnames[i] = tablecols.ElementAt(i).Value.GetName();
            for (int i = 0; i < resulttokens.Count; i++)
            {
                if (resulttokens[i].type == Token.Type.CONDITION)
                {
                    var condition = TableUtils.Split(resulttokens[i].Value, ' ');

                    int index = 0;
                    for (int k = 0; k < colnames.Length; k++)
                    {
                        if (colnames[k] == condition[0])
                        {
                            index = k;
                            break;
                        }                          
                    }
                    var value = rowvalues[index];
                    bool flag = false;
                    switch (condition[1])
                    {
                        case "<>": // !=
                            {
                                flag = value != condition[2];                                
                            }
                            break;
                        case ">": 
                            {
                                if (tablecols.ElementAt(index).Value.GetType() == typeof(System.DateTime))
                                {
                                    // To Do : Trim
                                    value = value.Trim('"');
                                    condition[2] = condition[2].Trim('"');
                                    flag = DateTime.ParseExact(value,"dd.MM.yyyy",CultureInfo.InvariantCulture) > DateTime.ParseExact(condition[2],"dd.MM.yyyy",CultureInfo.InvariantCulture);
                                }
                                else
                                    flag = int.Parse(value) > int.Parse(condition[2]);
                            }
                            break;
                        case "<": 
                            {
                                if (tablecols.ElementAt(index).Value.GetType() == typeof(DateTime))
                                {
                                    value = value.Trim('"');
                                    condition[2] = condition[2].Trim('"');
                                    flag = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(condition[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                    flag = int.Parse(value) < int.Parse(condition[2]);
                            } 
                            break;
                        case "=": 
                            {
                                flag = value == condition[2];
                            }
                            break;
                        default: Console.WriteLine("Invalid Expression"); break;
                    }
                    resulttokens[i].Value = flag.ToString();
                }
            }
            var tree = TableUtils.CreateTree(resulttokens);

            var result = Evaluate(tree);

            return result;
        }
        public static bool Evaluate(Node root)
        {
            if (root == null)
            {
                return false;
            }

            if (root.GetLeft() == null && root.GetRight() == null)
            {
                return bool.Parse(root.GetValue()) == true;
            }

            bool left = Evaluate(root.GetLeft());
            bool right = Evaluate(root.GetRight());

            switch (root.GetValue())
            {
                case "AND": return left && right;
                case "OR": return left || right;
                case "NOT": return !left;
                case "True": return true;
                case "False": return false;
            }

            return false;
        }
        static ulong UniqueHash(string s)
        {
            // Initialize the hash value to a prime number
            ulong hash = 17;

            // For each character in the string, update the hash value
            // using the following formula:
            // hash = (hash * 31) + c
            // The hash value is multiplied by the index of the character
            // in the string, so that the order of the characters matters
            // The hash value is also XORed with the Unicode code of the
            // character, to make the hash value more difficult to predict
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                hash = (hash * 31 * ((ulong)i + 1)) ^ c;
            }

            // Return the final hash value
            return hash;
        }
        public static void UpdateTableIndexes(string Name)
        {
            var filenames = Directory.GetFiles(_indexespath);

            for (int i = 0; i < filenames.Length; i++)
            {
                // using the Path command we get the full file name without extensions and paths
                var splitname = TableUtils.Split(Path.GetFileNameWithoutExtension(filenames[i]), '_', 3);

                // Sample BirthDate bd_index

                if (splitname[0] == Name) 
                {
                    string[] collines;
                    using (StreamReader sr = new StreamReader($"{_tablepath}/{Name}.txt"))
                    {
                        collines = TableUtils.Split(sr.ReadLine(), '\t');
                    }

                    for (int k = 0; k < collines.Length; k++)
                        if (splitname[1] == TableUtils.Split(collines[k], ':')[0])
                        {
                            File.Delete(filenames[i]);
                            // Saves the index 
                            using (StreamWriter sw = File.CreateText($"{_indexespath}/{Name}_{splitname[1]}_{splitname[2]}.txt"))
                            {
                                using (StreamReader sr = new StreamReader($"{_tablepath}/{Name}.txt"))
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        // Writes directly into the new file the index selecte while splitting each line
                                        sw.WriteLine(TableUtils.Split(sr.ReadLine(), '\t')[k]);
                                    }
                                }
                            }
                            break;
                        }                       
                }
            }
        }
    }
}
