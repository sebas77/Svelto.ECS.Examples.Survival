using Svelto.ES;
using Nodes.HUD;
using UnityEngine;
using Nodes.Player;
using Nodes.Enemies;
using Nodes.DamageableEntities;
using Nodes.Sound;

namespace EntityDescriptors.Player
{
    class PlayerEntityDescriptor : EntityDescriptor
	{
        static readonly INodeBuilder[] _nodesToBuild;

        static PlayerEntityDescriptor()
		{
			_nodesToBuild = new INodeBuilder[]
			{
				new NodeBuilder<HUDDamageEventNode>(),
                new NodeBuilder<PlayerNode>(),
                new NodeBuilder<EnemyTargetNode>(),
                new NodeBuilder<HealthNode>(),
                new NodeBuilder<DamageSoundNode>()
			};
		}

		public PlayerEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
		{}
	}

	[DisallowMultipleComponent]
	public class PlayerEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new PlayerEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
