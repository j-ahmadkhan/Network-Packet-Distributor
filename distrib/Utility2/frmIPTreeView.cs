using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using PacketDotNet;
using System.Threading;
namespace Utility2
{
    public partial class frmIPTreeView : Form
    {
        public frmIPTreeView()
        {
            InitializeComponent();
        }
        IpPacket packet = null;
        TreeNode node = null;
        int Sport = 0;
        int Dport = 0;
        string SAddress = null;
        string DAddress = null;
        int HeaderLength = 0;
        string Proto = null;
        string version = null;
        int TTL = 0;
        int PayloadLength = 0;
        int HopLimit = 0;
        int TotalLength = 0;
        DateTime dt;
        delegate void Send();
        Thread Sender = null;
        static int counter = 0;
        public IpPacket PACKET
        {
            set
            {
                packet = value;
            }
        }
        static public ArrayList AL = new ArrayList();

        public void ShowHex()
        {
            IpPacket pack = (IpPacket)AL[treeView1.SelectedNode.Index];
            string HEX = BitConverter.ToString(pack.Bytes);
            HEX.Replace("-", " ");
            ContextMenuStrip cm = new ContextMenuStrip();
            cm.Items.Add(HEX);
        }
        public void AddPacket(IpPacket pack)
        {
            try
            {
                packet = pack;
                SAddress = packet.SourceAddress.ToString();
                DAddress = packet.DestinationAddress.ToString();
                HeaderLength = packet.HeaderLength;
                PayloadLength = packet.PayloadLength;
                TotalLength = packet.TotalLength;
                Proto = packet.Protocol.ToString();
                version = packet.Version.ToString();
                TTL = packet.TimeToLive;
                dt = packet.Timeval.Date;
                counter += 1;
                string head = SAddress + " -> " + DAddress +" ("+ counter.ToString()+ ")";
                //TreeNode rootnode = new TreeNode();
                node = new TreeNode();
                //rootnode.Text = head;
                node.Text = head;
                node.Nodes.Add("HeaderLength:"+ HeaderLength.ToString());
                node.Nodes.Add("PayLoadLength:"+ PayloadLength.ToString());
                node.Nodes.Add("TotalLength:"+ TotalLength.ToString());
                node.Nodes.Add("Protocol:"+ Proto);
                node.Nodes.Add("Version:"+ version);
                node.Nodes.Add("TTL:"+ TTL.ToString());
                node.Nodes.Add("Time:"+ dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + ":" + dt.Millisecond.ToString());
                if (packet.Protocol == IPProtocolType.TCP | packet.Protocol == IPProtocolType.UDP)
                {
                    if (packet.Protocol == IPProtocolType.TCP)
                    {
                        TcpPacket tcp = (TcpPacket)packet.PayloadPacket;
                        Sport = tcp.SourcePort;
                        Dport = tcp.DestinationPort;

                    }
                    if (packet.Protocol == IPProtocolType.UDP)
                    {
                        UdpPacket udp = (UdpPacket)packet.PayloadPacket;
                        Sport = udp.SourcePort;
                        Dport = udp.DestinationPort;
                    }
                    node.Nodes.Add("SrcPort:"+ Sport.ToString());
                    node.Nodes.Add("DstPort:"+ Dport.ToString());
                }
                node.ToolTipText = "Click To View Payload Data";
                //rootnode.Nodes.Add(node);
                treeView1.Nodes.Add(node);
                checkedListBox1.Items.Add(head);
                AL.Add(packet);
            }
            catch (Exception ex)
            {
                ex = ex;
            }
            
        }
        public void send()
        {
                int prt = 0;
                IPEndPoint iep = null;
                UdpClient sock = new UdpClient();
                int count = 0;
                try
                {
                    prt = Convert.ToInt16(txtPort.Text);
                    iep = new IPEndPoint(IPAddress.Parse(txtIPAddress1.IP), prt);
                    //byte[] data1 = new byte[1024];//Encoding.ASCII.GetBytes("This is a test message");
                    for(int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            IpPacket pack = (IpPacket)AL[i];
                            int recv = pack.TotalLength;
                            sock.Send(pack.Bytes, recv, iep);
                            count += 1;
                        }
                    }
                    //object[] parameters = { lbl, Counter };
                    //ThisInvoked th = new ThisInvoked(CounterInvoker);
                    //this.Invoke(th, parameters);
                    sock.Close();
                    Sender.Join();
                    button1.Enabled = true;
                    MessageBox.Show( count.ToString() + " packets sent to " + txtIPAddress1.IP,"Packets Sent");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error while Sending Packet/s","Error");
                    sock.Close();
                    Sender.Join();
                    button1.Enabled = true;
                }
        }
        
        private void frmIPTreeView_Load(object sender, EventArgs e)
        {

        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.Nodes.IndexOf(treeView1.SelectedNode) != -1)
            {
                IpPacket pack = (IpPacket)AL[treeView1.Nodes.IndexOf(treeView1.SelectedNode)];
                string HEX = BitConverter.ToString(pack.Bytes);
                //HEX.Replace("-", " ");
                richTextBox1.Text = HEX;
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sender = new Thread(new ThreadStart(send));
            Sender.Start();
        }

        private void txtIPAddress2_Load(object sender, EventArgs e)
        {

        }
    }
}