using Svelto.ES;
using UnityEngine;
using Nodes.Enemies;
using Nodes.DamageableEntities;
using Nodes.Player;

namespace EntityDescriptors.Enemies
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
		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new EnemyEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}