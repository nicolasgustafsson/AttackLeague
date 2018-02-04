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
            : base(aPlayerInfo) { }

        public override void Update()
        {
            base.Update();

            AdvanceFrameMessage frameMessage = new AdvanceFrameMessage();
            frameMessage.Actions = myPlayerInfo.myMappedActions.GetActiveActions().ToList();
            //bleh.FrameNumber = x
            frameMessage.PlayerIndex = myPlayerInfo.myPlayerIndex;
            Utility.Network.Messages.NetPoster.Instance.PostMessage<AdvanceFrameMessage>(frameMessage);
        }
    }
}
