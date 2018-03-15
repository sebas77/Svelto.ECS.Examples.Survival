using System;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyDeathEngine:IStep<DamageInfo>, IQueryingEntityViewEngine
    {
        public EnemyDeathEngine(IEntityFunctions entityFunctions, ITime time)
        {
            _entityFunctions = entityFunctions;
            _time = time;
        }
        
        public void Step(ref DamageInfo token, int condition)
        {
            var entity = entityViewsDB.QueryEntityView<EnemyEntityView>(token.entityDamagedID);
            
            _entityFunctions.RemoveEntity(entity.ID);
            
            Sink(entity).Run();
        }
        
        IEnumerator Sink(EnemyEntityView entity)
        {
            DateTime afterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < afterTwoSec)
            {
                entity.transformComponent.position = 
                    entity.positionComponent.position + -UnityEngine.Vector3.up * entity.sinkSpeedComponent.sinkAnimSpeed * _time.deltaTime;

                yield return null;
            }

            //we need to wait until the animation is finished
            //before to destroy the gameobject! 
            entity.destroyComponent.mustDestroy.value = true;
        }

        readonly IEntityFunctions _entityFunctions;
        public IEntityViewsDB entityViewsDB { get; set; }
        
        public void Ready()
        {}
        
        ITime _time;
    }
}