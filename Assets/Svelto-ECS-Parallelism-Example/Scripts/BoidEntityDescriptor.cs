namespace Svelto.ECS.Example.Parallelism
{
    class BoidEntityDescriptor : EntityDescriptor
    {
        static readonly INodeBuilder[] _nodesToBuild;

        static BoidEntityDescriptor()
        {
            _nodesToBuild = new INodeBuilder[]
            {
#if FOURTH_TIER_EXAMPLE
                new FastStructNodeBuilder<BoidNode>(),
#endif
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
                new NodeBuilder<BoidNode>(),
#endif
            };
        }
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
        public BoidEntityDescriptor(object[] implementors) : base(_nodesToBuild, implementors)
#else
        public BoidEntityDescriptor() : base(_nodesToBuild)
#endif
        { }
    }
}