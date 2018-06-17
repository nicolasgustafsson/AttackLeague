using AttackLeague.Utility.Network.Messages;
using System;

namespace AttackLeague.AttackLeague.Player
{
    [Serializable]
    class DebugDiedMessage : BaseMessage
    {
        public int PlayerIndex;
    }
}
