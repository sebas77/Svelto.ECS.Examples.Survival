using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Gun;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Player
{
    class PlayerGunEntityDescriptor : EntityDescriptor
    {
        static readonly INodeBuilder[] _nodesToBuild;

        static PlayerGunEntityDescriptor()
		{
			_nodesToBuild = new INodeBuilder[]
			{
				new NodeBuilder<GunNode>(),
			};
		}

		public PlayerGunEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
		{}
	}

    [DisallowMultipleComponent]
	public class PlayerGunEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
		{
			return new PlayerGunEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
