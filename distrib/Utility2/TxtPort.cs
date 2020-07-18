using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Utility2
{
    public partial class TxtPort : UserControl
    {
        public TxtPort()
        {
            InitializeComponent();
        }
       
        public override string Text
        {
            set
            {
                Port.Text = value;
            }
            get
            {
                return Port.Text;
            }
 
        }

        private void Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            
        }

     
    }
}
