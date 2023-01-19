using DBMSPain.Utilities;
using OwnDBMS.Utilities;

namespace DBMS_UI
{
    public partial class FormMain : Form
    {
        public string _wintablepath = "../../../../AltDBMS/Tables";
        // To Do Make The column info Clear fully after each request
        public FormMain()
        {
            InitializeComponent();
            dataGridViewTable.Visible = false;
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
            }
        }

        private void listViewTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTables.SelectedItems.Count > 0)
            {
                var item = listViewTables.SelectedItems[0].Text;
                fillGridViewTable(Commands.Select($"* FROM {item}"));
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
    }
}