﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.AttackLeague.Player;
using AttackLeague.AttackLeague.Blocks.Angry;

namespace AttackLeague.AttackLeague.GameInfo
{
    static class GameInfo
    {
        //contains GameMode which has rules n studff
        static public int myPlayerCount { get { return myPlayers.Count; }  }

        static public Vector2 myScreenSize;
        static public List<Player.Player> myPlayers = new List<Player.Player>();

        static public void SetAutomaticAttackOrder()
        {
            for (int iMe = 0; iMe < myPlayers.Count; iMe++)
            {
                for (int iEnemy = 0; iEnemy < myPlayers.Count; iEnemy++)
                {
                    if (iMe == iEnemy)
                        continue;
                    int nextNumber = (iEnemy + 1);// % myPlayers.Count;
                    int wrappedNumber = nextNumber >= myPlayers.Count ? 0 : nextNumber;
                    myPlayers[iMe].myAttackOrder.Add(nextNumber);
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
