using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.StateStack
{
    class StateStack
    {
        List<List<State>> myStates = new List<List<State>>();
        List<StateCommand> myQueuedCommands = new List<StateCommand>();

        public State GetCurrentState()
        {
            return myStates.Last().Last();
        }

        public void AddCommand(StateCommand aCommand)
        {
            myQueuedCommands.Add(aCommand);
        }

        public void Update()
        {
            int startIndex = myStates.Last().Count -1;
            for (int i = startIndex; i > 0; i--)
            {
                if (myStates.Last()[i].UpdateThrough == false)
                    break;
                startIndex--;
            }

            for (int i = startIndex; i < myStates.Last().Count; i++)
            {
                myStates.Last()[i].StateUpdate();
            }
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            int startIndex = myStates.Last().Count - 1;
            for (int i = startIndex; i > 0; i--)
            {
                if (myStates.Last()[i].RenderThrough == false)
                    break;
                startIndex--;
            }

            for (int i = startIndex; i < myStates.Last().Count; i++)
            {
                myStates.Last()[i].StateDraw(aSpriteBatch);
            }
        }

        public void ResolveQueuedThings()
        {
            foreach (var command in myQueuedCommands)
            {
                switch (command.myStateType)
                {
                    case EStateType.Major:
                        switch (command.myCommandType)
                        {
                            case EStateCommandType.Add:
                                AddMajorState(command.myState);
                                break;
                            case EStateCommandType.Remove:
                                RemoveMajorState();
                                break;
                            case EStateCommandType.Replace:
                                RemoveMajorState();
                                AddMajorState(command.myState);
                                break;
                            case EStateCommandType.RemoveToFirst:
                                RemoveToFirst();
                                break;
                            case EStateCommandType.RemoveAll:
                                RemoveAll();
                                break;
                        }
                        break;
                    case EStateType.Sub:
                        switch (command.myCommandType)
                        {
                            case EStateCommandType.Add:
                                AddSubState(command.myState);
                                break;
                            case EStateCommandType.Remove:
                                RemoveState();
                                break;
                            case EStateCommandType.Replace:
                                RemoveState();
                                AddSubState(command.myState);
                                break;
                            case EStateCommandType.RemoveToFirst:
                                RemoveToFirst();
                                break;
                            case EStateCommandType.RemoveAll:
                                RemoveAll();
                                break;
                        }
                        break;
                }
            }

            myQueuedCommands.Clear();
        }

        private void AddMajorState(State aState)
        {
            aState.myStateStack = this;
            myStates.Add(new List<State>());
            AddSubState(aState);
        }

        private void AddSubState(State aState)
        {
            aState.myStateStack = this;
            ExitCurrentState();
            myStates.Last().Add(aState);
            myStates.Last().Last().OnCreated();
            EnterCurrentState();
        }

        private void RemoveToFirst()
        {
            while (myStates.Count > 1)
                RemoveMajorState();
            while (myStates.Last().Count > 1)
                RemoveState();
        }   

        private void RemoveAll()
        {
            while (myStates.Count > 0)
                RemoveMajorState();
        }

        private void RemoveMajorState()
        {
            int stateCount = myStates.Last().Count;
            for (int i = 0; i < stateCount -1; ++i)
                RemoveState();
        }

        private void RemoveState()
        {
            if (myStates.Count > 0 && myStates.Last().Count > 0)
            {
                ExitCurrentState();
                myStates.Last().Last().OnDestroyed();
                myStates.Last().RemoveAt(myStates.Last().Count - 1);
                if (myStates.Last().Count == 0)
                    myStates.RemoveAt(myStates.Count - 1);
                EnterCurrentState();
            }
        }

        private void ExitCurrentState()
        {
            if (myStates.Count > 0 && myStates.Last().Count > 0)
            {
                myStates.Last().Last().OnExit();
            }
        }

        private void EnterCurrentState()
        {
            if (myStates.Count > 0 && myStates.Last().Count > 0)
            {
                myStates.Last().Last().OnEnter();
            }
        }
    }
}
