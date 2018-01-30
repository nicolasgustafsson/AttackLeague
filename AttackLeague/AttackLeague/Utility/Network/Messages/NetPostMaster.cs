using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
LOOK IN DEBUGPLAYER
SEND HARDCODED MESSAGES ON M
APPLY THIS LOGIC ON GAEM (Maek networked player?)
EZ NETWORKS
GG
Setp 3 prophet
*/

namespace AttackLeague.Utility.Network
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

        public void Unsubscribe<T>(ISubscriber aSubscriber)
        {
            if (!mySubscribers.ContainsKey(typeof(T)))
            {
                return;
            }

            mySubscribers[typeof(T)].Remove(aSubscriber);
        }

        public void AddMessageToQueue(BaseMessage aMessage) 
        {
            lock (myQueuedMessages)
            {
                if (!myQueuedMessages.ContainsKey(aMessage.GetType()))
                {
                    myQueuedMessages.Add(aMessage.GetType(), new List<BaseMessage>());
                }

                myQueuedMessages[aMessage.GetType()].Add(aMessage);
            }
        }

        public void ResolveMessages()
        {
            foreach (var keyAndValue in myQueuedMessages)
            {
                List<BaseMessage> tenpy = keyAndValue.Value;

                foreach (BaseMessage message in tenpy)
                {
                    foreach (ISubscriber baseSubscriber in mySubscribers[keyAndValue.Key])
                    {
                        baseSubscriber.ReceiveMessageInternal(message);
                    }
                }
            }

            lock(myQueuedMessages)
            {
                myQueuedMessages.Clear();
            }
        }

        Dictionary<Type, List<ISubscriber>> mySubscribers = new Dictionary<Type, List<ISubscriber>>();
        Dictionary<Type, List<BaseMessage>> myQueuedMessages = new Dictionary<Type, List<BaseMessage>>();

        public static NetPostMaster Master = new NetPostMaster();
    }
}
