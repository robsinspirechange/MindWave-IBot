using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using NeuroSky.ThinkGear;
using NeuroSky.ThinkGear.Algorithms;

using InTheHand.Net.Sockets;
using System.Diagnostics;
using HelloEEG;
using System.Net;
using System.Net.Sockets;

namespace testprogram {

    public class Program {

        public static bool connected = false;
        public static int eyeblink = 0;
        static Connector connector;
        static byte poorSig;
        public static double counter;
        public static System.Diagnostics.Stopwatch watch = new Stopwatch();  //watch to create timestamp

        public static BlueConnect bc;

        public static void Main(string[] args) {

            Console.WriteLine("Hello I-Bot!");
            // Initialize a new Connector and add event handlers
            bc = new BlueConnect();
            Thread thdUdpServer = new Thread(new ThreadStart(bc.ConnectDevice));    //thread to make connection alive 
            thdUdpServer.Start();
            //bc = new BlueConnect();
            //bc.ConnectDevice();
            connector = new Connector();

            connector.DeviceConnected += new EventHandler(OnDeviceConnected);
            connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
            connector.DeviceValidating += new EventHandler(OnDeviceValidating);


            // Scan for devices across COM ports
            // The COM port named will be the first COM port that is checked.
            connector.ConnectScan("COM40");
         
            // Blink detection needs to be manually turned on
            connector.setBlinkDetectionEnabled(true);
            Thread.Sleep(450000);

            System.Console.WriteLine("Goodbye.");
            connector.Close();
            Environment.Exit(0);
        }


        // Called when a device is connected 

        static void OnDeviceConnected(object sender, EventArgs e) {

            Connector.DeviceEventArgs de = (Connector.DeviceEventArgs)e;

            Console.WriteLine("Device found on: " + de.Device.PortName);
            de.Device.DataReceived += new EventHandler(OnDataReceived);
        }


        // Called when scanning fails

        static void OnDeviceFail(object sender, EventArgs e) {

            Console.WriteLine("No devices found! :(");

        }


        // Called when each port is being validated

        static void OnDeviceValidating(object sender, EventArgs e) {

            Console.WriteLine("Validating: ");

        }

        // Called when data is received from a device

        static void OnDataReceived(object sender, EventArgs e) {

            //Device d = (Device)sender;

            Device.DataEventArgs de = (Device.DataEventArgs)e;
            DataRow[] tempDataRowArray = de.DataRowArray;

            TGParser tgParser = new TGParser();
            tgParser.Read(de.DataRowArray);

           
          //  counter = counter + watch.Elapsed.TotalMilliseconds;


            /* Loops through the newly parsed data of the connected headset*/
			// The comments below indicate and can be used to print out the different data outputs. 

            for (int i = 0; i < tgParser.ParsedData.Length; i++)
            {
                if ((tgParser.ParsedData[i].ContainsKey("BlinkStrength")) && (tgParser.ParsedData[i]["BlinkStrength"] > 50))
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();            //Start timer
                    Console.WriteLine("Eyeblink " + tgParser.ParsedData[i]["BlinkStrength"]);
                    eyeblink++;                                                     //Increase counter if EyeBlibk is detected
                    Console.WriteLine(eyeblink);
                    Console.WriteLine(counter);
                }
                if ((watch.Elapsed.TotalMilliseconds > 1000) && (tgParser.ParsedData[i].ContainsKey("Attention")) && (tgParser.ParsedData[i]["Attention"] < 70))
                {
                    Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                    if ((eyeblink > 0) && (eyeblink < 5))
                    {
                        bc.sendData(eyeblink.ToString());               //Send data 
                        eyeblink = 0;
                        counter = 0;
                    }
                    else if (eyeblink > 4)
                    {
                        eyeblink = 0;
                        counter = 0;
                    }
                }
                else if ((watch.Elapsed.TotalMilliseconds > 1000) && (tgParser.ParsedData[i].ContainsKey("Attention")))
                {
                    Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                    bc.sendData("5");
                    eyeblink = 0;
                    counter = 0;
                    // Send 5 to stop Bot
                }
               } 
            }
        }   
    }
