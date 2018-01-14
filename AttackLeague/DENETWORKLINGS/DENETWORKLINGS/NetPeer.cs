using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class NetPeer
    {
        TcpClient myClient = new TcpClient();
        const int Port = 32323;

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

        private void InitializeConnection()
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
            while(true)
            {
                Read();
            }
        }

        void Read()
        {
            try
            {
                Console.WriteLine(myReader.ReadString());
            }
            catch (Exception e)
            {
                myReader.Close();
                myClient.Close();

                Console.WriteLine("Host closed the connection");
            }
        }

        public void Write(string aMessage)
        {
            NetworkStream netStream = myClient.GetStream();
            BinaryWriter binaryWriter = new BinaryWriter(netStream);

            binaryWriter.Write(aMessage);
        }
    }
}