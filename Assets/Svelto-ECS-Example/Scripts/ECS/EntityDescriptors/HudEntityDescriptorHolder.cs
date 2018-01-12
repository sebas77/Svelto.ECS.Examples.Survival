using Svelto.ECS.Example.Survive.EntityViews.HUD;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.HUD
{
    [DisallowMultipleComponent]
	public class HudEntityDescriptorHolder:GenericEntityDescriptorHolder<GenericEntityDescriptor<HUDEntityView>>
	{}
}
