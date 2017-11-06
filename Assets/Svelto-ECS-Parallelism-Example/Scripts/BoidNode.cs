using UnityEngine;

namespace Svelto.ECS.Example.Parallelism
{
#if FOURTH_TIER_EXAMPLE
    public struct BoidNode : IStructNodeWithID
    {
        public Vector3 position;

        public int ID { get; set; }
    }
#else
    public class BoidNode : NodeWithID
    {
        public IBoidComponent node;
    }
#endif
}