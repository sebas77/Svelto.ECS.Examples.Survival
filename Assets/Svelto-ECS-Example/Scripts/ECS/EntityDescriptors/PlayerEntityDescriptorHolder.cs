using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.HUD;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.Sound;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Player
{
    public class PlayerEntityDescriptor : GenericEntityDescriptor<HUDDamageEntityView, PlayerEntityView, EnemyTargetEntityView, DamageSoundEntityView>
	{}

	[DisallowMultipleComponent]
	public class PlayerEntityDescriptorHolder:GenericEntityDescriptorHolder<PlayerEntityDescriptor>
	{}
}
