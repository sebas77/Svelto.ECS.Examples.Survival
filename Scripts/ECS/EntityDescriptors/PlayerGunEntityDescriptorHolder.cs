using Svelto.ES;
using Nodes.HUD;
using UnityEngine;
using Nodes.Player;
using Nodes.Enemies;
using Nodes.DamageableEntities;
using Nodes.Sound;

namespace EntityDescriptors.Player
{
	[DisallowMultipleComponent]
	public class PlayerGunEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		class PlayerGunEntityDescriptor : Svelto.ES.EntityDescriptor
		{
            static private readonly INodeBuilder[] _nodesToBuild;

            static PlayerGunEntityDescriptor()
			{
				_nodesToBuild = new INodeBuilder[]
				{
					new NodeBuilder<PlayerGunNode>(),
				};
			}

			public PlayerGunEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
			{}
		}

		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new PlayerGunEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
