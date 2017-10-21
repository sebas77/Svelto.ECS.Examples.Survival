#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE|| THIRD_TIER_EXAMPLE
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Parallelism
{
    public class Boid : IBoidComponent
    {
        public Vector3 position { set; get; }

        public Quaternion rotation { set; get; }

        public Vector3 velocity { set; get; }
    }
}
#endif