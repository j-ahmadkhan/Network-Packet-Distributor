namespace Utility2
{
    partial class TxtPort
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
            this.Port = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Port
            // 
            this.Port.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Port.Location = new System.Drawing.Point(3, 4);
            this.Port.MaxLength = 5;
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(60, 13);
            this.Port.TabIndex = 0;
            this.Port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Port_KeyPress);
            // 
            // TxtPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Port);
            this.Name = "TxtPort";
            this.Size = new System.Drawing.Size(66, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Port;
    }
}
