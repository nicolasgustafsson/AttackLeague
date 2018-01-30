using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Network
{
    class NetPeer : IConnection
    {
        TcpClient myClient = new TcpClient();
        const int Port = 32323;

        public const string YlfsIP = "81.231.17.12";
        public const string NicosIP = "109.228.175.44";

        BinaryWriter myWriter;
        BinaryReader myReader;

        public void StartConnection(string aIP)
        {
            try
            {
                myClient.Connect(aIP, Port);

                InitializeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect to host!");
                Console.WriteLine(e);
            }
        }

        public void SetClient(TcpClient aClient)
        {
            myClient = aClient;
        }

        public void InitializeConnection()
        {
            NetworkStream NetStream = myClient.GetStream();
            myWriter = new BinaryWriter(NetStream);
            myReader = new BinaryReader(NetStream);

            Thread ReadThread = new Thread(ReadLoop);
            ReadThread.Name = "Reading Time!";
            ReadThread.Start();
        }

        private void ReadLoop()
        {
            while(myClient.Connected)
            {
                Read();
            }
        }

        void Read()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();

                object obj = formatter.Deserialize(myClient.GetStream());
                Debug.Assert(obj is BaseMessage);


                //Console.WriteLine($"{obj.apa}, {obj.derp}, {obj.thingy}!");
            }
            catch (Exception e)
            {
                myReader.Close();
                myClient.Close();

                Console.WriteLine("Host closed the connection");
            }
        }

        public void WriteMessage<T>(T aMessage)
        {
            NetworkStream netStream = myClient.GetStream();

            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, aMessage);
        }
    }
}