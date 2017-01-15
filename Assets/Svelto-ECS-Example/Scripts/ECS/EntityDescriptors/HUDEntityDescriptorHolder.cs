using Svelto.ECS.Example.Nodes.HUD;
using UnityEngine;

namespace Svelto.ECS.Example.EntityDescriptors.HUD
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
		public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
		{
			return new HUDEntityDescriptor(GetComponentsInChildren<IComponent>());
		}
	}
}
