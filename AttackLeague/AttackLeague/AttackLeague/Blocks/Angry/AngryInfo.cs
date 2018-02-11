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

    [Serializable]
    public class AngryInfo
    {
        public AngryInfo(Point aSize, int aSendingPlayer, EAngryType aAngryType)
        {
            mySizeX = aSize.X;
            mySizeY = aSize.Y;
            mySendingPlayer = aSendingPlayer;
            myAngryType = aAngryType;
        }

        public Point GetSize()
        {
            return new Point(mySizeX, mySizeY);
        }

        public int mySizeX;
        public int mySizeY;
        public int mySendingPlayer;         // used to acquire angry block cosmetics from other player
        public EAngryType myAngryType;
    }
}
