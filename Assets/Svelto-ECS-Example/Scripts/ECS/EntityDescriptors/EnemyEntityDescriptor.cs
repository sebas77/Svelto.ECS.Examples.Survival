using Svelto.ECS.Example.Survive.Player;
using Svelto.ECS.Example.Survive.Sound;

namespace Svelto.ECS.Example.Survive.Enemies
{
    class EnemyEntityDescriptor : GenericEntityDescriptor<EnemyEntityViewStruct, EnemyAttackEntityView, DamageSoundEntityView, 
        EnemyAttackStruct, HealthEntityStruct, PlayerTargetTypeEntityStruct>
    {}
}