using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class Steering
    {
        public float angular;
        public Vector3 linear;

        public Steering()
        {
            angular = 0.0f;
            linear = new Vector3();
        }

        public static Steering operator*(Steering first, float second)
        {
            Steering result = new Steering();
            result.angular = first.angular * second;
            result.linear = first.linear * second;
            return result;
        }

        public static Steering operator+(Steering first, Steering second)
        {
            Steering result = new Steering();
            result.angular = first.angular + second.angular;
            result.linear = first.linear + second.linear;
            return result;
        }
    }

    public abstract class SteeringBehavior : MonoBehaviour
    {
        protected Vector3 target;
        protected Movement movement;
        public float weight = 1f;
 
        protected bool active = true;

        public virtual void Awake()
        {
            movement = GetComponent<Movement>();
        }

        public virtual void Start()
        {
            target = movement.transform.position;
        }

        public virtual void Update()
        {
        }

        public virtual void SetTarget(Vector3 pos)
        {
            target = pos;
        }

        public virtual Steering GetSteering() { return new Steering(); }
    }
}