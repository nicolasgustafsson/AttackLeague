using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Angry
{
    [Serializable]
    class AngryBlockSpawnMessage : BaseMessage
    {
        public AngryBlockSpawnMessage(AngryInfo aAngryInfo, int aToAttack )
        {
            myAngryInfo = aAngryInfo;
            myAttackedPlayer = aToAttack;
        }
        public AngryInfo myAngryInfo;
        public int myAttackedPlayer;
    }

    [Serializable]
    class AngryBlockConfirmMessage : BaseMessage
    {
        public AngryBlockConfirmMessage(int aToAttack)
        {
            myPlayerIndex = aToAttack;
        }
        public int myPlayerIndex;
    }

}
