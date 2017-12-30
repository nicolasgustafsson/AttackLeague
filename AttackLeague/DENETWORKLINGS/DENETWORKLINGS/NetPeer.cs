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

                NetworkStream NetStream = myClient.GetStream();
                myWriter = new BinaryWriter(NetStream);
                myReader = new BinaryReader(NetStream);

                Thread ReadThread = new Thread(ReadLoop);
                ReadThread.Name = "Reading Time!";
                ReadThread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not connect to host!");
                Console.WriteLine(e);
            }
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
            Console.WriteLine(myReader.ReadString());
        }
    }
}
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class NetConnection
    {
        TcpClient myClient = new TcpClient();
        const int myPort = 32323;

        public void StartConnection(string aConnectTo)
        {
            try
            {
                myClient.Connect(aConnectTo, myPort);
                myClient.Client.BeginAccept(ListenCallback, myClient.Client);


                OnConnectionSuccessful();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection failed!");
                Console.WriteLine(e.ToString());
            }
        }

        private void OnConnectionSuccessful()
        {
            Console.WriteLine("Connect successful!");
            BeginReceive();
        }

        public void WriteMessage()
        {
            myClient.Client.Send(Encoding.ASCII.GetBytes("Hejsan"), Encoding.ASCII.GetBytes("Hejsan").Length, SocketFlags.None);
        }

        private void BeginReceive()
        {
            byte[] buffer = new byte[256];
            int receivedSize = myClient.Client.Receive(buffer, myClient.Client.Available, SocketFlags.None);
            // BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveData, buffer);
            if (receivedSize > 0)
            {
                Console.WriteLine(BitConverter.ToString(buffer));
            }
        }

        private void ReceiveData(IAsyncResult aASyncResult)
        {
            byte[] data = aASyncResult.AsyncState as byte[];

            int receiveLength = 0;

            try
            {
                receiveLength = myClient.Client.EndReceive(aASyncResult);
            }
            catch (Exception e)
            {
                Console.WriteLine("end receive failed!");
                Console.WriteLine(e);
                return;
            }

            if (receiveLength == 0)
            {
                Console.WriteLine("receive length == 0! (???)");
                return;
            }

            Console.WriteLine(BitConverter.ToString(data));

            BeginReceive();
        }

        static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        //-------------

        public class StateObject
        {
            public Socket myWorkSocket = null;
            public const int BUFFER_SIZE = 256;
            public byte[] myBuffer = new byte[BUFFER_SIZE];
            public StringBuilder myStringBuilder = new StringBuilder();
        }

        public static void ListenCallback(IAsyncResult aAsyncResult)
        {
            //allDone.Set();
            Socket remoteSocket = (Socket)aAsyncResult.AsyncState;
            Socket localSocket = remoteSocket.EndAccept(aAsyncResult);
            StateObject receivedObject = new StateObject();
            receivedObject.myWorkSocket = localSocket;
            localSocket.BeginReceive(receivedObject.myBuffer, 0, StateObject.BUFFER_SIZE, 0, ReadCallback, receivedObject);
        }

        public static void ReadCallback(IAsyncResult aAsyncCallback)
        {
            StateObject readObject = (StateObject)aAsyncCallback.AsyncState;
            Socket readSocket = readObject.myWorkSocket;

            int read = readSocket.EndReceive(aAsyncCallback);

            if (read > 0)
            {
                readObject.myStringBuilder.Append(Encoding.ASCII.GetString(readObject.myBuffer, 0, read));
                readSocket.BeginReceive(readObject.myBuffer, 0, StateObject.BUFFER_SIZE, 0, ReadCallback, readObject);
            }
            else
            {
                if (readObject.myStringBuilder.Length > 1)
                {
                    //All of the data has been read, so displays it to the console
                    string strContent;
                    strContent = readObject.myStringBuilder.ToString();
                    Console.WriteLine(String.Format("Read {0} byte from socket" +
                                       "data = {1} ", strContent.Length, strContent));
                }
                readSocket.Close();
            }
        }
    }
}
*/
