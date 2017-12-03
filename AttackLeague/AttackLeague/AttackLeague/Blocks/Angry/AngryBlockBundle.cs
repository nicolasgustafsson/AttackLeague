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
        private GridBundle myGridBundle;

        bool myIsDisintegrating = false;

        public int Index { get; private set; }
        // gridbundle?

        public AngryBlockBundle(GridBundle aGridBundle)
        {
            myGridBundle = aGridBundle;
            myAngryBlocks = new List<AngryBlock>();
        }

        public void AddBlock(AngryBlock aBlock)
        {
            myAngryBlocks.Add(aBlock);
        }

        public void Update(float aGameSpeed)
        {

        }

        public void SetIndex(int aIndex)
        {
            Index = aIndex;
        }

        public void OnHitByMatch()
        {
            myIsDisintegrating = true;

            foreach (var block in myAngryBlocks)
            {
                block.Freeze();
            }
        }

        public bool CanWeFall()
        {
            foreach (var block in myAngryBlocks)
            {
                if (block.AllowsFalling() == false)
                    return false;
            }
            return true;
        }

        public void UpdateFallingStatus()
        {
            foreach (var block in myAngryBlocks)
            {
                block.SetFriendlyAwareness(false);
            }
            bool canFall = CanWeFall();
            foreach (var block in myAngryBlocks)
            {
                block.SetCanFall(canFall);
                block.SetFriendlyAwareness(true);
            }
        }
    }
}
