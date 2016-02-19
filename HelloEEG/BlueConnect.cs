using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
/* This Class is used to connect to BluetoothConnect application */
namespace HelloEEG
{
    public class BlueConnect
    {
        UdpClient udpClient = new UdpClient();
        public void ConnectDevice()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5005);
            udpClient.Connect(ipep);
            Console.WriteLine("Connected with local server!!");
        }
        public void sendData(string data)
        {
            Byte[] senddata = Encoding.ASCII.GetBytes(data);
            udpClient.Send(senddata, senddata.Length);      //Send data to client
            Console.WriteLine("data sent: " + data);
        }
    }
}