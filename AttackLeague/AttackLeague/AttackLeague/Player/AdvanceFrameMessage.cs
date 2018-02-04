using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class AdvanceFrameMessage : BaseMessage
    {
        public IEnumerable<string> Actions;
        public int PlayerIndex;
        public int FrameIndex;
    }
}
