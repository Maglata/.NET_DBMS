using DBMSPain.Utilities;
using OwnDBMS.Structures;
using OwnDBMS.Utilities;

namespace DBMS_UI
{
    public partial class FormMain : Form
    {
        private static string _wintablepath = "../../../../AltDBMS/Tables";
        private static string _winindexespath = "../../../../AltDBMS/Indexes";
        private static string _winhashpath = "../../../../AltDBMS/";
        public FormMain()
        {
            InitializeComponent();
            CheckData();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            UpdateListTable();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            // To Do: Check for Data Correctness
            UpdateListTable();
        }
        private void UpdateListTable()
        {
            listViewTables.Items.Clear();
            var tablesinfolder = Directory.GetFiles(_wintablepath);
            for (int i = 0; i < tablesinfolder.Count(); i++)
                listViewTables.Items.Add(Path.GetFileNameWithoutExtension(tablesinfolder[i]));
        }
        private void buttonExecuteCommand_Click(object sender, EventArgs e)
        {
            InputParse(textBoxCommandInput.Text);
        }
        private void textBoxCommandInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                // Suppresses Key so that it doesnt make a new line
                e.SuppressKeyPress = true;
                InputParse(textBoxCommandInput.Text);
            }
                
        }
        private void InputParse(string input) 
        {
            //input = input.Trim('\r','\n');
            char[] chars = new char[2] { '\r', '\n' };
            input = TableUtils.Trim(input, chars);
            var splitinput = TableUtils.Split(input, ' ', 2);

            switch (TableUtils.ToUpper(splitinput[0]))
            {
                case "CREATETABLE":
                    Commands.CreateTable(splitinput[1]);
                    break;
                case "DROPTABLE":
                    FileManager.DeleteTableFile(splitinput[1]);
                    break;
                case "LISTTABLES":
                    {
                        string result = FileManager.GetTableNames();

                        if(result != null)
                        {
                            textBoxOutput.Text = result;
                        }
                        else
                        {
                            textBoxOutput.Text = "There are no available Tables";
                        }
                    }
                    break;
                case "TABLEINFO":
                    {
                        string a = string.Empty;
                        fillGridViewTable(FileManager.GetTableInfo(splitinput[1], out a),a);                      
                    }                
                    break;
                case "SELECT":
                    fillGridViewTable(Commands.Select(splitinput[1]));
                    break;
                case "INSERT":
                    {
                        string filepath = Commands.Insert(splitinput[1]);
                        if (filepath != null)
                            WriteHashToFile(CalculateFileHash(filepath), filepath);
                    }
                    break;
                case "HELP":
                    textBoxOutput.Text = "Available Commands: CREATETABLE, DROPTABLE, LISTTABLES, TABLEINFO, SELECT, INSERT, DELETE, CHECKDATA";
                    break;
                case "CHECKDATA":
                    {
                        CheckData();
                        textBoxOutput.Text = "Data Check Successful";
                    }
                    break;
                case "DELETE":
                    {
                        string filepath = Commands.Delete(splitinput[1]);
                        if (filepath != null)
                            WriteHashToFile(CalculateFileHash(filepath), filepath);
                    }
                    break;
                case "CREATEINDEX":
                    FileManager.CreateIndex(splitinput[1]);
                    break;
                case "DROPINDEX":
                    FileManager.DeleteIndex(splitinput[1]);
                    break;
                case "TEST":
                    textBoxOutput.Text = "Nothing put in Test Section";
                    break;
                default:
                    textBoxOutput.Text = $"Syntax Error: {input} not a valid syntax";
                    break;
            }
        }
        private void listViewTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTables.SelectedItems.Count > 0)
            {
                var item = listViewTables.SelectedItems[0].Text;
                fillGridViewTable(Commands.Select($"* FROM {item}"));
                showTableInfo(item);
            }
        }
        public void fillGridViewTable(string[] rows, string info)
        {
            if(rows != null)
            {
                dataGridViewTable.Visible = true;
                dataGridViewTable.Columns.Clear();
                dataGridViewTable.Rows.Clear();

                for (int i = 0; i < rows.Length; i++)
                {
                    if (i == 0)
                    {
                        var columninfo = TableUtils.Split(rows[i], '\t');
                        for (int k = 0; k < columninfo.Length; k++)
                        {
                            var columnname = TableUtils.Split(columninfo[k], ':')[0];
                            dataGridViewTable.Columns.Add(columnname, columnname);
                            dataGridViewTable.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        
                    }
                    else
                        dataGridViewTable.Rows.Add(TableUtils.Split(rows[i], '\t'));
                }

                textBoxOutput.Text = $"Entries Available in the Table are: {rows.Length - 1}" + 
                    Environment.NewLine +
                    $"Storage taken in Disk {info} bytes";

            }
        }
        public void fillGridViewTable(string[] rows)
        {
            if (rows != null)
            {
                dataGridViewTable.Visible = true;
                dataGridViewTable.Columns.Clear();
                dataGridViewTable.Rows.Clear();

                for (int i = 0; i < rows.Length; i++)
                {
                    if (i == 0)
                    {
                        var columninfo = TableUtils.Split(rows[i], '\t');
                        for (int k = 0; k < columninfo.Length; k++)
                        {
                            var columnname = TableUtils.Split(columninfo[k], ':')[0];
                            dataGridViewTable.Columns.Add(columnname, columnname);
                            dataGridViewTable.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }

                    }
                    else
                        dataGridViewTable.Rows.Add(TableUtils.Split(rows[i], '\t'));
                }

                textBoxOutput.Text = $"Entries Available in the Table are: {rows.Length - 1}";

            }
        }
        public void showTableInfo(string Name)
        {
            try
            {
                string[] columninfo;
                string tableinfo = $"Table : {Name}";

                if (!File.Exists($"{_wintablepath}/{Name}.txt"))
                {
                    throw new NotImplementedException($"Table Called: {Name} not Found");
                }
                using (StreamReader sr = new StreamReader($"{_wintablepath}/{Name}.txt"))
                {
                    columninfo = TableUtils.Split(sr.ReadLine(), '\t');
                }
                tableinfo += Environment.NewLine;
                tableinfo += "Columns:";
                for (int i = 0; i < columninfo.Length; i++)
                {
                    var colvalues = TableUtils.Split(columninfo[i], ':');
                    var coldefaultvalue = TableUtils.Split(colvalues[1], ' ');
                    colvalues[1] = coldefaultvalue[0];

                    // colvalues[0] - Name colvalues[1] - Type coldefaultvalue[1] - Default Value
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
                    tableinfo += Environment.NewLine;
                    tableinfo += $"{colvalues[0]} : {type.Name} ";
                    if (coldefaultvalue.Length != 1)
                    {
                        //coldefaultvalue[1] = coldefaultvalue[1].Trim('\"');
                        coldefaultvalue[1] = TableUtils.Trim(coldefaultvalue[1], '\"');
                        tableinfo += $"Default Value: {coldefaultvalue[1]}";
                    }
                }
                textBoxTableInfo.Text = tableinfo;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexcepted Error Occured: Getting Table Information Not Possible");
            }
        }
        private void CheckData()
        {
            CalculateHashesForFolder(_wintablepath);
            CalculateHashesForFolder(_winindexespath);
        }
        static public int CalculateFileHash(string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            int hash = 0;

            // Hash the contents of the file
            for (int i = 0; i < fileData.Length; i++)
            {
                // The modulus operator is used to keep the value of the hash within the range of an int
                hash += (hash * 31 + (fileData[i] * (i + 1))) % int.MaxValue;
            }

            return hash;
        }
        static public void WriteHashToFile(int hash, string filePath)
        {
            string outputFilePath = $"{_winhashpath}/hash.txt";

            // Create a string with the current file and hash
            string outputText = "File: " + filePath + "\nHash: " + hash;

            if (File.Exists(outputFilePath))
            {
                // If the output file already exists, read all the lines of the file into a string array
                string[] lines = File.ReadAllLines(outputFilePath);
                bool found = false;

                // Iterate through the array of lines and check if the line starts with the "File:" and input file path
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "File: " + filePath)
                    {
                        // If it finds it, replace the next line with the new hash value
                        lines[i + 1] = "Hash: " + hash;
                        found = true;
                        break;
                    }
                }
                // If it does not find the file in the output file, append the new hash value and file name to the existing file
                if (!found)
                {
                    File.AppendAllText(outputFilePath, "\n" + outputText);
                }
                // If it found the file, write all the lines back to the file
                else
                {
                    File.WriteAllLines(outputFilePath, lines);
                }
            }
            // If the output file does not exist, create a new file and write the hash value and file name
            else
            {
                File.WriteAllText(outputFilePath, outputText);
            }
        }
        static void CalculateHashesForFolder(string folderPath)
        {
            if (File.Exists($"{_winhashpath}/hash.txt") && File.ReadAllText($"{_winhashpath}/hash.txt") != "")
            {
                // Get a list of all files in the specified folder
                string[] files = Directory.GetFiles(folderPath);
                bool hashnotMatch = false;
                string[] lines = File.ReadAllLines($"{_winhashpath}/hash.txt");
                // Iterate through each file in the folder
                foreach (string file in files)
                {
                    // Calculate the hash of the current file
                    int currentHash = CalculateFileHash(file);
                    string formattedFile = string.Empty;
                    if (folderPath != _winindexespath)
                        formattedFile = TableUtils.Replace(file, "\\", "/");
                    else
                        formattedFile = file;

                    // Iterate through the array of lines
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Check if the line is equal to the "File:" and input file path
                        if (lines[i] == "File: " + formattedFile)
                        {
                            // Compare the next line with the current hash
                            if (lines[i + 1] != "Hash: " + currentHash)
                            {
                                hashnotMatch = true;
                                break;
                            }
                        }
                    }

                }
                // If the hash does not match, shows an error
                if (hashnotMatch)
                {
                    // In a fully working case we would not allow the user to continue working
                    MessageBox.Show("Interruption Detected");
                }
            }
            else if(!File.Exists($"{_winhashpath}/hash.txt"))
                File.Create($"{_winhashpath}/hash.txt");

        }
    }
}