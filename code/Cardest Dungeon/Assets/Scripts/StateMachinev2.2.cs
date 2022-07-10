using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MethodStateMachine
{

    public class StateMachine
    {
        private int numberOfStates = 0;

        private State activeState;

        private List<State> states;
        /// <summary>
        /// The index in the state list of a state is equal to the index in the adjacency list. 
        /// </summary>
        private List<Transition>[] adjacencyList;

        #region Constructors

        //It will start in this state
        //public StateMachine(string nameOfState)
        //{
        //    Initialize(new State(nameOfState));
        //}

        //It will start in this state
        //public StateMachine(State firstState)
        //{
        //    Initialize(firstState);
        //}

        //It will start in this state
        public StateMachine(string nameOfState, Action body)
        {
            Initialize(new State(nameOfState, body));
        }

        private void Initialize(State firstState)
        {
            activeState = firstState;

            states = new List<State>();
            states.Add(firstState);

            adjacencyList = new List<Transition>[0];
            ResizeAdjacencyList();

            numberOfStates++;
        }

        #endregion Constructors

        /// <summary>
        /// Run performs the active state's body.
        /// </summary>
        public void Run()
        {
            activeState.Run();
        }

        public string GetActiveStateName()
        {
            return activeState.GetName();
        }

        public string[] GetNameOfAllStates()
        {
            string[] allNames = new string[states.Count];
            
            for(int i = 0; i < states.Count; i++)
            {
                allNames[i] = states.ElementAt(i).GetName();
            }

            return allNames;
        }

        /// <summary>
        /// Will always work. If no valid transition found it will still change state.
        /// </summary>
        /// <param name="nameOfState"></param>
        public void TransitionToState(string nameOfTarget)
        {
            Transition transition = adjacencyList[states.IndexOf(activeState)].Find(transition => transition.GetTargetName() == nameOfTarget);
            activeState = states.Find(state => state.GetName() == nameOfTarget);
            
            transition?.Invoke();

        }


        /// <summary>
        /// Forces the state machine to switch state regardless if there is a valid transition. No transition will be executed.
        /// </summary>
        /// <param name="newStateName"></param>
        public void ForceActiveState(string newStateName)
        {
            activeState = states.Find(state => state.GetName() == newStateName);
        }

        #region Add

        /// <summary>
        /// Adds a state to the state machine with given name and body.
        /// </summary>
        /// <param name="nameOfState">name of the new State</param>
        /// <param name="body">a method attached to the state which will be invoked everytime the machine runs and is in this state.</param>
        public void AddState(string nameOfState, Action body)
        {
            if(numberOfStates == adjacencyList.Length)
            {
                ResizeAdjacencyList();
            }

            State state = new State(nameOfState, body);
            states.Add(state);
            numberOfStates++;
        }

        /// <summary>
        /// Adds a transition to the state machine that transitions from originState to targetState and performs action while doing so.
        /// </summary>
        /// <param name="originState">The start of the transition.</param>
        /// <param name="targetState">the target of the transition.</param>
        /// <param name="action">The method that will be performed by this transition.</param>
        public Transition AddTransition(string originState, string targetState, Action action)
        {
            State target = FindState(targetState, "Target could not be found.");
            Transition transition = new Transition(target, action);

            AddTransition(originState, transition);

            return transition;
        }

        /// <summary>
        /// Adds an already existing transition to the state machine with originState as origin.
        /// </summary>
        /// <param name="originState">The origin of the transition.</param>
        /// <param name="transition">the transition.</param>
        public void AddTransition(string originState, Transition transition)
        {
            State state = FindState(originState, "Origin could not be found.");
            adjacencyList[states.IndexOf(state)].Add(transition);
        }

        #endregion AddState

        private State FindState(string nameOfState, string additionalErrorMessage = "")
        {
            State state = states.Find(x => x.GetName() == nameOfState);
            if (state == null)
            {
                throw new ArgumentException("No such state was found." + " " + additionalErrorMessage);
            }

            return state;
        }

        /// <summary>
        /// Example: sizeDelta = 10 ==> The list will be extended by 10
        /// </summary>
        /// <param name="sizeDelta"></param>
        private void ResizeAdjacencyList(int sizeDelta = 10)
        {
            int oldSize = adjacencyList.Length;
            Array.Resize(ref adjacencyList, adjacencyList.Length + sizeDelta);
            for (int i = oldSize; i < adjacencyList.Length; i++)
            {
                adjacencyList[i] = new List<Transition>();
            }
        }

        public class State
        {
            private string name = default;

            private Action body = default;

            public State(string nameOfState)
            {
                name = nameOfState;
            }

            public State(string nameOfState, Action bodyMethod)
            {
                name = nameOfState;
                body = bodyMethod;
            }

            public void SetBodyMethod(Action method)
            {
                body = method;
            }

            public void Run()
            {
                body?.Invoke();
            }

            public string GetName()
            {
                return name;
            }
        }

        public class Transition
        {
            /// <summary>
            /// The state into which this transition will transfer.
            /// </summary>
            private State target;

            /// <summary>
            /// Will be performed exactly once each time this transition is used.
            /// </summary>
            private Action action;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="targetState">The state into which this transition will transfer.</param>
            /// <param name="action">Will be performed exactly once each time this transition is used.</param>
            public Transition(State targetState, Action action)
            {
                target = targetState;
                this.action = action;
            }
            
            public void Invoke()
            {
                action?.Invoke();
            }

            public string GetTargetName()
            {
                return target.GetName();
            }

            public State GetTarget()
            {
                return target;
            }
        }
    }
}