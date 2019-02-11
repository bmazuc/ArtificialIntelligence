using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace steering
{
    public class Patrol : PathFollow
    {
        private int pathDirection = 1;

        public override Steering GetSteering()
        {
            if (path != null && path.Count > 0)
            {
                target = path[pathId].ToNode.Position;
                if (HasReachTarget())
                {
                    pathId += pathDirection;
                    if (IsAtFinalPosition() || pathId == 0)
                        pathDirection *= -1;
                }

                return base.GetSteering();
            }

            return new Steering();
        }
    }
}