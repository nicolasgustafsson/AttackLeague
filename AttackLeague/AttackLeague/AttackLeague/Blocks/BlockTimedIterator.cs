using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks
{
    delegate void BlockDelegate(AbstractBlock aBlock);

    class BlockTimedIterator
    {
        BlockTimedIterator(BlockDelegate aFunctionToRunOnBlocks, List<AbstractBlock> aBlocks, float aTimeBetweenIterations)
        {
            myFunctionToRun = aFunctionToRunOnBlocks;
            myBlocks = aBlocks;
            myTimeBetweenIterations = aTimeBetweenIterations;
            myCurrentCooldown = myTimeBetweenIterations;

            myCurrentIndex = 0;
        }

        void Update(float aDeltaTime)
        {
            if (IsFinished())
                return;

            myCurrentCooldown -= aDeltaTime;

            if (myCurrentCooldown < 0)
            {
                myCurrentCooldown += myTimeBetweenIterations;

                Iterate();
            }
        }

        void Iterate()
        {
            myFunctionToRun(myBlocks[myCurrentIndex]);
            myCurrentIndex++;
        }

        bool IsFinished()
        {
            return myCurrentIndex <= myBlocks.Count;
        }

        List<AbstractBlock> myBlocks;
        BlockDelegate myFunctionToRun;
        float myTimeBetweenIterations;
        float myCurrentCooldown;
        int myCurrentIndex;
    }
}
