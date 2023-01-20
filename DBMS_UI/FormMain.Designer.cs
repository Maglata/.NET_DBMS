namespace DBMS_UI
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.listViewTables = new System.Windows.Forms.ListView();
            this.imagetable = new System.Windows.Forms.ImageList(this.components);
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelTables = new System.Windows.Forms.Label();
            this.textBoxCommandInput = new System.Windows.Forms.TextBox();
            this.buttonExecuteCommand = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            this.textBoxTableInfo = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewTables
            // 
            this.listViewTables.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listViewTables.Location = new System.Drawing.Point(12, 34);
            this.listViewTables.MultiSelect = false;
            this.listViewTables.Name = "listViewTables";
            this.listViewTables.ShowGroups = false;
            this.listViewTables.Size = new System.Drawing.Size(318, 288);
            this.listViewTables.TabIndex = 0;
            this.listViewTables.UseCompatibleStateImageBehavior = false;
            this.listViewTables.View = System.Windows.Forms.View.List;
            this.listViewTables.SelectedIndexChanged += new System.EventHandler(this.listViewTables_SelectedIndexChanged);
            // 
            // imagetable
            // 
            this.imagetable.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imagetable.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagetable.ImageStream")));
            this.imagetable.TransparentColor = System.Drawing.Color.Black;
            this.imagetable.Images.SetKeyName(0, "1994825.png");
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(307, 9);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(23, 22);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "🗘";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelTables
            // 
            this.labelTables.AutoSize = true;
            this.labelTables.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTables.Location = new System.Drawing.Point(12, 8);
            this.labelTables.Name = "labelTables";
            this.labelTables.Size = new System.Drawing.Size(58, 21);
            this.labelTables.TabIndex = 2;
            this.labelTables.Text = "Tables";
            // 
            // textBoxCommandInput
            // 
            this.textBoxCommandInput.AcceptsReturn = true;
            this.textBoxCommandInput.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxCommandInput.Location = new System.Drawing.Point(336, 34);
            this.textBoxCommandInput.Multiline = true;
            this.textBoxCommandInput.Name = "textBoxCommandInput";
            this.textBoxCommandInput.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxCommandInput.Size = new System.Drawing.Size(606, 86);
            this.textBoxCommandInput.TabIndex = 3;
            this.textBoxCommandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCommandInput_KeyDown);
            // 
            // buttonExecuteCommand
            // 
            this.buttonExecuteCommand.Location = new System.Drawing.Point(336, 8);
            this.buttonExecuteCommand.Name = "buttonExecuteCommand";
            this.buttonExecuteCommand.Size = new System.Drawing.Size(58, 23);
            this.buttonExecuteCommand.TabIndex = 4;
            this.buttonExecuteCommand.Text = "🗲";
            this.buttonExecuteCommand.UseVisualStyleBackColor = true;
            this.buttonExecuteCommand.Click += new System.EventHandler(this.buttonExecuteCommand_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Enabled = false;
            this.textBoxOutput.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxOutput.Location = new System.Drawing.Point(336, 492);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.PlaceholderText = " To see the Available Commands type: HELP";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxOutput.Size = new System.Drawing.Size(606, 86);
            this.textBoxOutput.TabIndex = 5;
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            this.dataGridViewTable.AllowUserToResizeColumns = false;
            this.dataGridViewTable.AllowUserToResizeRows = false;
            this.dataGridViewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Location = new System.Drawing.Point(336, 126);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.ReadOnly = true;
            this.dataGridViewTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewTable.RowTemplate.Height = 25;
            this.dataGridViewTable.Size = new System.Drawing.Size(606, 360);
            this.dataGridViewTable.TabIndex = 6;
            // 
            // textBoxTableInfo
            // 
            this.textBoxTableInfo.Enabled = false;
            this.textBoxTableInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxTableInfo.Location = new System.Drawing.Point(12, 328);
            this.textBoxTableInfo.Multiline = true;
            this.textBoxTableInfo.Name = "textBoxTableInfo";
            this.textBoxTableInfo.ReadOnly = true;
            this.textBoxTableInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxTableInfo.Size = new System.Drawing.Size(318, 250);
            this.textBoxTableInfo.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(314, 592);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(8, 23);
            this.comboBox1.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 590);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBoxTableInfo);
            this.Controls.Add(this.dataGridViewTable);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.buttonExecuteCommand);
            this.Controls.Add(this.textBoxCommandInput);
            this.Controls.Add(this.labelTables);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.listViewTables);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DBMS";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView listViewTables;
        private Button buttonRefresh;
        private Label labelTables;
        private TextBox textBoxCommandInput;
        private Button buttonExecuteCommand;
        private TextBox textBoxOutput;
        private DataGridView dataGridViewTable;
        private TextBox textBoxTableInfo;
        private ComboBox comboBox1;
        private ImageList imagetable;
    }
}