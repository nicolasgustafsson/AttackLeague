using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility
{
    //Helper class for checking current frame in game(used for recordings)
    //static class is probably wrong approach tho
    static class FrameCounter
    {
        public static void ResetFrames()
        {
            myCurrentFrame = 0;
        }
        public static void IncrementFrameCount()
        {
            myCurrentFrame++;
        }
        public static int GetCurrentFrame()
        {
            return myCurrentFrame;
        }

        private static int myCurrentFrame = 0;
    }
}
