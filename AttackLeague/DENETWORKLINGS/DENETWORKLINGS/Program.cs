using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<NetPeer> netConnections = new List<NetPeer>();
            NetHost listener = new NetHost();


            List<string> consoleCommand = new List<string>();
            consoleCommand.Add("Hello");

            while(consoleCommand.Count == 0 || consoleCommand[0] != "exit")
            {
                if (consoleCommand.Count == 0)
                {
                    consoleCommand = Console.ReadLine().Split(' ').ToList();
                    continue;
                }

                string firstWord = consoleCommand[0];

                switch (firstWord)
                {
                    case "c":
                        NetPeer newConnection = new NetPeer();

                        if (consoleCommand.Count < 2)
                        {
                            newConnection.StartConnection("localhost");

                        }
                        else
                        {
                            newConnection.StartConnection(consoleCommand[1]);
                        }

                        netConnections.Add(newConnection);
                        break;

                    case "l":
                        listener.StartListen();
                        break;

                    case "write":
                        if (netConnections.Count > 0)
                        {
                            netConnections[0].Write(string.Join(" ", consoleCommand.Skip(1)));
                        }
                        else
                        {
                            listener.PrintToAllClients(string.Join(" ", consoleCommand.Skip(1)));
                        }
                        break;
                }

                consoleCommand = Console.ReadLine().Split(' ').ToList();
            }
        }
    }
}
