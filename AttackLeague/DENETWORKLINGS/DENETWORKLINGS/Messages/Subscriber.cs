using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS.Messages
{
    interface ISubscriber
    {
        
    }

    interface ISubscriber<T> : ISubscriber where T : BaseMessage
    {
        void ReceiveMessage(T aMessage);
    }
}
