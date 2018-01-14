using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks
{
    public delegate void BlockDelegate(AbstractBlock aBlock);
    public delegate void FinishedDelegate(List<AbstractBlock> aPreviousBlocks);

    class BlockTimedIterator
    {
        public BlockTimedIterator(BlockDelegate aFunctionToRunOnBlocks, List<AbstractBlock> aBlocks, float aTimeBetweenIterations, FinishedDelegate aFinnishFunction = null)
        {
            myFunctionToRun = aFunctionToRunOnBlocks;
            myBlocks = aBlocks;
            myFramesBetweenIterations = aTimeBetweenIterations;
            myCurrentCooldown = myFramesBetweenIterations;
            myFunctionToFinnish = aFinnishFunction;

            myCurrentIndex = 0;
        }

        public void Update()
        {
            if (myBlocks == null) // we are really done
                return;

            --myCurrentCooldown;

            if (IsFinished() && myBlocks != null) // delay finish with one timed iteration, instead of calling directly after Iterate()
            {
                myFunctionToFinnish(myBlocks);
                myBlocks.Clear();
                myBlocks = null;
                return;
            }

            while (myCurrentCooldown < 0)
            {
                myCurrentCooldown += myFramesBetweenIterations;
                Iterate();
            }
        }

        void Iterate()
        {
            myFunctionToRun(myBlocks[myCurrentIndex]);
            myCurrentIndex++;
        }

        public bool IsFinished()
        {
            return (myBlocks == null || myBlocks.Count <= myCurrentIndex) && myCurrentCooldown < 0;
        }

        List<AbstractBlock> myBlocks;
        BlockDelegate myFunctionToRun;
        FinishedDelegate myFunctionToFinnish;
        float myFramesBetweenIterations;
        float myCurrentCooldown;
        int myCurrentIndex;
    }
}
