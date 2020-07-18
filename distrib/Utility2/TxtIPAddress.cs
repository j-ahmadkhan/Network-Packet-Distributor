using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace Utility2
{
    public partial class TxtIPAddress : UserControl
    {
        public TxtIPAddress()
        {
            InitializeComponent();
        }
        #region Data
        private string ip = null;
        //private bool error = true;
        
        #endregion
        #region FUNCTIONS
        public string IP
        {
           get
           {
               return Add1.Text + "." + Add2.Text + "." + Add3.Text + "." + Add4.Text;
           }
        }
        bool RangeValidation(string ch,string st)
        {
            Regex reg = null;
            //switch (legth)
            {
             //   case 0:
                reg = new Regex(@"^([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])$");
            }
            if (reg.IsMatch(st+ch))
            {
                return true;
            }
            else { return false; }
               
        }
        #endregion
        private void TxtIPAddress_Load(object sender, EventArgs e)
        {
            Add1.Focus();
        }

        private void Add2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey | (e.KeyCode == Keys.Back && Add2.Text == ""))
            {
                Add1.Focus();
            }
            else if (e.KeyCode == Keys.Decimal && Add2.Text.Length >= 1)
            { Add3.Focus(); }
        }

        private void Add3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey | (e.KeyCode == Keys.Back && Add3.Text == ""))
            {
                Add2.Focus();
            }
            else if (e.KeyCode == Keys.Decimal && Add3.Text.Length >= 1)
            { Add4.Focus(); }
        }

        private void Add4_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.ShiftKey) | (e.KeyCode == Keys.Back && Add4.Text == ""))
            {
                Add3.Focus();
            }
        }
        private void Add1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsNumber(e.KeyChar)& RangeValidation(e.KeyChar.ToString(),Add1.Text))|(char.IsControl(e.KeyChar)& (!char.IsWhiteSpace(e.KeyChar))))
            {
                 e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void Add2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsNumber(e.KeyChar) & RangeValidation(e.KeyChar.ToString(), Add2.Text)) | (char.IsControl(e.KeyChar) & (!char.IsWhiteSpace(e.KeyChar))))
            {
                e.Handled = false;
                if (Add2.Text.Length >= 2)
                { Add3.Focus(); }
            }
            else { e.Handled = true; }
        }

        private void Add3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsNumber(e.KeyChar) & RangeValidation(e.KeyChar.ToString(), Add3.Text)) | (char.IsControl(e.KeyChar) & (!char.IsWhiteSpace(e.KeyChar))))
            {
                e.Handled = false;
                if (Add3.Text.Length >= 2)
                { Add4.Focus(); }
            }
            else { e.Handled = true; }
        }

        private void Add4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsNumber(e.KeyChar) & RangeValidation(e.KeyChar.ToString(), Add4.Text)) | (char.IsControl(e.KeyChar) & (!char.IsWhiteSpace(e.KeyChar))))
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void Add1_TextChanged(object sender, EventArgs e)
        {
            if (Add1.Text.Length == 3)
            { Add2.Focus(); }
        }

        private void Add2_TextChanged(object sender, EventArgs e)
        {
            if (Add2.Text.Length == 3)
            { Add3.Focus(); }
        }

        private void Add3_TextChanged(object sender, EventArgs e)
        {
            if (Add3.Text.Length == 3)
            { Add4.Focus(); }
        }

        private void Add1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Decimal && Add1.Text.Length >= 1)
            { Add2.Focus(); }
        }
     

        

}  

    
}
