using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public class ConditionTrigger<T> : Condition<T>
    {
        private string parameter;

        public ConditionTrigger(string newValue)
        {
            parameter = newValue;
        }

        public override bool Test(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetTrigger(parameter) == true)
                return true;

            return false;
        }
    }
}