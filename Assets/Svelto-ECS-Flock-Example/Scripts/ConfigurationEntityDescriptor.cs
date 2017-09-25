using Svelto.ECS.Example.Flock.Nodes;

namespace Svelto.ECS.Example.Flock.Entities
{
    class ConfigurationEntityDescriptor:EntityDescriptor
    {
        static readonly INodeBuilder[] _nodesToBuild;

        static ConfigurationEntityDescriptor()
        {
            _nodesToBuild = new INodeBuilder[]
            {
                new NodeBuilder<SettingsNode>(),
            };
        }
        public ConfigurationEntityDescriptor(object[] implementors) : base(_nodesToBuild, implementors)
        {}
    }
}