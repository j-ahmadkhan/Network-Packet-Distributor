using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Utility2
{
    public partial class ExtendedLog : Form
    {
        public ExtendedLog()
        {
            InitializeComponent();
        }

        private void ExtendedLog_Load(object sender, EventArgs e)
        {
            Distributor.ext = this;
        }

        private void ExtendedLog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void ExtendedLog_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ContextMenuStrip cm = new ContextMenuStrip();
            char[] par = { '[', ']' };
            string[] abc = new string[30];
            abc = listView1.SelectedItems[0].SubItems[1].Text.Split(par);
            //MenuItem mi = new MenuItem(listView1.SelectedItems[0].SubItems[1].Text);
            for (int i = 0; i < abc.Length; i++)
            {
                cm.Items.Add(abc[i]);
            }
            cm.Show(e.X,e.Y);
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}