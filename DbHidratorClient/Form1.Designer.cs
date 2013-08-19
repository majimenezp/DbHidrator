namespace Client
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDbProvider = new System.Windows.Forms.ComboBox();
            this.lstchkTables = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetTables = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numRows = new System.Windows.Forms.NumericUpDown();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.rdbtnAll = new System.Windows.Forms.RadioButton();
            this.rdbtnSimul = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ConnectionString:";
            // 
            // txtConnStr
            // 
            this.txtConnStr.Location = new System.Drawing.Point(109, 6);
            this.txtConnStr.Multiline = true;
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.Size = new System.Drawing.Size(300, 40);
            this.txtConnStr.TabIndex = 1;
            this.txtConnStr.Text = "data source=127.0.0.1;user id=testapp;password=testapp;initial catalog=testapp;Mu" +
    "ltipleActiveResultSets=true;";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Db provider:";
            // 
            // cmbDbProvider
            // 
            this.cmbDbProvider.FormattingEnabled = true;
            this.cmbDbProvider.Items.AddRange(new object[] {
            "System.Data.SqlClient",
            "System.Data.SQLite",
            "System.Data.Odbc",
            "System.Data.OleDb",
            "System.Data.OracleClient",
            "System.Data.SqlServerCe.4.0",
            "System.Data.SqlServerCe.3.5"});
            this.cmbDbProvider.Location = new System.Drawing.Point(109, 52);
            this.cmbDbProvider.Name = "cmbDbProvider";
            this.cmbDbProvider.Size = new System.Drawing.Size(300, 21);
            this.cmbDbProvider.TabIndex = 3;
            this.cmbDbProvider.Text = "System.Data.SqlClient";
            // 
            // lstchkTables
            // 
            this.lstchkTables.FormattingEnabled = true;
            this.lstchkTables.Location = new System.Drawing.Point(12, 121);
            this.lstchkTables.Name = "lstchkTables";
            this.lstchkTables.Size = new System.Drawing.Size(397, 259);
            this.lstchkTables.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tables:";
            // 
            // btnGetTables
            // 
            this.btnGetTables.Location = new System.Drawing.Point(109, 80);
            this.btnGetTables.Name = "btnGetTables";
            this.btnGetTables.Size = new System.Drawing.Size(75, 23);
            this.btnGetTables.TabIndex = 6;
            this.btnGetTables.Text = "Get tables";
            this.btnGetTables.UseVisualStyleBackColor = true;
            this.btnGetTables.Click += new System.EventHandler(this.btnGetTables_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 387);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "How many rows per table?:";
            // 
            // numRows
            // 
            this.numRows.Location = new System.Drawing.Point(147, 386);
            this.numRows.Name = "numRows";
            this.numRows.Size = new System.Drawing.Size(120, 20);
            this.numRows.TabIndex = 8;
            this.numRows.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 501);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(460, 23);
            this.progress.TabIndex = 9;
            // 
            // rdbtnAll
            // 
            this.rdbtnAll.AutoSize = true;
            this.rdbtnAll.Checked = true;
            this.rdbtnAll.Location = new System.Drawing.Point(15, 427);
            this.rdbtnAll.Name = "rdbtnAll";
            this.rdbtnAll.Size = new System.Drawing.Size(113, 17);
            this.rdbtnAll.TabIndex = 10;
            this.rdbtnAll.TabStop = true;
            this.rdbtnAll.Text = "Insert all in one trip";
            this.rdbtnAll.UseVisualStyleBackColor = true;
            // 
            // rdbtnSimul
            // 
            this.rdbtnSimul.AutoSize = true;
            this.rdbtnSimul.Location = new System.Drawing.Point(147, 427);
            this.rdbtnSimul.Name = "rdbtnSimul";
            this.rdbtnSimul.Size = new System.Drawing.Size(136, 17);
            this.rdbtnSimul.TabIndex = 11;
            this.rdbtnSimul.Text = "Simulate random inserts";
            this.rdbtnSimul.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 460);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 536);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rdbtnSimul);
            this.Controls.Add(this.rdbtnAll);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnGetTables);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstchkTables);
            this.Controls.Add(this.cmbDbProvider);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "DbHidrator";
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDbProvider;
        private System.Windows.Forms.CheckedListBox lstchkTables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetTables;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numRows;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.RadioButton rdbtnAll;
        private System.Windows.Forms.RadioButton rdbtnSimul;
        private System.Windows.Forms.Button btnStart;
    }
}

