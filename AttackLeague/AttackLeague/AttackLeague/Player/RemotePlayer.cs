using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class RemotePlayer : Player
    {
        private Subscriber<AdvanceFrameMessage> myAdvanceFrameSubscriber;

        public RemotePlayer(PlayerInfo aPlayerInfo)
            :base(aPlayerInfo)
        {
            myAdvanceFrameSubscriber = new Subscriber<AdvanceFrameMessage>(OnFrameMessageReceived, true);
        }

        public override void Update()
        {
        }

        private void OnFrameMessageReceived(AdvanceFrameMessage aMessage)
        {
            // TODO if aMessage.playerIndex == myIndex
            myLastAdvancedFrame = aMessage;
            Update();
        }

        protected override void HandleActions()
        {
            foreach (var Action in myLastAdvancedFrame.Actions)
            {
                if (myLastAdvancedFrame.Actions.Contains(ActionList.SwapBlocks))
                    myGridBundle.Behavior.SwapBlocksRight(myPosition);

                if (myLastAdvancedFrame.Actions.Contains(ActionList.RaiseBlocks))
                    myGridBundle.Behavior.SetIsRaisingBlocks();
            }
        }

        protected override void HandleMovement()
        {
            if (myLastAdvancedFrame.Actions.Contains(ActionList.MoveRight))
            {
                if (myPosition.X < myGridBundle.Container.GetInitialWidth() - 2)
                    myPosition.X += 1;
            }
            if (myLastAdvancedFrame.Actions.Contains(ActionList.MoveLeft))
            {
                if (myPosition.X > 0)
                    myPosition.X -= 1;
            }
            if (myLastAdvancedFrame.Actions.Contains(ActionList.MoveUp))
            {
                if (myPosition.Y < myGridBundle.Container.GetInitialHeight() - (CanBeAtTop() ? 0.0f : 1.0f))
                    myPosition.Y += 1;
            }
            if (myLastAdvancedFrame.Actions.Contains(ActionList.MoveDown))
            {
                if (myPosition.Y > 1)
                    myPosition.Y -= 1;
            }
        }

        AdvanceFrameMessage myLastAdvancedFrame;
    }
}
