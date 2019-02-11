using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class AvoidWall : Seek
    {
        [SerializeField]
        private float MaxSeeAhead = 10f;
        [SerializeField]
        public float AvoidForce = 20f;

        public override void SetTarget(Vector3 pos)
        {
        }

        public override Steering GetSteering()
        {
            if (movement.Velocity.magnitude == 0f)
                return new Steering();

            Vector3 ahead = Vector3.Normalize(movement.Velocity) * MaxSeeAhead;
            ahead.y = 0;
            int floorLayer = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Factory"));
            RaycastHit hit;


            if (Physics.Raycast(transform.position, ahead + new Vector3(0.5f, 0f, 0.5f), out hit, MaxSeeAhead, floorLayer))
            {
                target = hit.point + hit.normal * AvoidForce;
                return base.GetSteering();
            }
            if (Physics.Raycast(transform.position, ahead + new Vector3(-0.5f, 0f, -0.5f), out hit, MaxSeeAhead, floorLayer))
            {
                target = hit.point + hit.normal * AvoidForce;
                return base.GetSteering();
            }

            return new Steering();
        }
    }
}