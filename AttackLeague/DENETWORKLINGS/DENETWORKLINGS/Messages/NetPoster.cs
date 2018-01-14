using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS.Messages
{
    class NetPoster
    {
        static public NetPoster Instance;

        private IConnection myPostingConnection;

        public NetPoster(IConnection aConnection)
        {
            myPostingConnection = aConnection;
        }

        public void PostMessage<T>(T aMessage)
        {
            myPostingConnection.WriteMessage<T>(aMessage);
        }
    }
}
