namespace Utility2
{
    partial class TxtIPAddress
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Add1 = new System.Windows.Forms.TextBox();
            this.Add2 = new System.Windows.Forms.TextBox();
            this.Add3 = new System.Windows.Forms.TextBox();
            this.Add4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = ".";
            // 
            // Add1
            // 
            this.Add1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Add1.Font = new System.Drawing.Font("Nina", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add1.Location = new System.Drawing.Point(3, 3);
            this.Add1.MaxLength = 3;
            this.Add1.Name = "Add1";
            this.Add1.Size = new System.Drawing.Size(20, 14);
            this.Add1.TabIndex = 5;
            this.Add1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Add1_KeyPress);
            this.Add1.TextChanged += new System.EventHandler(this.Add1_TextChanged);
            this.Add1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Add1_KeyDown);
            // 
            // Add2
            // 
            this.Add2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Add2.Font = new System.Drawing.Font("Nina", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add2.Location = new System.Drawing.Point(27, 3);
            this.Add2.MaxLength = 3;
            this.Add2.Name = "Add2";
            this.Add2.Size = new System.Drawing.Size(20, 14);
            this.Add2.TabIndex = 6;
            this.Add2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Add2_KeyPress);
            this.Add2.TextChanged += new System.EventHandler(this.Add2_TextChanged);
            this.Add2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Add2_KeyDown);
            // 
            // Add3
            // 
            this.Add3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Add3.Font = new System.Drawing.Font("Nina", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add3.Location = new System.Drawing.Point(50, 3);
            this.Add3.MaxLength = 3;
            this.Add3.Name = "Add3";
            this.Add3.Size = new System.Drawing.Size(20, 14);
            this.Add3.TabIndex = 7;
            this.Add3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Add3_KeyPress);
            this.Add3.TextChanged += new System.EventHandler(this.Add3_TextChanged);
            this.Add3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Add3_KeyDown);
            // 
            // Add4
            // 
            this.Add4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Add4.Font = new System.Drawing.Font("Nina", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add4.Location = new System.Drawing.Point(74, 3);
            this.Add4.MaxLength = 3;
            this.Add4.Name = "Add4";
            this.Add4.Size = new System.Drawing.Size(21, 14);
            this.Add4.TabIndex = 8;
            this.Add4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Add4_KeyPress);
            this.Add4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Add4_KeyDown);
            // 
            // TxtIPAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Add4);
            this.Controls.Add(this.Add3);
            this.Controls.Add(this.Add2);
            this.Controls.Add(this.Add1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "TxtIPAddress";
            this.Size = new System.Drawing.Size(99, 19);
            this.Load += new System.EventHandler(this.TxtIPAddress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Add1;
        private System.Windows.Forms.TextBox Add2;
        private System.Windows.Forms.TextBox Add3;
        private System.Windows.Forms.TextBox Add4;
    }
}
