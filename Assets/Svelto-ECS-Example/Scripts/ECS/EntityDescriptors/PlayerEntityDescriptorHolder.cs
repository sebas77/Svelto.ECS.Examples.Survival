using Svelto.ECS.Example.Nodes.HUD;
using UnityEngine;
using Svelto.ECS.Example.Nodes.Player;
using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.ECS.Example.Nodes.DamageableEntities;
using Svelto.ECS.Example.Nodes.Sound;

namespace Svelto.ECS.Example.EntityDescriptors.Player
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
		public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
		{
			return new PlayerEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
