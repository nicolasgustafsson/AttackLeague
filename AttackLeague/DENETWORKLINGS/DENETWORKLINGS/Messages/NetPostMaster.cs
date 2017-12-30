using DENETWORKLINGS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class NetPostMaster
    {
        public void Subscribe<T>(ISubscriber aSubscriber) where T : BaseMessage
        {
            if (!mySubscribers.ContainsKey(typeof(T)))
            {
                mySubscribers.Add(typeof(T), new List<ISubscriber>());
            }

            mySubscribers[typeof(T)].Add(aSubscriber);
        }

        public void AddMessageToQueue(BaseMessage aMessage) 
        {
            if (!myQueuedMessages.ContainsKey(aMessage.GetType()))
            {
                myQueuedMessages.Add(aMessage.GetType(), new List<BaseMessage>());
            }

            myQueuedMessages[aMessage.GetType()].Add(aMessage);
        }

        public void ResolveMessages()
        {
            foreach (var keyAndValue in myQueuedMessages)
            {
                List<BaseMessage> tenpy = keyAndValue.Value;

                MethodInfo methodInfo = typeof(NetPostMaster).GetMethod("PublishMessage");

                MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(keyAndValue.Key);

                foreach (BaseMessage message in tenpy)
                {
                    genericMethodInfo.Invoke(this, new object[] { message });
                }
            }
        }

        void PublishMessage<T>(BaseMessage aMessage) where T : BaseMessage
        {
            foreach(ISubscriber baseSubscriber in mySubscribers[typeof(T)] )
            {
                ISubscriber<T> subscriber = baseSubscriber as ISubscriber<T>;

                subscriber.ReceiveMessage(aMessage as T);
            }
        }

        Dictionary<Type, List<ISubscriber>> mySubscribers = new Dictionary<Type, List<ISubscriber>>();
        Dictionary<Type, List<BaseMessage>> myQueuedMessages = new Dictionary<Type, List<BaseMessage>>();
    }
}
