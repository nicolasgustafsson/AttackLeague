using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.AttackLeague.Player;
using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility.Network.Messages;
using System.Diagnostics;

namespace AttackLeague.AttackLeague.GameInfo
{
    static class GameInfo
    {
        //contains GameMode which has rules n studff
        static public int myPlayerCount { get { return myPlayers.Count; } }

        static public Vector2 myScreenSize;
        static public List<Player.Player> myPlayers = new List<Player.Player>();
        public delegate void SetMouseFunction(bool aVisibility);
        static public SetMouseFunction myMouseFunction { get; set; }

        static public void SetAutomaticAttackOrder()
        {
            for (int iMe = 0; iMe < myPlayers.Count; iMe++)
            {
                int iEnemy = iMe + 1;

                while(iEnemy != iMe)
                {
                    if (iEnemy >= myPlayerCount)
                        iEnemy = 0;

                    int nextNumber = (iEnemy);// % myPlayers.Count;
                    int wrappedNumber = nextNumber >= myPlayers.Count ? 0 : nextNumber;
                    myPlayers[iMe].myAttackOrder.Add(nextNumber);
                    if (myPlayers[iMe].myAttackOrder.Count >= myPlayers.Count - 1)
                        break;

                    iEnemy++;
                }
            }
        }

        static public void SendMyRegards(AngryInfo aAngryInfo)
        {
            foreach (var toAttackIndex in myPlayers[aAngryInfo.mySendingPlayer].myAttackOrder)
            {
                if (myPlayers[toAttackIndex].CanBeAttacked() == true)
                {
                    myPlayers[toAttackIndex].ReceiveAttack(aAngryInfo);
                    break;
                }
            }
        }

        static public void SetMouseVisibility(bool aVisibility)
        {
            Debug.Assert(myMouseFunction != null);
            myMouseFunction(aVisibility);
        }

        // static List<Friend>

        /*
         internal class Friend
         {
            string Name
            string Greeting <eg. "I'M THE VERY BEST!" etc>
            string? IP
            info about previous win/losses against this person >:D
         }
        */
    }
}
