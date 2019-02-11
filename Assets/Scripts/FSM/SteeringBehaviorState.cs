using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    public class SteeringBehaviorState<T> : State<T>
    {
        private steering.Steering steering = new steering.Steering();
        private List<steering.SteeringBehavior> behaviors = new List<steering.SteeringBehavior>();

        public SteeringBehaviorState(T newOwner, FiniteStateMachine<T> ownerStateMachine)
        {
            owner = newOwner;
            stateMachine = ownerStateMachine;
        }

        public void AddBehavior(steering.SteeringBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        public steering.Steering GetSteering()
        {
            return steering;
        }

        public override void Execute()
        {
            steering.linear = Vector3.zero;
            steering.angular = 0f;

            foreach (steering.SteeringBehavior behavior in behaviors)
                if (behavior.enabled)
                    steering += behavior.GetSteering() * behavior.weight;

            ISteeringUser steeringOwner = (ISteeringUser)Owner;
            if (steeringOwner != null)
                steeringOwner.SetSteering(steering);
        }
    }
}