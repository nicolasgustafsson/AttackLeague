using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility.Network.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class RemotePlayer : Player
    {
        private Subscriber<AdvanceFrameMessage> myAdvanceFrameSubscriber;
        private Subscriber<AngryBlockConfirmMessage> myAngryBlockSubscriber;
        private List<AngryInfo> myQueuedNetAngries;

        public RemotePlayer(PlayerInfo aPlayerInfo)
            :base(aPlayerInfo)
        {
            myQueuedNetAngries = new List<AngryInfo>();
            myAdvanceFrameSubscriber = new Subscriber<AdvanceFrameMessage>(OnFrameMessageReceived, true);
            myAngryBlockSubscriber = new Subscriber<AngryBlockConfirmMessage>(OnAngryAttackReceived, true);
        }

        public override void Update()
        {
        }

        private void OnFrameMessageReceived(AdvanceFrameMessage aMessage)
        {
            myLastAdvancedFrame = aMessage;
            base.Update();
        }

        private void OnAngryAttackReceived(AngryBlockConfirmMessage aMessage)
        {
            if (aMessage.myPlayerIndex == myPlayerInfo.myPlayerIndex)
            {
                Debug.Assert(myQueuedNetAngries.Count > 0);
                Console.WriteLine("Confirmed angry block on: " +  myElapsedFrames);
                myQueuedAngryBlocks.Add(myQueuedNetAngries[0]);
                myQueuedNetAngries.RemoveAt(0);
            }
        }

        public override void ReceiveAttack(AngryInfo aAngryInfo)
        {
            myQueuedNetAngries.Add(aAngryInfo);
            NetPoster.Instance.PostMessage(new AngryBlockSpawnMessage(aAngryInfo, myPlayerInfo.myPlayerIndex));
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

        protected override void ResolveAngryQueue() 
        {
            /*
             nicos => angryblock på Ylf
             syns på Nicos skärm
             Ylf får inget på sin
             
             
             */


            base.ResolveAngryQueue();
        }

        AdvanceFrameMessage myLastAdvancedFrame;
    }
}
