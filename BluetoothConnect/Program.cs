using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Threading;


namespace BluetoothConnect
{
    public class Program1
    {
        BluetoothClient bc = new BluetoothClient();
        
        public void getDevice()
        {
            try
            {
                BluetoothDeviceInfo[] array = bc.DiscoverDevices();

                foreach (BluetoothDeviceInfo device in array)
                {
                    Console.WriteLine(device.DeviceName.ToString());

                    if(device.DeviceName.Equals("HC-05"))       // Bluetooth Device name is HC-50
                    {
                         bool isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, "1234");       //Pair request
                             if (isPaired)
                            {
                               Console.WriteLine("Paired with I-Bot!!");
                                var ep = new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort);
                               Thread.Sleep(1000);
                               bc.Connect(ep);
                               Console.WriteLine("Connected with I-Bot!!");
                        
                       // byte[] buffer = System.Text.Encoding.ASCII.GetBytes("2");
                        
                        //while (true)
                        //{
                        //    bluetoothStream.Write(buffer, 0, buffer.Length);
                        //}
                    }
                }
                }

            }
            catch (Exception e) { Console.Write(e); }
        }

       public void sendData(byte[] buffer)
       {
           var bluetoothStream = bc.GetStream();            //BluetoothStream to send data
           bluetoothStream.Write(buffer, 0, buffer.Length);         //Sending data to I-bot
       }

    }
}
