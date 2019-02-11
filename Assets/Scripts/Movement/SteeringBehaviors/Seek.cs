using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class Seek : SteeringBehavior
    {
        [SerializeField]
        private bool slowDownOnApproach;
        [SerializeField]
        private float targetRadius;
        [SerializeField]
        private float slowRadius;


        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 desiredVelocity = target - movement.transform.position;

            if (slowDownOnApproach)
            {
                float distance = desiredVelocity.magnitude;

                float targetSpeed;
                if (distance < targetRadius)
                    return steering;
                if (distance > slowRadius)
                    targetSpeed = movement.GetMaxSpeed;
                else
                    targetSpeed = movement.GetMaxSpeed * (distance / slowRadius);

                if (steering.linear.magnitude > targetSpeed)
                {
                    desiredVelocity.Normalize();
                    desiredVelocity *= targetSpeed;
                }
            }
            else if (steering.linear.magnitude > movement.GetMaxSpeed)
            {
                desiredVelocity.Normalize();
                desiredVelocity *= movement.GetMaxSpeed;
            }

            steering.linear = weight * (desiredVelocity - movement.Velocity);

            return steering;
        }
    }
}