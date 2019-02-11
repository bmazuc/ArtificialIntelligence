using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public class Condition<T>
    {
        public virtual bool Test(FiniteStateMachine<T> stateMachine)
        {
            return false;
        }
    }
}