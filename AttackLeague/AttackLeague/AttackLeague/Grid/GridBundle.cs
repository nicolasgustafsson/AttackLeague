using AttackLeague.AttackLeague.Blocks.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Grid
{
    public class GridBundle
    {
        public GridBehavior Behavior;
        public GridContainer Container;
        public BlockGenerator Generator;

        public GridBundle(int aPlayerIndex)
        {
            Container = new GridContainer();
            Generator = new BlockGenerator();

            Container.SetGenerator(Generator);
            Generator.SetBundle(this);

            Behavior = new GridBehavior(Container, aPlayerIndex);
        }
    }
}
