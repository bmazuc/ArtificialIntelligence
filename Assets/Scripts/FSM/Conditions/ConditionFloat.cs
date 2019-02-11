using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public enum E_FloatOperator
    {
        GREATER = 0,
        LESSER
    };

    public class ConditionFloat<T> : Condition<T>
    {
        private string parameter;
        private float valueTest;
        private E_FloatOperator operatorTest;

        public ConditionFloat(string newValue, float newValueTest, E_FloatOperator newOperator)
        {
            parameter = newValue;
            valueTest = newValueTest;
            operatorTest = newOperator;
        }

        public override bool Test(FiniteStateMachine<T> stateMachine)
        {
            switch (operatorTest)
            {
                case E_FloatOperator.GREATER :
                    return IsValueGreaterThanValueTest(stateMachine);
                case E_FloatOperator.LESSER :
                    return IsValueLesserThanValueTest(stateMachine);
                default : return false;
            }
        }

        private bool IsValueGreaterThanValueTest(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetFloat(parameter) > valueTest)
                return true;

            return false;
        }

        private bool IsValueLesserThanValueTest(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetFloat(parameter) < valueTest)
                return true;

            return false;
        }
    }
}