using Svelto.ES;
using UnityEngine;
using Nodes.Enemies;
using Nodes.DamageableEntities;
using Nodes.Player;

namespace EntityDescriptors.Enemies
{
	[DisallowMultipleComponent]
	public class EnemyEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		class EnemyEntityDescriptor : EntityDescriptor
        {
            static private readonly INodeBuilder[] _nodesToBuild;

            static EnemyEntityDescriptor() 
            {
				_nodesToBuild = new INodeBuilder[]
				{
					new NodeBuilder<EnemyNode>(),
                    new NodeBuilder<PlayerTargetNode>(),
                    new NodeBuilder<DamageNode>(),
				};
			}

			public EnemyEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
			{}
		}

		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new EnemyEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}