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
            entitiesDB.ExecuteOnEntity(id,
                                       (ref EnemyEntityViewStruct entity) =>
                                       {
                                           Sink(entity).Run();
                                           entity.animationComponent.playAnimation = "Dead";
                                       });
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            //is player is dead, the enemy cheers
            int count;
            var entity = entitiesDB.QueryEntities<EnemyEntityViewStruct>(out count);

            for (var i = 0; i < count; i++)
                entity[i].animationComponent.playAnimation = "PlayerDead";
        }
        
        IEnumerator CheckForDamage()
        {
            while (true)
            {
                int numberOfEnemies;
                var enemies = entitiesDB.QueryEntities<EnemyEntityStruct>(out numberOfEnemies);

                for (int i = 0; i < numberOfEnemies; i++)
                {
                    uint damageableEntityIndex;
                    var damageableEntityStructs = entitiesDB.QueryEntitiesAndIndex<DamageableEntityStruct>(enemies[i].ID, out damageableEntityIndex);
                    
                    if (damageableEntityStructs[damageableEntityIndex].damaged == false) continue;

                    entitiesDB.ExecuteOnEntity(enemies[i].ID, ref damageableEntityStructs[damageableEntityIndex].damageInfo,
                                               (ref EnemyEntityViewStruct entity, ref DamageInfo damageInfo) =>
                                               {
                                                   entity.vfxComponent.position   = damageInfo.damagePoint;
                                                   entity.vfxComponent.play.value = true;
                                               });
                }

                yield return null;
            }
        }
        
        IEnumerator Sink(EnemyEntityViewStruct entity)
        {
            DateTime afterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < afterTwoSec)
            {
                entity.transformComponent.position = 
                    entity.positionComponent.position + -UnityEngine.Vector3.up * entity.sinkSpeedComponent.sinkAnimSpeed * _time.deltaTime;

                yield return null;
            }

            uint index;
            int enemyType = (int) entitiesDB.QueryEntitiesAndIndex<EnemyEntityStruct>(entity.ID, out index)[index].enemyType;
            
            _entityFunctions.SwapEntityGroup<EnemyEntityDescriptor>(entity.ID, (int)ECSGroups.EnemyToRecycleGroups + enemyType);

            _enemyDeadSequencer.Next(this, EnemyDeathCondition.Death, entity.ID);
        }

        readonly ITime _time;
        readonly EnemyDeathSequencer _enemyDeadSequencer;
        readonly IEntityFunctions _entityFunctions;
    }
}