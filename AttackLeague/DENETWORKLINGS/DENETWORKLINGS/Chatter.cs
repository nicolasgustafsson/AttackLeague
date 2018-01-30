using DENETWORKLINGS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    class Chatter : Subscriber<ChatMessage>
    {
        protected override void ReceiveMessage(ChatMessage aMessage)
        {
            Console.WriteLine(aMessage.myMessage);
        }
    }
}
