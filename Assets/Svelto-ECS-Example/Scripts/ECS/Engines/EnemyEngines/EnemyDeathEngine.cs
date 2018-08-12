using System.Collections;
using Svelto.DataStructures;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyDeathEngine : IQueryingEntitiesEngine
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
            var enemyIterationInfo = new FasterList<EGID>();

            while (true)
            {
                //wait for enemies to be created
                while (entitiesDB.HasAny<EnemyEntityStruct>(ECSGroups.ActiveEnemies) == false) yield return null;

                //Very important: Since I use groups, I know that the entity queries are relative to the enemy
                //entities and therefore the count value will be always the same. Choose your groups wisely. 
                //if entities from two entitiy descriptors share one entity view and are created in the same group,
                //the total number of that specific entity views queried will be NumberOfEntityA + NumberOfEntityB.
                //Groups assure that the number of entity view are the same in the same group, even if the entity view
                //is shared with other entities in other groups.
                int count;
                var enemyEntitiesViews = entitiesDB.QueryEntities<EnemyEntityViewStruct>(ECSGroups.ActiveEnemies, out count);
                var enemyEntitiesHealth = entitiesDB.QueryEntities<HealthEntityStruct>(ECSGroups.ActiveEnemies, out count);

                for (int i = 0; i < count; i++)
                {
                    if (enemyEntitiesHealth[i].dead == false) continue;

                    SetParametersForDeath(ref enemyEntitiesViews[i]);

                    enemyIterationInfo.Add(enemyEntitiesHealth[i].ID);
                }

                for (int i = 0; i < enemyIterationInfo.Count; i++)
                {
                    //don't remove, but swap. This is how pooling is done in Svelto.
                    //Pooling is not needed when just pure EntityStructs are generated as they are allocation free.
                    //The swap is necessary so that the enemy entity cannot be shot while it's dying
                    //the group works like a sort of state for the Enemy entity in this case.
                    var newID = _entityFunctions.SwapEntityGroup<EnemyEntityDescriptor>(enemyIterationInfo[i],
                                                                                        (int) ECSGroups
                                                                                           .DeadEnemiesGroups);

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
}
