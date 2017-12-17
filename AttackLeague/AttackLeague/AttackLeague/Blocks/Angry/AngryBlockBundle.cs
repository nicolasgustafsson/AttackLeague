using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
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
        private List<FrozenBlock> myFrozenBlocks;

        private GridBundle myGridBundle;

        private bool myIsContaminated = false;

        public int Index { get; private set; }
        // gridbundle?

        public AngryBlockBundle(GridBundle aGridBundle)
        {
            myGridBundle = aGridBundle;
            myAngryBlocks = new List<AngryBlock>();
            myFrozenBlocks = new List<FrozenBlock>();
        }

        public void OnDestroy()
        {
            myGridBundle = null;
            myAngryBlocks.Clear();
            myFrozenBlocks.Clear();
        }

        public void AddBlock(AngryBlock aBlock)
        {
            myAngryBlocks.Add(aBlock);
        }

        public void Initialize(int aIndex)
        {
            myAngryBlocks = myAngryBlocks.OrderByDescending(block =>
            {
                return block.GetPosition().Y * -100 + block.GetPosition().X; 
            }).ToList();

            Index = aIndex;
        }

        public void Update(float aGameSpeed)
        {
        }

        public void HandleBop(AbstractBlock aBlock)
        {
            AngryBlock angryBlock = aBlock as AngryBlock;
            if (angryBlock.IsContaminated())
            {
                angryBlock.ResolveContamination();
            }
            if (angryBlock.IsDead())
            {
                myFrozenBlocks.Add(myGridBundle.Generator.GenerateFrozenBlockAtPosition(angryBlock.GetPosition()));
                myAngryBlocks.Remove(angryBlock);
            }
        }

        public void FinishDisintegrating()
        {
            if (myIsContaminated == false)
                return;

            foreach(FrozenBlock frozenBlock in myFrozenBlocks)
            {
                myGridBundle.Generator.ConvertFrozenBlockToFallingOrColorBlock(frozenBlock);
            }
            foreach(AngryBlock angryBlock in myAngryBlocks)
            {
                angryBlock.UnFreeze();
            }
            myFrozenBlocks.Clear();

            myIsContaminated = false;

            //if we are dead, remove from gridbehavior
        }

        public void OnHitByMatch()
        {
            myIsContaminated = true;
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

        public bool IsDed()
        {
            return myAngryBlocks.Count <= 0 && myFrozenBlocks.Count <= 0;
        }

        public bool IsContaminated()
        {
            return myIsContaminated;
        }

        public List<AngryBlock> GetBlocksForIteratingPurposes()
        {
            return myAngryBlocks;
        }
    }
}
