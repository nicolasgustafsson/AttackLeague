using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class DebugPlayer : Player
    {
        public DebugPlayer()
            :base(new DebugPlayerInfo())
        {
        }

        public override void Update()
        {
            base.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("RandomizeGrid"))
                myGrid.GenerateGrid();

            if (myPlayerInfo.myMappedActions.ActionIsActive("Pause"))
                myIsPaused = !myIsPaused;

            if (myPlayerInfo.myMappedActions.ActionIsActive("StepOnce"))
                myGrid.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("IncreaseGameSpeed"))
                myGrid.AddGameSpeed(0.5f);
        }

    }
}
