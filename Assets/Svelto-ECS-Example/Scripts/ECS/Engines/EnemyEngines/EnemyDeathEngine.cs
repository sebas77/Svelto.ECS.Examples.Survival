using System;
using System.Collections;
using Svelto.DataStructures;
using Tuple = System.ValueTuple<Svelto.DataStructures.FasterList<Svelto.ECS.Example.Survive.Characters.Enemies.EnemyIterationInfo>, Svelto.ECS.EGIDMapper<Svelto.ECS.Example.Survive.Characters.Enemies.EnemyEntityViewStruct>>;

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
            //this struct will allow allocation 0 lambdas. When c# 7 can be used in Unity
            //this will be less awkward thanks to the local functions
            var valueTuple = new Tuple {Item1 = enemyIterationInfo};

            while (true)
            {
                while (entitiesDB.HasAny<EnemyEntityStruct>() == false) yield return null;

                valueTuple.Item2 = entitiesDB.QueryMappedEntities<EnemyEntityViewStruct>();

                entitiesDB.ExecuteOnEntities(ref valueTuple,
                        (ref EnemyEntityStruct enemyStruct, 
                         ref HealthEntityStruct healthEntityStruct, 
                         ref Tuple _parameters) =>
                             {
                                 if (healthEntityStruct.dead != true) return;

                                 uint index;
                                 SetParametersForDeath(ref _parameters.Item2.entities(healthEntityStruct.ID, out index)[index]);
                                     
                                 _parameters.Item1.Add(new EnemyIterationInfo(healthEntityStruct.ID,
                                                 (int) ECSGroups.EnemyDisabledGroups + (int) enemyStruct.enemyType));
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

        static void SetParametersForDeath(ref EnemyEntityViewStruct enemyEntityViewStruct)
        {
            enemyEntityViewStruct.layerComponent.layer                  = GAME_LAYERS.NOT_SHOOTABLE_MASK;
            enemyEntityViewStruct.movementComponent.navMeshEnabled      = false;
            enemyEntityViewStruct.movementComponent.setCapsuleAsTrigger = true;
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