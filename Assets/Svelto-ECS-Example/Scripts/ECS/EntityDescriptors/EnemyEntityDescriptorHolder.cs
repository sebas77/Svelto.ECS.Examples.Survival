using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.ECS.Example.Survive.Nodes.DamageableEntities;
using Svelto.ECS.Example.Survive.Nodes.Player;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Enemies
{
    class EnemyEntityDescriptor : EntityDescriptor
    {
        static readonly INodeBuilder[] _nodesToBuild;

        static EnemyEntityDescriptor() 
        {
            _nodesToBuild = new INodeBuilder[]
            {
                new NodeBuilder<EnemyNode>(),
                new NodeBuilder<PlayerTargetNode>(),
                new NodeBuilder<HealthNode>(),
            };
        }

        public EnemyEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
        {}
    }

	[DisallowMultipleComponent]
	public class EnemyEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
        public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
        {
            return new EnemyEntityDescriptor(GetComponentsInChildren<IComponent>());
        }
	}
}