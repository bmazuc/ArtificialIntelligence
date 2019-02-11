using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public enum E_IntOperator
    {
        GREATER = 0,
        LESSER,
        EQUALS,
        NOTEQUAL,
        GREATEROREQUALS,
        LESSEROREQUALS
    };

    public class ConditionInt<T> : Condition<T>
    {
        private string parameter;
        private int valueTest;
        private E_IntOperator operatorTest;

        public ConditionInt(string newValue, int newValueTest, E_IntOperator newOperator)
        {
            parameter = newValue;
            valueTest = newValueTest;
            operatorTest = newOperator;
        }

        public override bool Test(FiniteStateMachine<T> stateMachine)
        {
            switch (operatorTest)
            {
                case E_IntOperator.GREATER:
                    return IsValueGreater(stateMachine);
                case E_IntOperator.LESSER:
                    return IsValueLesser(stateMachine);
                case E_IntOperator.EQUALS:
                    return IsValueEquals(stateMachine);
                case E_IntOperator.NOTEQUAL:
                    return IsValueNotEqual(stateMachine);
                case E_IntOperator.GREATEROREQUALS:
                    return IsValueGreaterOrEquals(stateMachine);
                case E_IntOperator.LESSEROREQUALS:
                    return IsValueLesserOrEquals(stateMachine);
                default: return false;
            }
        }

        private bool IsValueGreater(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) > valueTest)
                return true;

            return false;
        }

        private bool IsValueLesser(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) < valueTest)
                return true;

            return false;
        }

        private bool IsValueEquals(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) == valueTest)
                return true;

            return false;
        }

        private bool IsValueNotEqual(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) != valueTest)
                return true;

            return false;
        }

        private bool IsValueGreaterOrEquals(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) >= valueTest)
                return true;

            return false;
        }

        private bool IsValueLesserOrEquals(FiniteStateMachine<T> stateMachine)
        {
            if (stateMachine.GetInt(parameter) <= valueTest)
                return true;

            return false;
        }
    }
}