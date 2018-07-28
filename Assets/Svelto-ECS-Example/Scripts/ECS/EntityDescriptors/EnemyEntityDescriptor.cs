using Svelto.ECS.Example.Survive.HUD;
using Svelto.ECS.Example.Survive.Characters.Player;
using Svelto.ECS.Example.Survive.Sound;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyEntityDescriptor : IEntityDescriptor
    {
        static readonly IEntityBuilder[] _entitiesToBuild = {
                                                                new EntityBuilder <EnemyEntityViewStruct>(),
                                                                new EntityBuilder <EnemyAttackEntityView>(),
                                                                new EntityBuilder <DamageSoundEntityView>(),
                                                                new EntityBuilder <EnemyAttackStruct>(),
                                                                new EntityBuilder <HealthEntityStruct>(),
                                                                new EntityBuilder <ScoreValueEntityStruct>(), 
                                                                new EntityBuilder <TargetEntityViewStruct>()};
        public IEntityBuilder[] entitiesToBuild
        {
            get { return _entitiesToBuild; }
        }
    }
}