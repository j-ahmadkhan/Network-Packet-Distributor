using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Utility2
{
    public partial class frmPromiscous : Form
    {
        public frmPromiscous()
        {
            InitializeComponent();
        }
        string filter = "ip and tcp";
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbDevices.SelectedIndex == -1)
            {
                Distributor.deviceIndex = 0;
            }
            else
            {
                Distributor.deviceIndex = cmbDevices.SelectedIndex;
            }
            Distributor.Filter = chkAddFilter.Checked;
            Distributor.WritePacket = chkWritePackets.Checked;
            Distributor.ReadFromFile = chkReadFromFile.Checked;
            Distributor.filterType = filter;
            this.Hide();
            
        }

        private void cmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void frmPromiscous_Load(object sender, EventArgs e)
        {
            Promiscous pm = new Promiscous();
            string[] Device = pm.ProvideDeviceNames();

            if (Device != null)
            {

                for (int i = 0; i < Device.Length; i++)
                {
                   if(Device[i] != null)
                   cmbDevices.Items.Add(Device[i]);
                }
            }
            else
            {
                cmbDevices.Items.Add("No Network Devices Found Available");

            }
        }

        private void chkAddFilter_CheckedChanged(object sender, EventArgs e)
        {
            //ContextMenu cm = new ContextMenu();
            if (chkAddFilter.Checked)
            {
                ContextMenuStrip cm = new ContextMenuStrip();

                string[] a = { "tcp", "udp", "ip", "ip and tcp", "icmp","arp" };
                //MenuItem mi = null; 
                for (int i = 0; i < 6; i++)
                {
                    //mi = new MenuItem(a[i]);
                    cm.Items.Add(a[i]);
                }

                Point pnt = new Point(145, 58);
                cm.Show(this, pnt);
                cm.ItemClicked += new ToolStripItemClickedEventHandler(cm_ItemClicked);
            }
        }

        void cm_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
             filter =  e.ClickedItem.Text;
             chkAddFilter.Text = "filter Applied for " + filter + " packets";
        }

        private void chkWritePackets_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWritePackets.Checked)
            {
                openFileDialog1.ShowDialog();
                Distributor.filename = openFileDialog1.FileName;
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void chkReadFromFile_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReadFromFile.Checked)
            {
                openFileDialog1.ShowDialog();
                Distributor.ReadFileName = openFileDialog1.FileName;
            }
        }

        

    }
}