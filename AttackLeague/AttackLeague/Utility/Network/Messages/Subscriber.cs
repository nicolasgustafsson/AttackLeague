using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Network.Messages
{
    interface ISubscriber
    {
        void ReceiveMessageInternal(BaseMessage message);
    }

    delegate void OnReceiveMessage<T>(T aMessage);

    class Subscriber<T> : ISubscriber where T : BaseMessage
    {
        public Subscriber(OnReceiveMessage<T> aCallback, bool aStartSubscribing)
        {
            myOnReceiveMessage = aCallback;
            if (aStartSubscribing == true)
                Subscribe();
        }

        public void Subscribe()
        {
            NetPostMaster.Master.Subscribe<T>(this);
        }

        public void Unsubscribe()
        {
            NetPostMaster.Master.Unsubscribe<T>(this);
        }

        public void ReceiveMessageInternal(BaseMessage message)
        {
            myOnReceiveMessage(message as T);
        }

        private OnReceiveMessage<T> myOnReceiveMessage;
    }
}
