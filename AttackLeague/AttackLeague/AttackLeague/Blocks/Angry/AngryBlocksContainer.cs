using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Angry
{
    class AngryBlocksContainer
    {
        // hur vi tänkte
        //AngryBlock v
        //AngryContainer            List<AngryParts> allTheParts
        //AngryParts : ColorBlock   block
        //ShouldDecimate        block->AngryContainer.ShouldEvaporate foreach block in allTheParts

        public AngryBlocksContainer(/*combo from your friendly enemy or other stuff*/)
        {
            // generate some blocks
        }

        private List<AngryPartBlock> myParts;

        //bool CanFall()

        //Evaporate / frågan är om man vill skapa block här eller skicka infon till griden etc? 

        // Angry blocks touching other angry blocks should also evaporate
        // Vi behöver loopa alla Angrycontainers & se om något av någons block är bredvid något av våra block
        // och för varje det stämmer på, måste vi loopa alla angry blocks (som inte börjat evaporate) och se om dem är bredvid oss
        

        // BlockFactory & Secretary
        // Creates block According to specifications (special block allowed) (which doesn't combo if you don't want to)
        // Has all the blocks
        // Creates Angry blocks from combos ror specification etc
        // Replace angryblockpart with frozenblock
        // Can specify custom rules for allowed blocks (RainbowBlock, IronBlock, DerpBlock etc)

        // Custom Rules for gaem as a whole
    }
}
