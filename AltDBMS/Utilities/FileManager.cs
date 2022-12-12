using DBMSPain.Structures;
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
        public static void SelectInTable(string Name, string[] inputvalues, string[]? conditions = null)
        {
            string filepath = $"{_path}/{Name}.txt";

            if (!File.Exists(filepath))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            bool distinctflag = false;
            // flag = 0 -> No order by
            // flag = 1 -> order by ASC
            // flag = -1 -> order by DESC
            int orderbyflag = 0;
            string[] inputcols;
            string ordercolname = string.Empty;
            Type ordercoltype = null;
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
            int ordercolindex = -1;
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
            ImpLinkedList<ulong> rowhash = null;

            // Select..
            if (conditions == null)
            {
                Select(filepath, indexes, inputcols[0], rowhash, distinctflag, orderbyflag, ordercolindex, ordercoltype);              
            }
            else // Select... Where
            {
                SelectWhere(filepath, collines, indexes, inputcols[0], conditions, rowhash, distinctflag,orderbyflag,ordercolindex,ordercoltype);              
            }
            
        } 
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
        private static void Select(string filepath, int[] indexes,string inputcol, ImpLinkedList<ulong> rowhash, bool distinctflag, int orderbyflag, int ordercolindex, Type ordercoltype)
        {
            var lines = File.ReadAllLines(filepath);
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
        private static void SelectWhere(string filepath, string[] collines,int[] indexes,string inputcol, string[] conditions, ImpLinkedList<ulong> rowhash,bool distinctflag, int orderbyflag, int ordercolindex, Type ordercoltype)
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
                var rowindex = 1;
                while (!sr.EndOfStream)
                {
                    var tokens = TokenParser.CreateTokens(conditions);
                    var polishtokens = TokenParser.PolishNT(tokens);
                    var row = sr.ReadLine();
                    var rowvalues = TableUtils.Split(row, '\t');
                    if (CheckExpression(rowvalues, polishtokens, tablecols))
                    {
                        if(orderbyflag != 0)
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
                                }
                                else
                                {
                                    for (int k = 0; k < indexes.Length; k++)
                                        rowline += rowvalues[indexes[k]] + '\t';
                                    rows.AddLast(rowline);
                                }
                            }
                        }
                        else
                        {

                            if (distinctflag == true)
                            {
                                ImpLinkedList<string> a = null;
                                ImpLinkedList<int> b = null;

                                Distinct(row, rowindex, rowvalues, indexes, inputcol, ref rowhash,ref a, ref b);
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
            if (!File.Exists($"{_path}/{Name}.txt"))
            {
                Console.WriteLine("This Table doesn't exist");
                return;
            }

            if (expression == null)
                Console.WriteLine("No Conditions provided");
            else
            {
                string[] collines;
                using (StreamReader sr = new StreamReader($"{_path}/{Name}.txt"))
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
                using (StreamReader sr = new StreamReader($"{_path}/{Name}.txt"))
                {                   
                    using(StreamWriter sw = new StreamWriter($"{_path}/{Name}temp.txt"))
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
                File.Replace($"{_path}/{Name}temp.txt", $"{_path}/{Name}.txt", null);
                Console.WriteLine($"Deleted {deletednumber} entries");
            }

        }
        private static bool CheckExpression(string[] rowvalues, List<Token> tokens, ImpLinkedList<ColElement> tablecols)
        {
            string[] colnames = new string[tablecols.Count];
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
    }
}
