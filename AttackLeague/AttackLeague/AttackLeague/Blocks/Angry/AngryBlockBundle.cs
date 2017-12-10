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

        private float myBaseDisintegratingTimer = 30.0f;
        private float myCurrentDisintegratingTimer = 0.0f;
        private int myDisintegratingIndex = 0;

        private bool myIsContaminated = false;

        public int Index { get; private set; }
        // gridbundle?

        public AngryBlockBundle(GridBundle aGridBundle)
        {
            myGridBundle = aGridBundle;
            myAngryBlocks = new List<AngryBlock>();
            myFrozenBlocks = new List<FrozenBlock>();
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
            if (myIsContaminated)
            {
                UpdateDisintegrate(aGameSpeed);
            }
        }

        private bool UpdateDisintegrate(float aGameSpeed)
        {
            myCurrentDisintegratingTimer -= aGameSpeed;

            if (myCurrentDisintegratingTimer <= 0.0f)
            {
                if (myDisintegratingIndex >= myAngryBlocks.Count)
                {
                    //FinishDisintegrating();
                    return true;
                }

                AngryBlock angryBlock = myAngryBlocks[myDisintegratingIndex];
                if (angryBlock.DiedFromContamination())
                {
                    myFrozenBlocks.Add(myGridBundle.Generator.GenerateFrozenBlockAtPosition(angryBlock.GetPosition()));

                    myAngryBlocks.RemoveAt(myDisintegratingIndex);
                }
                else
                {
                    myDisintegratingIndex++;
                }

                myCurrentDisintegratingTimer = myBaseDisintegratingTimer;
            }
            return false;
        }

        public void FinishDisintegrating()
        {
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
            myDisintegratingIndex = 0;

            //if we are dead, remove from gridbehavior
        }

        public void OnHitByMatch()
        {
            myIsContaminated = true;
            myCurrentDisintegratingTimer = 120.0f;
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
