using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.HUD;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.Sound;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Player
{
    public class PlayerEntityDescriptor : GenericEntityDescriptor<HUDDamageEntityView, PlayerEntityView, EnemyTargetEntityView, DamageSoundEntityView, HealthEntityView>
	{}

	[DisallowMultipleComponent]
	public class PlayerEntityDescriptorHolder:GenericEntityDescriptorHolder<PlayerEntityDescriptor>
	{}
}
