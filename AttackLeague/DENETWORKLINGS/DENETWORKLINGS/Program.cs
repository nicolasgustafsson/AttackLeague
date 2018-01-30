using DENETWORKLINGS.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class Program
    {
        /*
         You return and dont remember anything.
         This is the run-down!

            NetPoster is static and sends messages by NetPeer/NetHost, which was registered upon its creation.
            All things in game which sends messages call NetPoster! (We could do inheritance from a BasePoster which calls NetPoster)

            Messages inherit from BaseMessage and everything magically works.

            On the receiving end is the NetPostMaster. It gets the message from the listening connection (NetHost/NetPeer) and
            distributes the message among its surbscribers.
            
            What you need to do now is move it into the project. And test NetPoster and figure out if you wanna edit anything in the flow. 
             */

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            NetPostMaster posty = new NetPostMaster();
            IConnection myCurrentConnection = null;
            NetPoster.Instance = new NetPoster(myCurrentConnection);
            
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
                        Debug.Assert(myCurrentConnection == null);
                        NetPeer newConnection = new NetPeer();

                        if (consoleCommand.Count < 2)
                        {
                            newConnection.StartConnection("localhost");
                        }
                        else
                        {
                            newConnection.StartConnection(consoleCommand[1]);
                        }

                        myCurrentConnection = newConnection;
                        break;

                    case "l":
                        Debug.Assert(myCurrentConnection == null);

                        NetHost host = new NetHost();
                        host.StartListen();

                        myCurrentConnection = host;
                        break;

                    case "pretty":
                        NetPoster.Instance.PostMessage(new PrettyMessage());
                        break;
                }

                consoleCommand = Console.ReadLine().Split(' ').ToList();
            }
        }
    }
}
