using Svelto.ES;
using UnityEngine;
using Nodes.Gun;

namespace EntityDescriptors.Player
{
    class PlayerGunEntityDescriptor : EntityDescriptor
    {
        static private readonly INodeBuilder[] _nodesToBuild;

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
		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new PlayerGunEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
