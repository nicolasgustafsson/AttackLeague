using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility
{
    class Networkton
    {
        static public void Create()
        {
            Debug.Assert(ourInstance == null, "Already created networkings");
            ourInstance = new Networkton();
        }

        static public void Destroy()
        {
            Debug.Assert(ourInstance != null, "Not created networkings");
            ourInstance = null;
        }

        public static Networkton ourInstance {get; private set;}

        // class
        private TcpClient myClient;

        private Networkton()
        {
            Int32 port = 13000;
            myClient = new TcpClient("127.0.0.1", port);
        }

        public void SendMessage(string aMessage)
        {
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(aMessage);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                using (NetworkStream stream = myClient.GetStream())
                {
                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", aMessage);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);

                    stream.Close();
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

    }
}
