using System;
using System.Collections;
using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyDeathEngine:IQueryingEntityViewEngine, IStep<DamageInfo, DamageCondition>
    {
        public EnemyDeathEngine(IEntityFunctions entityFunctions, ITime time, ISequencer enemyDeadSequencer)
        {
            _entityFunctions = entityFunctions;
            _time = time;
            _enemyDeadSequencer = enemyDeadSequencer;
        }
        
        public IEntityViewsDB entityViewsDB { get; set; }
        
        public void Ready()
        {}
        
        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            uint index;
            var entity = entityViewsDB.QueryEntities<EnemyEntityViewStruct>(token.entityDamagedID, out index)[index];
            var playerTargetTypeEntityStructs = entityViewsDB.QueryEntities<PlayerTargetTypeEntityStruct>(token.entityDamagedID, out index);

            _entityFunctions.SwapEntityGroup(token.entityDamagedID.entityID, ECSGroups.EnemyGroup[playerTargetTypeEntityStructs[index].targetType]);
            
            Sink(entity).Run();
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

            var entityId = entity.ID;
            _enemyDeadSequencer.Next(this, ref entityId);
        }

        readonly IEntityFunctions _entityFunctions;
        readonly ITime            _time;
        readonly ISequencer       _enemyDeadSequencer;
    }
}