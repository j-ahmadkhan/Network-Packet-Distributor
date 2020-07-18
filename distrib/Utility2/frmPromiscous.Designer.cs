namespace Utility2
{
    partial class frmPromiscous
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chkAddFilter = new System.Windows.Forms.CheckBox();
            this.chkWritePackets = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkReadFromFile = new System.Windows.Forms.CheckBox();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDevices);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose a Listening Device";
            // 
            // cmbDevices
            // 
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(6, 19);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(305, 21);
            this.cmbDevices.TabIndex = 0;
            this.cmbDevices.SelectedIndexChanged += new System.EventHandler(this.cmbDevices_SelectedIndexChanged);
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(104, 146);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(169, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkAddFilter
            // 
            this.chkAddFilter.AutoSize = true;
            this.chkAddFilter.Location = new System.Drawing.Point(18, 70);
            this.chkAddFilter.Name = "chkAddFilter";
            this.chkAddFilter.Size = new System.Drawing.Size(210, 17);
            this.chkAddFilter.TabIndex = 3;
            this.chkAddFilter.Text = "Apply Filter (default filter is \"ip and tcp\")";
            this.chkAddFilter.UseVisualStyleBackColor = true;
            this.chkAddFilter.CheckedChanged += new System.EventHandler(this.chkAddFilter_CheckedChanged);
            // 
            // chkWritePackets
            // 
            this.chkWritePackets.AutoSize = true;
            this.chkWritePackets.Location = new System.Drawing.Point(18, 93);
            this.chkWritePackets.Name = "chkWritePackets";
            this.chkWritePackets.Size = new System.Drawing.Size(125, 17);
            this.chkWritePackets.TabIndex = 4;
            this.chkWritePackets.Text = "Write Packets To file";
            this.chkWritePackets.UseVisualStyleBackColor = true;
            this.chkWritePackets.CheckedChanged += new System.EventHandler(this.chkWritePackets_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "packetFile";
            this.openFileDialog1.Filter = "dat files|*.txt";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // chkReadFromFile
            // 
            this.chkReadFromFile.AutoSize = true;
            this.chkReadFromFile.Location = new System.Drawing.Point(18, 116);
            this.chkReadFromFile.Name = "chkReadFromFile";
            this.chkReadFromFile.Size = new System.Drawing.Size(133, 17);
            this.chkReadFromFile.TabIndex = 5;
            this.chkReadFromFile.Text = "Read Packets from file";
            this.chkReadFromFile.UseVisualStyleBackColor = true;
            this.chkReadFromFile.CheckedChanged += new System.EventHandler(this.chkReadFromFile_CheckedChanged);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "packetFile";
            this.openFileDialog2.Filter = "dat files|*.txt";
            // 
            // frmPromiscous
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(341, 176);
            this.Controls.Add(this.chkReadFromFile);
            this.Controls.Add(this.chkWritePackets);
            this.Controls.Add(this.chkAddFilter);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPromiscous";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Promiscous Mode Settings";
            this.Load += new System.EventHandler(this.frmPromiscous_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkAddFilter;
        private System.Windows.Forms.CheckBox chkWritePackets;
        public System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkReadFromFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}