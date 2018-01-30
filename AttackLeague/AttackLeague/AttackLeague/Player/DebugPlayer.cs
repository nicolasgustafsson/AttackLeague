using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility.Network;
using AttackLeague.Utility.Network.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class DebugPlayer : Player
    {
        private Subscriber<HardcodedMessage> myHardCodedSubscriber;

        public DebugPlayer()
            :base(new DebugPlayerInfo())
        {
            myHardCodedSubscriber = new Subscriber<HardcodedMessage>(ReceiveHardCodedMassage, true);
        }

        public override void Update()
        {
            base.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("ConnectToNicos"))
                NetworkConnectAsPeer(NetPeer.NicosIP);

            if (myPlayerInfo.myMappedActions.ActionIsActive("ConnectToYlf"))
                NetworkConnectAsHost(NetPeer.YlfsIP);

            if (myPlayerInfo.myMappedActions.ActionIsActive("SendHardCodedMessage")) //M
                SendHardCodedMessage();

            if (myPlayerInfo.myMappedActions.ActionIsActive("RandomizeGrid"))
                myGridBundle.Generator.GenerateGrid();

            if (myPlayerInfo.myMappedActions.ActionIsActive("Pause"))
                myIsPaused = !myIsPaused;

            if (myPlayerInfo.myMappedActions.ActionIsActive("StepOnce"))
                myGridBundle.Behavior.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("IncreaseGameSpeed"))
                myGridBundle.Behavior.AddGameSpeed(0.5f);

            if (myPlayerInfo.myMappedActions.ActionIsActive("SpawnAngryBundle"))
            {
                Point angrySize = new Point(6, 3);
                Point position = new Point(0, myGridBundle.Container.GetInitialHeight() + angrySize.Y);
                AngryBlockBundle angryBundle = myGridBundle.Generator.CreateAngryBlockBundleAtPosition(position, angrySize);
                myGridBundle.Behavior.AddAngryBundle(angryBundle);
            }
        }

        private void NetworkConnectAsPeer(string aIP)
        {
            NetPeer newConnection = new NetPeer();
            newConnection.StartConnection(aIP);
            NetPoster.Instance.Connection = newConnection;
        }

        private void NetworkConnectAsHost(string aIP)
        {
            NetHost host = new NetHost();
            host.StartListen();
            NetPoster.Instance.Connection = host;
        }

        private void SendHardCodedMessage()
        {
            NetPoster.Instance.PostMessage(new HardcodedMessage { myHardcoding = "Hellos" });
        }

        private void ReceiveHardCodedMassage(HardcodedMessage aMessagings)
        {
            Console.WriteLine(aMessagings.myHardcoding);
        }
    }
}
