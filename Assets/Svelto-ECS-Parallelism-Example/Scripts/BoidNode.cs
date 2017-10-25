using UnityEngine;

namespace Svelto.ECS.Example.Parallelism
{
#if FOURTH_TIER_EXAMPLE

    public struct BoidNode : IStructNodeWithID
    {
        public Vector3 position;

        public int ID { get; set; }
    }
#elif FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
    public class BoidNode : NodeWithID
    {
        public IBoidComponent node;
    }
#endif
}