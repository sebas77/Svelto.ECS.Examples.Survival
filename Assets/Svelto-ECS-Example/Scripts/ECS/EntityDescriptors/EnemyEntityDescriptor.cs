using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.ECS.Example.Survive.EntityViews.Sound;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Enemies
{
    class EnemyEntityDescriptor : GenericEntityDescriptor<EnemyEntityView, DamageSoundEntityView, PlayerTargetEntityView, HealthEntityView>
    {}
}