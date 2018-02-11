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
        private Subscriber<AngryBlockSpawnMessage> myAngryBlockSubscriber;

        public NetPostingPlayer(PlayerInfo aPlayerInfo)
            : base(aPlayerInfo)
        {
            myAngryBlockSubscriber = new Subscriber<AngryBlockSpawnMessage>(OnAngryAttackReceived, true);
        }

        private void OnAngryAttackReceived(AngryBlockSpawnMessage aMessage)
        {
            if (aMessage.myAttackedPlayer == myPlayerInfo.myPlayerIndex)
            {
                Console.WriteLine("Received attack on: " + myElapsedFrames);
                myQueuedAngryBlocks.Add(aMessage.myAngryInfo);
                NetPoster.Instance.PostMessage(new AngryBlockConfirmMessage(myPlayerInfo.myPlayerIndex));
            }
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
