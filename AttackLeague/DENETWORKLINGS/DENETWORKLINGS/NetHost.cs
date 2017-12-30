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
    class NetHost
    {
        TcpListener myListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 32323);
        List<TcpClient> myClients = new List<TcpClient>();

        public void StartListen()
        {
            myListener.Start();

            Thread ClientAcceptionings = new Thread(AcceptClientLoop);
            ClientAcceptionings.Name = "ListenForNewClients";
            ClientAcceptionings.Start();
        }

        public void PrintToAllClients(string aStuff)
        {
            foreach (TcpClient client in myClients)
            {
                NetworkStream netStream = client.GetStream();
                BinaryWriter binaryWriter = new BinaryWriter(netStream);

                binaryWriter.Write(aStuff);
            }
        }

        private void AcceptClientLoop()
        {
            while(true)
            {
                AcceptClient();
            }
        }

        private void AcceptClient()
        {
            try
            {
                TcpClient client = myListener.AcceptTcpClient();

                NetworkStream netStream = client.GetStream();
                BinaryWriter binaryWriter = new BinaryWriter(netStream);

                binaryWriter.Write("Hey you guuuuys");
            


                lock(myClients)
                {
                    myClients.Add(client);
                }


                Console.WriteLine("Accepted Client");
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not accept Tcp Client!");
                Console.WriteLine(e);
            }
        }
    }
}
