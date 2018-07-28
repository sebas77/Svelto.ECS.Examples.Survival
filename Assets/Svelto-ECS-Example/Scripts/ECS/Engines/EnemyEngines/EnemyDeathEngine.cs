using System;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyDeathEngine:IQueryingEntitiesEngine, IStep<EnemyDeathCondition>
    {
        public EnemyDeathEngine(IEntityFunctions entityFunctions, ITime time, EnemyDeathSequencer enemyDeadSequencer)
        {
            _entityFunctions = entityFunctions;
            _time = time;
            _enemyDeadSequencer = enemyDeadSequencer;
        }
        
        public IEntitiesDB entitiesDB { get; set; }
        
        public void Ready()
        {}
        
        public void Step(EnemyDeathCondition condition, EGID id)
        {
            uint index;
            var entities = entitiesDB.QueryEntitiesAndIndex<EnemyEntityViewStruct>(id, out index);

         //   _entityFunctions.SwapEntityGroup(id, ECSGroups.enemyDisabledGroups + entities[index].enemyType);
            
            Sink(entities[index]).Run();
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

            _enemyDeadSequencer.Next(this, entity.ID);
        }

        readonly IEntityFunctions _entityFunctions;
        readonly ITime            _time;
        readonly EnemyDeathSequencer _enemyDeadSequencer;
    }
}