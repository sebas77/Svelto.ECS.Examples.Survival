namespace Svelto.ECS.Example.Flock
{
    class BoidEntityDescriptor : EntityDescriptor
    {
        static readonly INodeBuilder[] _nodesToBuild;

        static BoidEntityDescriptor()
        {
            _nodesToBuild = new INodeBuilder[]
            {
                new NodeBuilder<BoidNode>(),
            };
        }
        public BoidEntityDescriptor(IImplementor[] implementors) : base(_nodesToBuild, implementors)
        {}
    }
}