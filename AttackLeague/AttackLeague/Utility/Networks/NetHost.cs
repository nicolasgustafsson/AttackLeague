﻿using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Network
{
    class NetHost : IConnection
    {
        TcpListener myListener = new TcpListener(IPAddress.Any, 32323);
        List<NetPeer> myClients = new List<NetPeer>();

        public void StartListen()
        {
            myListener.AllowNatTraversal(true);

            myListener.Start();

            Thread ClientAcceptionings = new Thread(AcceptClientLoop);
            ClientAcceptionings.Name = "ListenForNewClients";
            ClientAcceptionings.Start();
        }

        public bool IsConnected()
        {
            return myClients.Count > 0;
        }

        public void WriteMessage<T>(T aStuff)
        {
            foreach (NetPeer client in myClients)
            {
                client.WriteMessage(aStuff);
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

                NetPeer peer = new NetPeer();
                peer.SetClient(client);
                peer.InitializeConnection();

                lock (myClients)
                {
                    myClients.Add(peer);
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
