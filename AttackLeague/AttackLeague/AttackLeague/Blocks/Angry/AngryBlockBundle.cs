using AttackLeague.AttackLeague.Grid;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Angry
{
    public class AngryBlockBundle
    {
        private List<AngryBlock> myAngryBlocks;
        public int Index { get; private set; }
        // gridbundle?

        public AngryBlockBundle()
        {
        }

        public void AddBlock(AngryBlock aBlock)
        {
            myAngryBlocks.Add(aBlock);
        }

        void Update()
        {
            bool canFall = true;
            foreach (var item in myAngryBlocks)
            {
                if (item.CanFall == false)
                {
                    canFall = false;
                    break;
                }
            }

            if (canFall)
            {
                // they fall. All fall. in myBlocks. Convert them or do a state or something.
            }
        }

        public void SetIndex(int aIndex)
        {
            Index = aIndex;
        }


        // we need something to handle falling, 
        // since we loop through all blocks in order from bottom to up and stuff we probably want to check each block first in some way, 
        //to whether or not it will be allowed to fall, in order to later check/set the whole angry block bundle to fall
        // this is the main thingy which will be hard to figure out, but we will do something nice!  :D =)

        // if angryblock below me with same angry index I can fall
        // if emptyblock below me, I may fall, otherwise I can't fall
        // if one can't fall, nobody can!
    }
}
