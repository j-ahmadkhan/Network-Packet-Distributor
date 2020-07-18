using System;
using System.Collections.Generic;
using System.Text;
//using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using Microsoft.Win32;
using PacketDotNet;
using PacketDotNet.LLDP;
using PacketDotNet.Utils;
using SharpPcap;

namespace Utility2
{
    public class Promiscous
    {

    static string[] devc = null;
    static Distributor dist = null;

    public Distributor DIST
    {
        set
        {
            dist = value;
        }
    }

    public string[] ProvideDeviceList()
    {
        

        // Print SharpPcap version 
        string ver = SharpPcap.Version.VersionString;
        Console.WriteLine("SharpPcap {0}, Example1.IfList.cs", ver);

        // Retrieve the device list
        LivePcapDeviceList devices = LivePcapDeviceList.Instance;

        // If no devices were found print an error
        if (devices.Count < 1)
        {
            //Console.WriteLine("No devices were found on this machine");
            return null;
        }
        devc = new string[150];
        // Print out the available network devices 
        int i = 0;
        foreach (LivePcapDevice dev in devices)
        {
            devc[i] = dev.ToString();
            i += 1;
            /////////////////////Console.WriteLine("{0}\n", dev.ToString());
        }
        return devc;

    }
        public string[] ProvideDeviceNames()
        {


            // Print SharpPcap version 
            string ver = SharpPcap.Version.VersionString;
            Console.WriteLine("SharpPcap {0}, Example1.IfList.cs", ver);

            // Retrieve the device list
            LivePcapDeviceList devices = LivePcapDeviceList.Instance;
         
            LivePcapDevice device = null;
            // If no devices were found print an error
            if (devices.Count < 1)
            {
                //Console.WriteLine("No devices were found on this machine");
                return null;
            }
            devc = new string[150];
            // Print out the available network devices 
            int i = 0;
            foreach (LivePcapDevice dev in devices)
            {
      
                devc[i] = dev.Description;
                i += 1;
                /////////////////////Console.WriteLine("{0}\n", dev.ToString());
            }
            return devc;

        }


    }
}
