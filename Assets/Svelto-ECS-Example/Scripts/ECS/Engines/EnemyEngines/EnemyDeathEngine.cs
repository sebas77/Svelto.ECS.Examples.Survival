using System.Collections;
using Svelto.DataStructures;

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
            var enemyIterationInfo = new FasterList<EnemyIterationInfo>();
            
            while (true)
            {
                entitiesDB.ExecuteOnEntities(
                        (ref EnemyEntityStruct enemyStruct, ref HealthEntityStruct healthEntityStruct) =>
                             {
                                 if (healthEntityStruct.dead == true)
                                 {
                                     SetParametersForDeath(healthEntityStruct.ID);
                                     
                                     enemyIterationInfo.Add(new EnemyIterationInfo(healthEntityStruct.ID,
                                                    (int) ECSGroups.EnemyDisabledGroups + (int) enemyStruct.enemyType));
                                 }
                             });

                for (int i = 0; i < enemyIterationInfo.Count; i++)
                {
                    //don't remove, but swap. This is how pooling is done in Svelto.
                    //Pooling is not needed when just pure EntityStructs are generated as they are allocation free.
                    //The swap is necessary so that the enemy entity cannot be shot while it's dying
                    //the group works like a sort of state for the Enemy entity in this case.
                    var newID = _entityFunctions.SwapEntityGroup<EnemyEntityDescriptor>(enemyIterationInfo[i].ID, 
                                                                                        enemyIterationInfo[i].group);
                                                                        
                    _enemyDeadSequencer.Next(this, EnemyDeathCondition.Death, newID);                    
                }
                
                enemyIterationInfo.FastClear();

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

    struct EnemyIterationInfo
    {
        public readonly EGID ID;

        public EnemyIterationInfo(EGID id, int group)
        {
            ID = id;
            this.group = group;
        }

        public int group { get; set; }
    }
}