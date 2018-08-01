using System.Collections;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyDeathEngine:IQueryingEntitiesEngine
    {
        public EnemyDeathEngine(IEntityFunctions entityFunctions, EnemyDeathSequencer enemyDeadSequencer)
        {
            _entityFunctions = entityFunctions;
            
            _enemyDeadSequencer = enemyDeadSequencer;
        }
        
        public IEntitiesDB entitiesDB { get; set; }

        public void Ready()
        {
            CheckIfDead().Run();
        }
        
        IEnumerator CheckIfDead()
        {
            while (true)
            {
                int count;
                //fetch all the enemies
                var enemies = entitiesDB.QueryEntities<EnemyEntityStruct>(out count);
                for (int i = 0; i < count; i++)
                {
                    uint index;

                    //are they dead?
                    if (entitiesDB.QueryEntitiesAndIndex<HealthEntityStruct>
                            (enemies[i].ID, out index)[index].dead == true)
                    {
                        SetParametersForDeath(enemies[i].ID);
                        
                        //don't remove, but swap. This is how pooling is done in Svelto.
                        //Pooling is not needed when just pure EntityStructs are generated, 
                        //as they are allocation free.
                        var newID = _entityFunctions.SwapEntityGroup<EnemyEntityDescriptor>(enemies[i].ID, (int)ECSGroups.EnemyDisabledGroups + (int)enemies[i].enemyType);
                        i--;
                        count--;
                                                
                        _enemyDeadSequencer.Next(this, EnemyDeathCondition.Death, newID);
                    }
                }

                yield return null;
            }
        }

        void SetParametersForDeath(EGID ID)
        {
            uint index;
            var  enemyEntityViewStructs = entitiesDB.QueryEntitiesAndIndex<EnemyEntityViewStruct>(ID, out index);

            enemyEntityViewStructs[index].layerComponent.layer                  = GAME_LAYERS.NOT_SHOOTABLE_MASK;
            enemyEntityViewStructs[index].movementComponent.navMeshEnabled      = false;
            enemyEntityViewStructs[index].movementComponent.setCapsuleAsTrigger = true;
        }

        readonly IEntityFunctions    _entityFunctions;
        readonly EnemyDeathSequencer _enemyDeadSequencer;
    }
}