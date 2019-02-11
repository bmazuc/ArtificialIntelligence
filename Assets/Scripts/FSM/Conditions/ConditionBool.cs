using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public class ConditionBool<T> : Condition<T>
    {
        protected string parameter;
        private bool valueTest;

        public ConditionBool(string newValue, bool newValueTest)
        {
            parameter = newValue;
            valueTest = newValueTest;
        }

        public override bool Test(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetBool(parameter) == valueTest)
                return true;

            return false;
        }
    }
}