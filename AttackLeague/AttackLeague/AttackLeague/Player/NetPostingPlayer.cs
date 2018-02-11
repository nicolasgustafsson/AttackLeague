using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class NetPostingPlayer : Player
    {
        public NetPostingPlayer(PlayerInfo aPlayerInfo)
            : base(aPlayerInfo)
        {
        }

        public override void ReceiveAttack(AngryInfo aAngryInfo)
        {
            myQueuedAngryBlocks.Add(aAngryInfo);
            NetPoster.Instance.PostMessage(new AngryBlockMessage(aAngryInfo, myPlayerInfo.myPlayerIndex));
        }

        public override void Update()
        {
            base.Update();

            AdvanceFrameMessage frameMessage = new AdvanceFrameMessage();
            frameMessage.Actions = myPlayerInfo.myMappedActions.GetActiveActions().ToList();
            frameMessage.PlayerIndex = myPlayerInfo.myPlayerIndex;
            frameMessage.FrameIndex = myElapsedFrames - 1;
            NetPoster.Instance.PostMessage(frameMessage);
        }
    }
}
