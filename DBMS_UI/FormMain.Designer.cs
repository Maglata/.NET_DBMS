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
            this.listViewTables = new System.Windows.Forms.ListView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelTables = new System.Windows.Forms.Label();
            this.textBoxCommandInput = new System.Windows.Forms.TextBox();
            this.buttonExecuteCommand = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewTables
            // 
            this.listViewTables.Location = new System.Drawing.Point(12, 34);
            this.listViewTables.MultiSelect = false;
            this.listViewTables.Name = "listViewTables";
            this.listViewTables.Scrollable = false;
            this.listViewTables.ShowGroups = false;
            this.listViewTables.Size = new System.Drawing.Size(110, 353);
            this.listViewTables.TabIndex = 0;
            this.listViewTables.UseCompatibleStateImageBehavior = false;
            this.listViewTables.View = System.Windows.Forms.View.List;
            this.listViewTables.SelectedIndexChanged += new System.EventHandler(this.listViewTables_SelectedIndexChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(99, 6);
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
            this.labelTables.Location = new System.Drawing.Point(12, 7);
            this.labelTables.Name = "labelTables";
            this.labelTables.Size = new System.Drawing.Size(58, 21);
            this.labelTables.TabIndex = 2;
            this.labelTables.Text = "Tables";
            // 
            // textBoxCommandInput
            // 
            this.textBoxCommandInput.AcceptsReturn = true;
            this.textBoxCommandInput.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxCommandInput.Location = new System.Drawing.Point(265, 34);
            this.textBoxCommandInput.Multiline = true;
            this.textBoxCommandInput.Name = "textBoxCommandInput";
            this.textBoxCommandInput.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxCommandInput.Size = new System.Drawing.Size(677, 86);
            this.textBoxCommandInput.TabIndex = 3;
            this.textBoxCommandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCommandInput_KeyDown);
            // 
            // buttonExecuteCommand
            // 
            this.buttonExecuteCommand.Location = new System.Drawing.Point(265, 5);
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
            this.textBoxOutput.Location = new System.Drawing.Point(265, 492);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxOutput.Size = new System.Drawing.Size(677, 86);
            this.textBoxOutput.TabIndex = 5;
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            this.dataGridViewTable.AllowUserToResizeColumns = false;
            this.dataGridViewTable.AllowUserToResizeRows = false;
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Location = new System.Drawing.Point(265, 126);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.ReadOnly = true;
            this.dataGridViewTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewTable.RowTemplate.Height = 25;
            this.dataGridViewTable.Size = new System.Drawing.Size(677, 360);
            this.dataGridViewTable.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 590);
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
    }
}