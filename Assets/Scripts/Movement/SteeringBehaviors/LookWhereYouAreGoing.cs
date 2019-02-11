using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class LookWhereYouAreGoing : SteeringBehavior
    {
        public override void SetTarget(Vector3 pos)
        {
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            Vector3 velocity = movement.Velocity;
            if (velocity.magnitude > 0)
                steering.angular = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            return steering;
        }
    }
}