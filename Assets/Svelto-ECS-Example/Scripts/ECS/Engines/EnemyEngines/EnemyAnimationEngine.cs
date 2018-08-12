using System;
using System.Collections;
using Svelto.ECS.Example.Survive.Characters.Player;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntitiesEngine
                                      , IStep<PlayerDeathCondition>
                                      , IStep<EnemyDeathCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public EnemyAnimationEngine(ITime time, EnemyDeathSequencer enemyDeadSequencer, IEntityFunctions entityFunctions)
        {
            _time = time;
            _enemyDeadSequencer = enemyDeadSequencer;
            _entityFunctions = entityFunctions;
        }

        public void Ready()
        {
            CheckForDamage().Run();
        }

        public void Step(EnemyDeathCondition condition, EGID id)
        {
            uint index;
            Sink(entitiesDB.QueryEntitiesAndIndex<EnemyEntityViewStruct>(id, out index)[index]).Run();
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            //is player is dead, the enemy cheers
            int count;
            var entity = entitiesDB.QueryEntities<EnemyEntityViewStruct>(ECSGroups.ActiveEnemies, out count);

            for (var i = 0; i < count; i++)
                entity[i].animationComponent.playAnimation = "PlayerDead";
        }
        
        IEnumerator CheckForDamage()
        {
            while (true)
            {
                int numberOfEnemies;
                var damageableEntityStructs =
                    entitiesDB.QueryEntities<DamageableEntityStruct>(ECSGroups.ActiveEnemies, out numberOfEnemies);
                var enemyEntityViewsStructs =
                    entitiesDB.QueryEntities<EnemyEntityViewStruct>(ECSGroups.ActiveEnemies, out numberOfEnemies);

                for (int i = 0; i < numberOfEnemies; i++)
                {
                    if (damageableEntityStructs[i].damaged == false) continue;

                    enemyEntityViewsStructs[i].vfxComponent.position = damageableEntityStructs[i].damageInfo.damagePoint;
                    enemyEntityViewsStructs[i].vfxComponent.play = true;
                }

                yield return null;
            }
        }
        
        IEnumerator Sink(EnemyEntityViewStruct entity)
        {
            entity.animationComponent.playAnimation = "Dead";
            
            DateTime afterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < afterTwoSec)
            {
                entity.transformComponent.position = 
                    entity.positionComponent.position + -UnityEngine.Vector3.up * entity.sinkSpeedComponent.sinkAnimSpeed * _time.deltaTime;

                yield return null;
            }

            uint index;
            PlayerTargetType enemyType = entitiesDB.QueryEntitiesAndIndex<EnemyEntityStruct>(entity.ID, out index)[index].enemyType;
            
            _entityFunctions.SwapEntityGroup<EnemyEntityDescriptor>(entity.ID, (int)ECSGroups.EnemiesToRecycleGroups + (int)enemyType);

            _enemyDeadSequencer.Next(this, EnemyDeathCondition.Death, entity.ID);
        }

        readonly ITime               _time;
        readonly EnemyDeathSequencer _enemyDeadSequencer;
        readonly IEntityFunctions    _entityFunctions;
    }
}