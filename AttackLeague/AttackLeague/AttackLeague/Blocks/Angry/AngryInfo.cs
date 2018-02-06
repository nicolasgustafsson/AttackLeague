using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Angry
{
    public enum EAngryType
    {
        Normal,
        Iron,
        Rainbow
    }

    public class AngryInfo
    {
        public AngryInfo(Point aSize, int aSendingPlayer, EAngryType aAngryType)
        {
            mySize = aSize;
            mySendingPlayer = aSendingPlayer;
            myAngryType = aAngryType;
        }

        public Point mySize;
        public int mySendingPlayer;         // used to acquire angry block cosmetics from other player
        public EAngryType myAngryType;
    }
}
