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

    abstract class Subscriber<T> : ISubscriber where T : BaseMessage
    {
        protected Subscriber()
        {
            Subscribe();
        }

        protected void Subscribe()
        {
            NetPostMaster.Master.Subscribe<T>(this);
        }

        protected void Unsubscribe()
        {
            NetPostMaster.Master.Unsubscribe<T>(this);
        }

        public void ReceiveMessageInternal(BaseMessage message)
        {
            ReceiveMessage(message as T);
        }

        protected abstract void ReceiveMessage(T aMessage);
    }
}
