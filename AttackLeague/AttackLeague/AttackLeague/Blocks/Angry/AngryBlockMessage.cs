using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Angry
{
    [Serializable]
    class AngryBlockMessage : BaseMessage
    {
        public AngryBlockMessage(AngryInfo aAngryInfo, int aToAttack )
        {
            myAngryInfo = aAngryInfo;
            myAttackedPlayer = aToAttack;
        }
        public AngryInfo myAngryInfo;
        public int myAttackedPlayer;
    }
}
