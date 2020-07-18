using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Utility2
{
    public partial class frmUdpReceiver : Form
    {
        public frmUdpReceiver()
        {
            InitializeComponent();
        }
#region FUNCTIONS
        void PortInput(TextBox txt, char ch)
        {
           
        }

#endregion

    

        private void frmUdpReceiver_Load(object sender, EventArgs e)
        {
         

        }

        private void TXTRcvPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            ErrorProvider pd = new ErrorProvider();
            if ((e.KeyChar < 49 | e.KeyChar > 57) & (e.KeyChar != 8)) 
            {
               // TXTRcvPort.Text = TXTRcvPort.Text.Substring(0,TXTRcvPort.Text.Length -1) ;
                e.Handled = false;
            }
            


        }
    }
}