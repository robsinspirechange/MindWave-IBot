using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace BluetoothConnect
{
    class RecieveData
    {
        Program1 obj = new Program1();      //Object of Program class

        public static void Main(String[] args)
        { 
            RecieveData ob = new RecieveData();
            ob.RecieveSignal();
        }

        void RecieveSignal()
        {
            obj.getDevice();
            Thread thdUdpServer = new Thread(new ThreadStart(serverThread));        //Thread to recieve data from HelloEEG
            thdUdpServer.Start();
        }

    public void serverThread()
        {
            Console.WriteLine("Server thread getting started!!");
            UdpClient udpCliend = new UdpClient(5005);
            while (true)
            {
                try
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 5005);
                    Byte[] recieveByte = udpCliend.Receive(ref RemoteIpEndPoint);
                    obj.sendData(recieveByte);              //Send data to I-Bot
                    Console.WriteLine("Sending data");
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }
    }
}