using System;
using Svelto.ES;
using Nodes.HUD;
using UnityEngine;

namespace EntityDescriptors.HUD
{
    class HUDEntityDescriptor : EntityDescriptor
    {
		static private INodeBuilder[] _nodesToBuild;

		static HUDEntityDescriptor()
		{
			_nodesToBuild = new INodeBuilder[]
			{
				new NodeBuilder<HUDNode>()
			};
		}

		public HUDEntityDescriptor(IComponent[] componentsImplementor):base(_nodesToBuild, componentsImplementor)
		{}
	}

	[DisallowMultipleComponent]
	public class HUDEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new HUDEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
