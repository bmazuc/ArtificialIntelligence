using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public class Transition<T>
    {
        public Condition<T> condition;
        public State<T> target;

        public Transition(Condition<T> newCondition, State<T> newTarget)
        {
            condition = newCondition;
            target = newTarget;
        }

        public Transition(string value, float valueTest, E_FloatOperator conditionOperator, State<T> newTarget)
        {
            condition = new ConditionFloat<T>(value, valueTest, conditionOperator);
            target = newTarget;
        }

        public Transition(string value, int valueTest, E_IntOperator conditionOperator, State<T> newTarget)
        {
            condition = new ConditionInt<T>(value, valueTest, conditionOperator);
            target = newTarget;
        }

        public Transition(string value, bool valueTest, State<T> newTarget)
        {
            condition = new ConditionBool<T>(value, valueTest);
            target = newTarget;
        }

        public Transition(string value, State<T> newTarget)
        {
            condition = new ConditionTrigger<T>(value);
            target = newTarget;
        }
    }
}