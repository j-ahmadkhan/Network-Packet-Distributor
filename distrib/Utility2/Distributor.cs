using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PacketDotNet;
using PacketDotNet.LLDP;
using PacketDotNet.Utils;
using SharpPcap;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;

namespace Utility2
{
    public partial class Distributor : Form
    {
        public Distributor()
        {
            InitializeComponent();
        }
      
        #region VARIABLES
        Thread  MainThread = null;
        Thread CounterThread = null;
        LivePcapDevice device = null;
        PcapDevice offline = null;
        PacketDotNet.Packet packet = null;
        RawPacket Rapacket = null;
        int recv = 0; 
        byte[] data = null;
        string DeviceList = null;
        static public bool Filter = false;
        static public bool WritePacket = false;
        static public bool ReadFromFile = false;
        static public string ReadFileName = null;
        static public int deviceIndex = 0;
        static public string filterType = "arp";
        static int RawCounter = 0;
        static public PacketDotNet.IPv4Packet packe = null;
        static public ARPPacket arp = null;
        static public TcpPacket tcp = null;
        static public UdpPacket udp = null;
        static public ICMPv4Packet icmp = null;
        static public string filename = null;
        static RawEthernet re = new RawEthernet();
        static string RawHeader = null;
        static string EthHeader = null;
        static string PayPacket = null;
        bool TryIPSpoofing = false;
        int PacketCounter = 0;
        int PacketCounter1 = 0;
        int PacketCounter2 = 0;
        int PacketCounter3 = 0;
        int PacketCounter4 = 0;
        int PacketCounter5 = 0;
        int PacketCounter6 = 0;
        int PacketCounter7 = 0;
        int PacketCounter8 = 0;
        int PacketCounter9 = 0;
        int PacketCounter10 = 0;
        int Receivers = 0;
        int PicReceivers = 0;
        bool threadStarted = false;
        bool SpoofIP = false;
        static bool active1, active2, active3, active4, active5, active6, active7, active8, active9, active10 = false;
        static bool check1, check2, check3, check4, check5, check6, check7, check8, check9, check10 = false;  
        ListViewItem lv = null;
        string IPADDRESS = null;
        int PORT = 0;
        string MachineIP =null;
        Socket Server = null;
        IPEndPoint sender = null;
        string DestinationIP = null;
        string DestinationPort = null;
        delegate void Manager(bool dest, UserControl ip, TxtPort port, PictureBox pic);
        delegate void ThisInvoked(Label lbl, int Counter);

        static public ExtendedLog ext = null;
        public ExtendedLog EXTD
        {
            set
            {
                ext = value;
            }
            
        }
        static public frmIPTreeView Tree = null;
        public frmIPTreeView TREE
        {
            set
            {
                TREE = value;
            }

        }
#endregion
        # region Socket Functions

        /// <summary>
        /// //////////////FUNCTION TO RECEIVE DATA AT SPECIFIED PORT
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="Port"></param>
        string[] GetDeviceList(bool name)
        {
            Promiscous pm = new Promiscous();
            if (!name)
            {
                string[] Device = pm.ProvideDeviceList();
                return Device;
            }
            else
            {
                string[] Device1 = pm.ProvideDeviceNames();
                return Device1;
            }
       
        }
        bool CreateSocket(string IPAdd, int Port)
        {
            IPADDRESS = IPAdd;
            PORT = Port;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(MachineIP), Port);
            Server = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                Server.Bind(ipep);
                if (IPADDRESS == null)
                {
                    Server.EnableBroadcast = true;
                }
                //sender = new IPEndPoint(IPAddress.Parse(IPADDRESS), 0);
                notify.BalloonTipText = "Socket Bound at " + MachineIP;
                notify.ShowBalloonTip(2);
                return true;
            }
            catch (Exception ex)
            {
                notify.BalloonTipText = ex.ToString();
                notify.ShowBalloonTip(3);
                return false;
            }
               
        }

        void ChangeDHCP(string ipaddress)
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject objNewGate = null;
            ManagementBaseObject outPar = null;
            ManagementBaseObject hostPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                try
                {

                    inPar = mo.GetMethodParameters("EnableStatic");
                    hostPar = mc.GetMethodParameters("EnableDNS");
                    objNewGate = mo.GetMethodParameters("SetGateways");

                    //Set IPAddress and Subnet Mask
                    inPar["IPAddress"] = new string[] { ipaddress };
                    inPar["SubnetMask"] = new string[] { "255.255.255.0" };

                    // Set the HostName
                    hostPar["DNSHostName"] = "user machine";
                    hostPar["DNSDomain"] = null;
                    hostPar["DNSServerSearchOrder"] = null;
                    hostPar["DNSDomainSuffixSearchOrder"] = null;


                    //Set DefaultGateway
                    //objNewGate["DefaultIPGateway"] = new string[] { "192.168.1.100" };
                    //objNewGate["GatewayCostMetric"] = new int[] { 1 };

                    outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                    outPar = mo.InvokeMethod("SetGateways", objNewGate, null);
                    outPar = mc.InvokeMethod("EnableDNS", hostPar, new InvokeMethodOptions());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to Set IP : " + ex.Message);
                }


            }
        }
        void ReceiveData()
        {
                data = new byte[4096];

                //byte first = null;
                //UdpClient recv = new UdpClient(AddressFamily.InterNetwork);
                IPEndPoint iep = new IPEndPoint(IPAddress.Any,PORT);
                LivePcapDeviceList devices = LivePcapDeviceList.Instance;
                
                if (ReadFromFile)
                {
                    if (ReadFileName != null)
                    {
                        try
                        {
                            offline = new OfflinePcapDevice(ReadFileName);
                            offline.Open();
                        }
                        catch (Exception ex)
                        {
                            notify.BalloonTipTitle = "file reading error!!";
                            notify.BalloonTipText = ex.Message;
                            notify.ShowBalloonTip(100);
                            offline = null;
                        }
                    }
                }
                else
                {
                    device = devices[deviceIndex];
                    //device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
                    try
                    {
                        device.Open(DeviceMode.Promiscuous);
                        if (Filter)
                        {
                            device.SetFilter(filterType);
                        }
                    }
                    catch (Exception ex)
                    {
                        notify.BalloonTipTitle = "error!!";
                        notify.BalloonTipText = ex.Message;
                        notify.ShowBalloonTip(100);
                    }
                    //recv.Connect(iep);
                    //Server.Receive(
                    if (WritePacket)
                    {
                        if (filename != null)
                        {
                            try
                            {
                                device.DumpOpen(filename);
                            }
                            catch (Exception ex)
                            {
                                notify.BalloonTipTitle = "file writing error!!";
                                notify.BalloonTipText = ex.Message;
                                notify.ShowBalloonTip(100);
                            }
                        }
                    }
                }
        
                try
                {
                    while (true)
                    {
                      
                        // Extract a device from the list 
                        recv = 0;
                        bool valid = true;     
                        {
                            if (!ReadFromFile)
                            {
                                if ((Rapacket = device.GetNextPacket()) != null)
                                {

                                    recv = Rapacket.Data.Length;
                                    valid = IPFilter();
                                }
                            }
                            else
                            {
                                if (ReadFileName != null & offline.Opened)
                                {
                                    try
                                    {
                                        //offline = new OfflinePcapDevice(ReadFileName);
                                        //offline.Open();
                                        if ((Rapacket = offline.GetNextPacket()) != null)
                                        {
                                            recv = Rapacket.Data.Length;
                                            valid = IPFilter();
                                            offline.DumpFlush();
                                        }
                                        else
                                        {
                                            offline.DumpFlush();
                                            offline.DumpClose();
                                            offline.Close();
                                        }

                                    }
                                     catch (Exception ex)
                                    {
                                        notify.BalloonTipTitle = "file reading error!!";
                                        notify.BalloonTipText = ex.Message;
                                        notify.ShowBalloonTip(100);
                                        offline.DumpFlush();
                                        offline.Close();
                                    }
                                }
                                    
                            }

                        }

                        if (recv > 0 & valid)
                        {
                            PacketCounter++;
                            if (device != null)
                            {
                                if (device.DumpOpened)
                                { device.Dump(Rapacket); }
                            }
                            object[] parameters = { lbl0, PacketCounter };
                            ThisInvoked th = new ThisInvoked(CounterInvoker);
                            this.Invoke(th, parameters);

                            if (Receivers > 0)
                            {
                                if (check1 && active1) { PacketCounter1++; }
                                if (check2 && active2) { PacketCounter2++; }
                                if (check3 && active3) { PacketCounter3++; }
                                if (check4 && active4) { PacketCounter4++; }
                                if (check5 && active5) { PacketCounter5++; }
                                if (check6 && active6) { PacketCounter6++; }
                                if (check7 && active7) { PacketCounter7++; }
                                if (check8 && active8) { PacketCounter8++; }
                                if (check9 && active9) { PacketCounter9++; }
                                if (check10 && active10) { PacketCounter10++; }
                                SendData(check1, active1, TXTIPSnd1.IP.Trim(), TXTSndPort1.Text, data, recv, lbl1, PacketCounter1);
                                SendData(check2, active2, TXTIPSnd2.IP.Trim(), TXTSndPort2.Text, data, recv, lbl2, PacketCounter2);
                                SendData(check3, active3, TXTIPSnd3.IP.Trim(), TXTSndPort3.Text, data, recv, lbl3, PacketCounter3);
                                SendData(check4, active4, TXTIPSnd4.IP.Trim(), TXTSndPort4.Text, data, recv, lbl4, PacketCounter4);
                                SendData(check5, active5, TXTIPSnd5.IP.Trim(), TXTSndPort5.Text, data, recv, lbl5, PacketCounter5);
                                SendData(check6, active6, TXTIPSnd6.IP.Trim(), TXTSndPort6.Text, data, recv, lbl6, PacketCounter6);
                                SendData(check7, active7, TXTIPSnd7.IP.Trim(), TXTSndPort7.Text, data, recv, lbl7, PacketCounter7);
                                SendData(check8, active8, TXTIPSnd8.IP.Trim(), TXTSndPort8.Text, data, recv, lbl8, PacketCounter8);
                                SendData(check9, active9, TXTIPSnd9.IP.Trim(), TXTSndPort9.Text, data, recv, lbl9, PacketCounter9);
                                SendData(check10, active10, TXTIPSnd10.IP.Trim(), TXTSndPort10.Text, data, recv, lbl10, PacketCounter10);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    notify.BalloonTipText = ex.ToString();
                    notify.ShowBalloonTip(3);
                }
            
                //MessageBox.Show("Messages Sent................... CLOSING SERVER");
                //Server.Close();
                // Close the pcap device 
                //device.Close();
                
    
       }

        void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (e.Packet is TcpPacket | e.Packet is UdpPacket | e.Packet is IpPacket | e.Packet is RawPacket)
            {
                //TcpPacket pack
               //PacketDotNet.TcpPacket pack = (TcpPacket)e.Packet;
                //.Pcap.pack.sourcea
                //SharpPcap= ;
                data = new byte[1024];
                notify.Visible = true;
                notify.BalloonTipTitle = "Sniffer On:[Click Icon for Extended View]";
                recv = e.Packet.Data.Length;
                data = e.Packet.Data;
                notify.BalloonTipText = "Packet Data Length:" + recv.ToString() + "\n Listening on Device:" + device.Name;
                notify.ShowBalloonTip(100);

            }
            
        }
        void SendData(bool Enable,bool Active,string IPAdd, string Port, byte[] data, int recv,Label lbl, int Counter)
        {
            if (Enable & Active)
            {
                int prt = 0;
                IPEndPoint iep = null;
                try
                {

                    prt = Convert.ToInt16(Port);
                    iep = new IPEndPoint(IPAddress.Parse(IPAdd), prt);
                    if (!SpoofIP)
                    {
                        UdpClient sock = new UdpClient(0);
                        sock.Send(data, recv, iep);
                        sock.Close();
                    }
                    else
                    {
                        packe.DestinationAddress = IPAddress.Parse(IPAdd);
                        //string str = "0x00,0x1C,0xC0,0x9C,0x88,0x28,0x00,0x50,0x04,0x4A,0xB8,0x33,0x08,0x00";
                        RawHeader = RawHeader.Replace("-", ",0x");
                        RawHeader = "0x" + RawHeader;
                        if (packe.Protocol == IPProtocolType.TCP)
                        {
                            tcp.DestinationPort = (ushort)prt;
                            PayPacket = BitConverter.ToString(tcp.Bytes);
                            PayPacket = PayPacket.Replace("-", ",0x");
                            PayPacket = "0x" + PayPacket;
                        }
                        else if (packe.Protocol == IPProtocolType.UDP)
                        {
                            udp.DestinationPort = (ushort)prt;
                            PayPacket = BitConverter.ToString(udp.Bytes);
                            PayPacket = PayPacket.Replace("-", ",0x");
                            PayPacket = "0x" + PayPacket;
                        }
                        if (packe.Protocol == IPProtocolType.TCP | packe.Protocol == IPProtocolType.UDP)
                        {
                            EthHeader = BitConverter.ToString(packe.Header);
                            EthHeader = EthHeader.Replace("-", ",0x");
                            EthHeader = "0x" + EthHeader;
                            PayPacket = RawHeader + "," + EthHeader + ","+ PayPacket;
                        }
                        else
                        {
                            EthHeader = BitConverter.ToString(packe.Bytes);
                            EthHeader = EthHeader.Replace("-", ",0x");
                            EthHeader = "0x" + EthHeader;
                            PayPacket = RawHeader + "," + EthHeader;
                        }

                        char[] c = new char[] { ',' };
                        string[] del = PayPacket.Split(c);
                        byte[] pack = new byte[del.Length];
                        for (int i = 0; i < del.Length; i++)
                        {
                            pack[i] = Convert.ToByte(del[i], 16);
                        }
                        device.SendPacket(pack);
                        
                    }
                    object[] parameters = { lbl, Counter };
                    ThisInvoked th = new ThisInvoked(CounterInvoker);
                    this.Invoke(th, parameters);
                }
                catch(Exception ex)
                {
                    notify.BalloonTipText = ex.ToString();
                }
               
            }
        }
/// <summary>
/// ////////////////////////////////////////////////IP FILTER FOR SOURCE AND DESTIONATION///////////////////////
/// </summary>
/// <returns></returns>
        bool IPFilter()
        {
            packe = null;
            
            int SPort = -1;
            int DPort = -1;
            IPAddress SendIP = null;
            IPAddress DestIP = null;
            bool ValidPacket = true;
            string a = null;
            try
            {
                DateTime time = Rapacket.Timeval.Date;
                //data = Rapacket.Data;
                packet = PacketDotNet.LLDPPacket.Parse(Rapacket.Data);
                EthernetPacket ttp = (EthernetPacket)packet;

                /*byte[] pack = new byte[] { 0x00,0x1C,0xC0,0x9C,0x88,0x28, 
                0x00, 0x50, 0x04, 0x4A, 0xB8, 0x54, 
                0x08, 0x00, 
                0x45, 0x00, 0x00, 0x4E, 
                0x00, 0x7A, 0x00, 0x00, 0x80, 0x11, 
                0xB5, 0x62, 
                0xC0, 0xA8, 0x01, 0x73, 
                0xC0, 0xA8, 0x01, 0x82,
                0x00, 0x89, 
                0x00, 0x89, 0x00, 0x3A, 0x77, 0x07, 
                0x80, 0x1F, 0x01, 0x10, 0x00, 0x01, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x20, 0x45, 0x43, 0x45, 0x42, 0x45, 
                0x44, 0x45, 0x4C, 0x46, 0x46, 0x46, 
                0x41, 0x43, 0x4F, 0x45, 0x42, 0x46, 
                0x47, 0x45, 0x48, 0x43, 0x4F, 0x45, 
                0x44, 0x46, 0x4B, 0x43, 0x41, 0x43, 
                0x41, 0x41, 0x41, 0x00, 0x00, 0x20, 
                0x00, 0x01 };*/


                RawHeader = BitConverter.ToString(ttp.Header);
                if (ttp.Type != EthernetPacketType.Arp & ttp.PayloadPacket != null)
                {
                    packe = (PacketDotNet.IPv4Packet)ttp.PayloadPacket;
                    
                    /*byte[] pac = new byte[] {0x00,0x1C,0xC0,0x9C,0x88,0x28,  //destination mac  00 1C C0 9C 88 29 
										        0x00,0x50,0x04,0x4A,0xB8,0x33,  //source mac 0050044AB832
										        0x08,0x00};
                    //EthernetPacket eth = new EthernetPacket(ttp.SourceHwAddress,System.Net.NetworkInformation.PhysicalAddress.Parse("001CC09C8829"), ttp.Type);
                    byte[] conc = new byte[pac.Length + packe.Bytes.Length];
                    //System.Buffer.BlockCopy(eth.Bytes, 0, conc, 0, eth.Bytes.Length);

                    System.Buffer.BlockCopy(pac, 0, conc, 0, pac.Length);
                    System.Buffer.BlockCopy(packe.Bytes,0,conc,14,packe.Bytes.Length);*/
                    
                  
                    //ChangeDHCP(packe.SourceAddress.ToString()); ////////////////////// CHANGE DHCP ///////////////
    

                    SendIP = packe.SourceAddress;
                    DestIP = packe.DestinationAddress;
                  
                    
                    if (ReadFromFile)
                    { a = "Packet Data Length:" + recv.ToString() + "\n Listening on Device:" + offline.Name + "\n Packet Type =" + ttp.Type.ToString() + "\n Add:" + packe.SourceAddress.ToString() + "->" + packe.DestinationAddress.ToString(); }
                    else
                    { a = "Packet Data Length:" + recv.ToString() + "\n Listening on Device:" + device.Name + "\n Packet Type =" + ttp.Type.ToString() + "\n Add:" + packe.SourceAddress.ToString() + "->" + packe.DestinationAddress.ToString(); }
                    
                    if (packe.Protocol == IPProtocolType.TCP)
                    {
                        
                        tcp = (TcpPacket)packe.PayloadPacket;
                        a += "\n SPort:" + tcp.SourcePort.ToString() + " DPort:" + tcp.DestinationPort.ToString();
                        SPort = tcp.SourcePort;
                        DPort = tcp.DestinationPort;
                        
                    }
                    else if (packe.Protocol == IPProtocolType.UDP)
                    {
                        udp = (UdpPacket)packe.PayloadPacket;
                        a += "\n SPort:" + udp.SourcePort.ToString() + " DPort:" + udp.DestinationPort.ToString();
                        SPort = udp.SourcePort;
                        DPort = udp.DestinationPort;
                    }
                    notify.BalloonTipText = a;

                }
                else if (ttp.Type == EthernetPacketType.Arp)
                {
                    arp = (ARPPacket)ttp.PayloadPacket;
                    SendIP = arp.SenderProtocolAddress;
                    DestIP = arp.TargetProtocolAddress;
                    if (ReadFromFile)
                    {
                        notify.BalloonTipText = "Packet Data Length:" + recv.ToString() + "\n Listening on Device:" + offline.Name + "\n TTP =" + ttp.ToString();
                    }
                    else
                    {
                        notify.BalloonTipText = "Packet Data Length:" + recv.ToString() + "\n Listening on Device:" + device.Name + "\n TTP =" + ttp.ToString();
                    }
                }
                notify.BalloonTipTitle = "Listening : [Click Icon for Extended Log]";
                notify.ShowBalloonTip(100);
            
            if (chkSource.Checked | chkDest.Checked)
                {
                        if (chkSource.Checked)
                        {
                            if (TXTRcvIP1.IP != "...")
                            {
                                if (!(IPAddress.Equals(SendIP,IPAddress.Parse(TXTRcvIP1.IP))))
                                { ValidPacket = false; }
                            }
                            if (TXTRcvPorts1.Text != "")
                            {
                                if (!(Convert.ToInt32(TXTRcvPorts1.Text) == SPort))
                                { ValidPacket = false; }
                            }
                        }
                        if (chkDest.Checked)
                        {
                            if (txtIPDest1.IP != "...")
                            {
                                if (!(IPAddress.Equals(DestIP,IPAddress.Parse(txtIPDest1.IP))))
                                { ValidPacket = false; }
                            }
                            if (txtDestPort1.Text != "")
                            {
                                if (!(Convert.ToInt32(txtDestPort1.Text) == DPort))
                                { ValidPacket = false; }
                            }
                        }
                     
                }

            }
            catch (Exception ex)
            {
                notify.BalloonTipTitle = "Error!!!!!!!!!!!!!!";
                notify.BalloonTipText = ex.Message.ToString();
                notify.ShowBalloonTip(100);
                ValidPacket = false;
            }

            return ValidPacket;
        }
 /// <summary>
 /// ///////////////////////////////////end ip filter function/////////////////////////////////////////////
 /// </summary>
 /// <returns></returns>
       
        string GetMachineIP()
        {
            String strHostName = Dns.GetHostName();
            Console.WriteLine("Host Name: " + strHostName);

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Enumerate IP addresses
            int nIP = 0;
            IPAddress ip = null;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                //Console.WriteLine("IP #" + ++nIP + ": " + ipaddress.ToString());
                ip = ipaddress;
            }
            return ip.ToString(); 
        }
        
        void ThreadFunc()
        {
            int port = 0;
            string ip = null;
            try
            {
                ip = TXTRcvIP1.IP.Trim();
            //    if(TXTRcvPorts.Text.Trim() != "")
             //   { port = Convert.ToInt32(TXTRcvPorts.Text.Trim()); }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("**************  Port is set to 0  ****************");
                  notify.BalloonTipText = "Receiver Address/Port Not Valid";
                  notify.ShowBalloonTip(3);
            }
            if (CreateSocket(ip, port))
            {
                ReceiveData(); 
            }
        }
#endregion

        #region INTERFACE FUNCTIONS

        void CounterReset(int Counter, Label lbl)
        {
            Counter = 0;
            lbl.Text = "0000";
        }

        void CounterInvoker(Label lbl, int Counter)
        {
            try
            {
                if (Counter >= 6500)
                { Counter = 0; }
                //else { Counter++; }
                lbl.Text = Counter.ToString();
                IPHostEntry HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));        
                
                EthernetPacket pac = (EthernetPacket)packet;
                lv = new ListViewItem();
                if (pac.Type == EthernetPacketType.IpV4)
                {
                    lv.BackColor = Color.Aquamarine;
                    Tree.AddPacket((IpPacket)pac.PayloadPacket);
                }
                lv.Text = Counter.ToString();
                lv.SubItems.Add(pac.ToString());
                ext.listView1.Items.Add(lv);
                lv.EnsureVisible();
                
                
            }
             catch (Exception ex)
            {
                notify.BalloonTipTitle = "Error!!!!!!!!!!!!!!";
                notify.BalloonTipText = ex.Message.ToString();
                notify.ShowBalloonTip(100);
            }
        }

        void ThreadManager(int thr)
        {
            switch (thr)
            {
                case 1:
                    if (!threadStarted)
                    {
                        MainThread = new Thread(new ThreadStart(ReceiveData));
                        MainThread.Start();
                        threadStarted = true;
                    }
                    else if (MainThread.ThreadState == ThreadState.Stopped | MainThread.ThreadState == ThreadState.Suspended)
                    { 
                        MainThread.Resume();
                        
                    }
                    break;
                case 2:
                    if (MainThread.ThreadState == ThreadState.Running)

                    {
                        try
                        {
                            Thread.Sleep(10);
                            MainThread.Suspend();
                            MainThread.Abort();
                            Server.Close();
                        }
                        catch
                        {

                        }
                        finally
                        {
                            Thread.Sleep(10);
                            MainThread.Suspend();
                            //MainThread.Join();
                            Server.Close();
                        }
                        //threadStarted = false;
                    
                    }
                    break;
            }

            //ControlManager(thr);
           
        }
        void ControlManager(int btntype,Button btn,CheckBox chk,TxtIPAddress ip, TxtPort port)
        {
            ////////////////////////// ThreadManager    
            try
            {
                
            if (btntype == 1)
            {
                
                    if (btn.Text == "<<")
                    {
                        btn.Text = ">>";
                        btn.BackColor = Color.LawnGreen; //LightSteelBlue
                        //ip.Enabled = false;
                        //port.Enabled = false;
                        //CreateSocket(ip.IP, Convert.ToInt32(port.Text));
                        MainThread = new Thread(new ThreadStart(ReceiveData));
                        MainThread.Start();
                        //ThreadManager(1);
                        threadStarted = true;

                    }
                    else
                    {
                        btn.Text = "<<";
                        btn.BackColor = Color.LightSteelBlue;
                        //ip.Enabled = true;
                        //port.Enabled = true;
                        Receivers = 0;
                        threadStarted = false;
                        for (int i = 0; i < 70; i++)
                        {
                            if (panel2.Controls[i] is Button)
                            {
                                panel2.Controls[i].Text = "<<";
                                panel2.Controls[i].BackColor = Color.SkyBlue;

                                
                            }
                            if (panel2.Controls[i] is CheckBox)
                            {
                                panel2.Controls[i].Enabled = true;
                            }
                        }
                        //Thread.Sleep(100);
                        //Server.Disconnect(true) ;
                        MainThread.Abort();
                        MainThread = null;
                        if (device != null)
                        {
                            device.StopCapture();
                        }
                        if (offline != null)
                        {
                            offline.DumpFlush();
                            offline.DumpClose();
                        }
                        if (Server != null)
                        {
                            Server.Close();
                        }
                        offline = null;
                        device = null;
                        Server = null;
                        progressBar1.Step = 0;
                        progressBar1.Hide();
                        lblStart.Text = "No Destination Active"; 
                        Receivers = 0;
                        active1 = active2 = active3 = active4 = active5 = active6 = active7 = active8 = active9 = active1 = false;
                        //\ThreadManager(2);
                    }
                }
                if (btntype == 2 & threadStarted)
                {
                    string dest = chk.Name.Substring(3);
                    if (btn.Text == "<<")
                    {

                        btn.Text = ">>";
                        btn.BackColor = Color.LawnGreen; //LightSteelBlue
                        chk.Enabled = false;
                        ip.Enabled = false;
                        port.Enabled = false;
                        notify.BalloonTipText = "Data Sending Started to destination # " + dest;
                        notify.ShowBalloonTip(2);
                        Receivers++;
                    }
                    else
                    {
                        btn.Text = "<<";
                        btn.BackColor = Color.SkyBlue;
                        chk.Enabled = true;
                        ip.Enabled = true;
                        port.Enabled = true;
                        notify.BalloonTipText = "Data Sending Stopped to destination # " + dest;
                        notify.ShowBalloonTip(2);
                        Receivers--;
                    }
                }

                if (Receivers > 0 & threadStarted == true)
                {
                    lblStart.Text = "Data Sending Started";
                    progressBar1.MarqueeAnimationSpeed = 40;
                    progressBar1.Show();
                    progressBar1.Step += 100;
                    notify.BalloonTipText = "Data Sending Started to " + Receivers.ToString() + " destinations";
                    notify.ShowBalloonTip(10);

                }
                else if (Receivers == 0 | threadStarted == false)
                {
                    lblStart.Text = "No Destination Active";
                    progressBar1.MarqueeAnimationSpeed = 0;
                    progressBar1.Hide();
                    notify.BalloonTipTitle = "Distribution Server";
                    notify.BalloonTipText = "Currently No Source/Destination Active";
                    notify.ShowBalloonTip(10);
                    //ThreadManager(2);

                }
                
            }
            catch(Exception ex)
            {
                notify.BalloonTipText = "An Error Occured.....\nPlease Try Again";
                notify.ShowBalloonTip(2);
            }
        }

        void ManageImages(bool dest,UserControl ip, TxtPort port,PictureBox pic)
        {
         
            switch (dest)
            {
                case true:
                        PicReceivers ++;
                        pic.Show();
                        port.Enabled = true;
                        ip.Enabled = true;
                        ip.Focus();
                        progressBar1.Show();
                    break;
                case false:
                    PicReceivers --;
                    pic.Hide();
                    port.Enabled = false;
                    ip.Enabled = false;
                    if (PicReceivers == 0)
                    {
                        progressBar1.Hide();
                    }
                    break; 
            }

            AdjustBarWidth();
            
        }

        void AdjustBarWidth()
        {
            if (chk1.Checked)
            {
                progressBar1.Width = 199;
            }
            if (chk2.Checked)
            {
                progressBar1.Width = 238;
            }
            if (chk3.Checked)
            {
                progressBar1.Width = 269;
            }
            if (chk4.Checked)
            {
                progressBar1.Width = 303;
            }
            if (chk5.Checked)
            {
                progressBar1.Width = 328;
            }
            if (chk6.Checked)
            {
                progressBar1.Width = 359;
            }
            if (chk7.Checked)
            {
                progressBar1.Width = 387;
            }
            if (chk8.Checked)
            {
                progressBar1.Width = 415;
            }
            if (chk9.Checked)
            {
                progressBar1.Width = 444;
            }
            if (chk10.Checked)
            {
                progressBar1.Width = 479;
            }
        }
        #endregion


        private void Distributor_Load(object sender, EventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            ext = new ExtendedLog();
            Tree = new frmIPTreeView();
            MainThread = new Thread(new ThreadStart(ThreadFunc));
            try
            { MachineIP = GetMachineIP(); }
            catch (Exception ex)
            {
                notify.BalloonTipText = ex.ToString();
                notify.ShowBalloonTip(2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MainThread.Abort();
            }
            catch (Exception ex)
            { ex = ex;  }
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                 PacketCounter = 0;
                 PacketCounter1 = 0;
                 PacketCounter2 = 0;
                 PacketCounter3 = 0;
                 PacketCounter4 = 0;
                 PacketCounter5 = 0;
                 PacketCounter6 = 0;
                 PacketCounter7 = 0;
                 PacketCounter8 = 0;
                 PacketCounter9 = 0;
                 PacketCounter10 = 0;
                 lbl0.Text = lbl1.Text = lbl2.Text = lbl3.Text = lbl4.Text = lbl5.Text = lbl6.Text = lbl7.Text = lbl8.Text = lbl9.Text = lbl10.Text  = "0000";
                 
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk1.Checked,TXTIPSnd1,TXTSndPort1,pic1);
            check1 = chk1.Checked;
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
           mg(chk2.Checked, TXTIPSnd2, TXTSndPort2,pic2);
           check2 = chk2.Checked;
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk3.Checked, TXTIPSnd3, TXTSndPort3,pic3);
            check3 = chk3.Checked;
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk4.Checked, TXTIPSnd4, TXTSndPort4,pic4);
            check4 = chk4.Checked;
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk5.Checked, TXTIPSnd5, TXTSndPort5, pic5);
            check5 = chk5.Checked;
        }

        private void chk6_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk6.Checked, TXTIPSnd6, TXTSndPort6, pic6);
            check6 = chk6.Checked;
        }

        private void chk7_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk7.Checked, TXTIPSnd7, TXTSndPort7, pic7);
            check7 = chk7.Checked;
        }

        private void chk8_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
            mg(chk8.Checked, TXTIPSnd8, TXTSndPort8, pic8);
            check8 = chk8.Checked;
        }

        private void chk9_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
           mg(chk9.Checked, TXTIPSnd9, TXTSndPort9, pic9);
           check9 = chk9.Checked;
        }

        private void chk10_CheckedChanged(object sender, EventArgs e)
        {
            Manager mg = new Manager(ManageImages);
           mg(chk10.Checked, TXTIPSnd10, TXTSndPort10, pic10);
           check10 = chk10.Checked;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd1.IP.Trim());
                if (TXTSndPort1.Text != "")
                {  
                    ControlManager(2, btn1, chk1, TXTIPSnd1, TXTSndPort1);
                    if (!active1) { active1 = true; }
                    else { active1 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTRcvIP1.IP.Trim());
                if (TXTRcvPorts1.Text != "" & !promisToolStripMenuItem.Checked)
                { ControlManager(1, button6, null, TXTRcvIP1, TXTRcvPorts1); }
    
            }
            catch
            {
                notify.BalloonTipText = "IP/PORT NOT Valid";
                notify.ShowBalloonTip(2);
            }
            finally
            {
                ControlManager(1, button6, null, TXTRcvIP1, TXTRcvPorts1);
            }
            
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd2.IP.Trim());
                if (TXTSndPort2.Text != "")
                { 
                    ControlManager(2, btn2, chk2, TXTIPSnd2, TXTSndPort2);
                    if (!active2) { active2 = true; }
                    else { active2 = false; }
                }
                
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }
            
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd3.IP.Trim());
                if (TXTSndPort3.Text != "")
                { 
                    ControlManager(2, btn3, chk3, TXTIPSnd3, TXTSndPort3);
                    if (!active3) { active3 = true; }
                    else { active3 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn4_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd4.IP.Trim());
                if (TXTSndPort4.Text != "")
                { 
                    ControlManager(2, btn4, chk4, TXTIPSnd4, TXTSndPort4);
                    if (!active4) { active4 = true; }
                    else { active4 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn5_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd5.IP.Trim());
                if (TXTSndPort5.Text != "")
                { 
                    ControlManager(2, btn5, chk5, TXTIPSnd5, TXTSndPort5);
                    if (!active5) { active5 = true; }
                    else { active5 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn6_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd6.IP.Trim());
                if (TXTSndPort6.Text != "")
                {
                    ControlManager(2, btn6, chk6, TXTIPSnd6, TXTSndPort6);
                    if (!active6) { active6 = true; }
                    else { active6 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn7_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd7.IP.Trim());
                if (TXTSndPort7.Text != "")
                { 
                    ControlManager(2, btn7, chk7, TXTIPSnd7, TXTSndPort7);
                    if (!active7) { active7 = true; }
                    else { active7 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn8_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd8.IP.Trim());
                if (TXTSndPort8.Text != "")
                { 
                    ControlManager(2, btn8, chk8, TXTIPSnd8, TXTSndPort8);
                    if (!active8) { active8 = true; }
                    else { active8 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn9_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd1.IP.Trim());
                if (TXTSndPort9.Text != "")
                { 
                    ControlManager(2, btn9, chk9, TXTIPSnd9, TXTSndPort9);
                    if (!active9) { active9 = true; }
                    else { active9 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void btn10_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(TXTIPSnd10.IP.Trim());
                if (TXTSndPort10.Text != "")
                { 
                    ControlManager(2, btn10, chk10, TXTIPSnd10, TXTSndPort10);
                    if (!active10) { active10 = true; }
                    else { active10 = false; }
                }
            }
            catch
            {
                notify.BalloonTipText = "IP NOT Valid";
                notify.ShowBalloonTip(2);
            }

        }

        private void addUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfo ui = new UserInfo();
            ui.ShowDialog();
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config con = new config();
            con.ShowDialog();
        }

        private void getDeviceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] Device = GetDeviceList(false);
            if (Device != null)
            {

                for (int i = 0; i < Device.Length; i++)
                {
                    DeviceList += Device[i];
                }
            }
            else
            {
                DeviceList = "No Devices Found Available";

            }
            MessageBox.Show(DeviceList, "Device List");
        }

        private void sniffPacketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (sniffPacketsToolStripMenuItem.Checked)

            frmPromiscous frm = new frmPromiscous();
            frm.ShowDialog();
        }

        private void Distributor_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                MainThread.Abort();
                Application.Exit();
            }
            catch (Exception ex)
            { ex = ex; }
            Application.Exit();
        }

        private void notify_Click(object sender, EventArgs e)
        {
            ext.Show();
        }

        private void chkSource_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSource.Checked)
            {
                TXTRcvIP1.Enabled = true;
                TXTRcvPorts1.Enabled = true;
                TXTRcvIP1.Select();
            }
            else
            {
                TXTRcvIP1.Enabled = false;
                TXTRcvPorts1.Enabled = false;
            }
        }

        private void chkDest_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDest.Checked)
            {
                txtDestPort1.Enabled = true;
                txtIPDest1.Enabled = true;
                txtIPDest1.Select();
            }
            else
            {
                txtIPDest1.Enabled = false;
                txtDestPort1.Enabled = false;
            }
        }

        private void iPLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tree.Show();
        }

        private void TrySpoofTsp_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void TrySpoofTsp_Click(object sender, EventArgs e)
        {
            if (TrySpoofTsp.Checked) { SpoofIP = true; }
            else { SpoofIP = false; }
        }



     


  

      

       

      

        
       
    }
}