using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DENETWORKLINGS.Messages;

namespace DENETWORKLINGS
{
    class PrettyClass : ISubscriber<PrettyMessage>
    {
        public void SendMessage(NetPostMaster postMastery)
        {
            PrettyMessage msg = new PrettyMessage();
            msg.thingy = "TOTEZ CREATED MSG";
            msg.apa = 32;
            postMastery.AddMessageToQueue(msg);
        }

        protected override void ReceiveMessage(PrettyMessage aMessage)
        {
            Console.WriteLine("{0}, {1}, {2}", aMessage.thingy, aMessage.apa, aMessage.derp);
            int apa = 0;
            apa++;
        }
    }
}
