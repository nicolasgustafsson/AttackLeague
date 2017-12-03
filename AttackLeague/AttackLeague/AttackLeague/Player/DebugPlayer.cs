using AttackLeague.AttackLeague.Blocks.Angry;
using Microsoft.Xna.Framework;
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
        {}

        public override void Update()
        {
            base.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("RandomizeGrid"))
                myGridBundle.Generator.GenerateGrid();

            if (myPlayerInfo.myMappedActions.ActionIsActive("Pause"))
                myIsPaused = !myIsPaused;

            if (myPlayerInfo.myMappedActions.ActionIsActive("StepOnce"))
                myGridBundle.Behavior.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("IncreaseGameSpeed"))
                myGridBundle.Behavior.AddGameSpeed(0.5f);

            if (myPlayerInfo.myMappedActions.ActionIsActive("SpawnAngryBundle"))
            {
                Point angrySize = new Point(6, 3);
                Point position = new Point(0, myGridBundle.Container.GetInitialHeight() + angrySize.Y);
                AngryBlockBundle angryBundle = myGridBundle.Generator.CreateAngryBlockBundleAtPosition(position, angrySize);
                myGridBundle.Behavior.AddAngryBundle(angryBundle);
            }
        }
    }
}
