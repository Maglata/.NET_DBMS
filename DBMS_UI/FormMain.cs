using DBMSPain.Utilities;
using OwnDBMS.Structures;
using OwnDBMS.Utilities;

namespace DBMS_UI
{
    public partial class FormMain : Form
    {
        public string _wintablepath = "../../../../AltDBMS/Tables";
        // Add an image for each table in the list

        public FormMain()
        {
            InitializeComponent();
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
            input = input.Trim('\r','\n');
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
                    Commands.Insert(splitinput[1]);
                    break;
                case "HELP":
                    textBoxOutput.Text = "Available Commands: CREATETABLE, DROPTABLE, LISTTABLES, TABLEINFO, SELECT, INSERT, DELETE";
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
                        coldefaultvalue[1] = coldefaultvalue[1].Trim('\"');
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
    }
}