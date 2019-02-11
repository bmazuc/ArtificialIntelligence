using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class PathFollow : Seek
    {
        protected Navigation.AStarPathfinder pathfinder;
        protected List<Navigation.Connection> path;
        protected int pathId;

        protected Unit unit;

        [SerializeField]
        protected float reachDistance = 10f;

        public override void Awake()
        {
            base.Awake();
            path = null;
            unit = GetComponent<Unit>();
        }

        public override void Start()
        {
            base.Start();
            pathfinder = GetComponent<Navigation.AStarPathfinder>();
        }

        public override void SetTarget(Vector3 pos)
        {
            if (pathfinder)
            {
                path = null;
                pathId = 0;
                path = pathfinder.FindPath(pos);
            }
        }

        protected bool HasReachTarget()
        {
            return (target - transform.position).magnitude <= reachDistance;
        }

        protected bool IsAtFinalPosition()
        {
            return path != null && pathId == path.Count - 1;
        }

        public override Steering GetSteering()
        {
            if (path != null && path.Count > 0)
            {
                target = path[pathId].ToNode.Position;
                
                if (HasReachTarget() && !IsAtFinalPosition())
                    ++pathId;

                return base.GetSteering();
            }
            
            return new Steering();
        }
    }
}