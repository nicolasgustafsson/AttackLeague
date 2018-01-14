using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS.Messages
{
    interface ISubscriber
    {
        void ReceiveMessageInternal(BaseMessage message);
    }

    abstract class ISubscriber<T> : ISubscriber where T : BaseMessage
    {
        public void ReceiveMessageInternal(BaseMessage message)
        {
            ReceiveMessage(message as T);
        }

        protected abstract void ReceiveMessage(T aMessage);
    }
}
