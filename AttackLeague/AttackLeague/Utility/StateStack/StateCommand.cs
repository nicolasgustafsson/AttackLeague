using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.StateStack
{
    public enum EStateCommandType
    {
        Add,
        Remove,
        Replace,
        RemoveToFirst,
        RemoveAll
    }

    public enum EStateType
    {
        Major,
        Sub
    }

    struct StateCommand
    {
        public EStateCommandType myCommandType;
        public EStateType myStateType;
        public State myState;
    }
}
