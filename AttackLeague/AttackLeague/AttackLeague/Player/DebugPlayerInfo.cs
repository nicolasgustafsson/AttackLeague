using AttackLeague.Utility;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Player
{
    class DebugPlayerInfo : PlayerInfo
    {
        public DebugPlayerInfo()
            :base(0, EInputType.GamePad1, "Debug Player")
        {
        }

        protected override void GenerateDefaultActions()
        {
            GenerateDefaultGamePadActions();
            GenerateDefaultKeyboardActions();

            myMappedActions.BindAction("RandomizeGrid", Keys.R, InputStatus.KeyPressed);
            myMappedActions.BindAction("Pause", Keys.Enter, InputStatus.KeyPressed);
            myMappedActions.BindAction("StepOnce", Keys.OemPeriod, InputStatus.KeyPressed);
            myMappedActions.BindAction("IncreaseGameSpeed", Keys.T, InputStatus.KeyPressed);
            myMappedActions.BindAction("SpawnAngryBundle", Keys.K, InputStatus.KeyReleased);
            myMappedActions.BindAction("ConnectToNicos", Keys.N, InputStatus.KeyReleased);
            myMappedActions.BindAction("ConnectToYlf", Keys.Y, InputStatus.KeyReleased); 
            myMappedActions.BindAction("SendHardCodedMessage", Keys.M, InputStatus.KeyReleased);
        }
    }
}
