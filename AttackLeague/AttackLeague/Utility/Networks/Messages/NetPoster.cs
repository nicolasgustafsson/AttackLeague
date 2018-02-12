using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Network.Messages
{
    class NetPoster
    {
        static public NetPoster Instance = new NetPoster();

        private IConnection myConnection;
        public IConnection Connection
        {
            get { return myConnection; }
            set
            {
                Debug.Assert(myConnection == null);
                myConnection = value;
            }
        }

        public NetPoster()
        {
        }

        public void PostMessage<T>(T aMessage) where T : BaseMessage
        {
            Debug.Assert(Connection != null);
            Connection.WriteMessage<T>(aMessage);
        }
    }
}
