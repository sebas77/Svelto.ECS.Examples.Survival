using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Gun;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Player
{
	public class PlayerGunEntityDescriptor : GenericEntityDescriptor<GunEntityView>
    {}

    [DisallowMultipleComponent]
	public class PlayerGunEntityDescriptorHolder:GenericEntityDescriptorHolder<PlayerGunEntityDescriptor>
	{}
}
