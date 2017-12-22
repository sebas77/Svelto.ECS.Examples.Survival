using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;
using Svelto.ECS.Example.Survive.EntityViews.Player;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.Enemies
{
    class EnemyEntityDescriptor : GenericEntityDescriptor<EnemyEntityView, PlayerTargetEntityView, HealthEntityView>
    {}
}