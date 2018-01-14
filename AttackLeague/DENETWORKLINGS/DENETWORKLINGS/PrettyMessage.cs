using DENETWORKLINGS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENETWORKLINGS
{
    [Serializable]
    class PrettyMessage : BaseMessage
    {
        public int derp = 5;
        public string thingy = "Hello";
        public int apa = 27;
    }
}
