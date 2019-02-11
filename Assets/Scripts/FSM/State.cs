using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public abstract class State<T>
    {
        public string name;
        protected FiniteStateMachine<T> stateMachine;

        protected List<Transition<T>> transitions = new List<Transition<T>>();

        protected T owner;
        public T Owner { get { return owner; } set { owner = value; } }

        public virtual void Execute()
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }
        
        public void AddTransition(Transition<T> transition)
        {
            transitions.Add(transition);
        }

        public State<T> CheckTransitions()
        {
            foreach (Transition<T> t in transitions)
            {
                if (t.condition.Test(stateMachine))
                {
                    t.target.Enter();
                    Exit();
                    return t.target;
                }
            }

            return null;
        }
    }
}