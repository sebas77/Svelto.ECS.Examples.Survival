using System.Collections;
using Svelto.Tasks.Enumerators;
using System.IO;
using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemySpawnerEngine : IStep<EGID>, IQueryingEntitiesEngine
    {
        public EnemySpawnerEngine(IEnemyFactory enemyFactory, IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
            _enemyFactory = enemyFactory;
            _numberOfEnemyToSpawn = 15;
        }
        
        public IEntitiesDB entitiesDB { get; set; }

        public void Ready()
        {
            IntervaledTick().Run();            
        }

        IEnumerator IntervaledTick()
        {
//this is of fundamental importance: Never create implementors as Monobehaviour just to hold 
//data (especially if read only data). Data should always been retrieved through a service layer
//regardless the data source.
//The benefit are numerous, including the fact that changing data source would require
//only changing the service code. In this simple example I am not using a Service Layer
//but you can see the point.          
//Also note that I am loading the data only once per application run, outside the 
//main loop. You can always exploit this trick when you now that the data you need
//to use will never change            
            var enemiestoSpawn = ReadEnemySpawningDataServiceRequest();
            var enemyAttackData = ReadEnemyAttackDataServiceRequest();
            
            float[] spawningTimes = new float[enemiestoSpawn.Length];

            for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                spawningTimes[i] = enemiestoSpawn[i].enemySpawnData.spawnTime;

            while (true)
            {
//Svelto.Tasks can yield Unity YieldInstructions but this comes with a performance hit
//so the fastest solution is always to use custom enumerators. To be honest the hit is minimal
//but it's better to not abuse it.                
                yield return _waitForSecondsEnumerator;
                {
                    //cycle around the enemies to spawn and check if it can be spawned
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        if (spawningTimes[i] <= 0.0f)
                        {
                            var spawnData = enemiestoSpawn[i];
                            
                            //In this example every kind of enemy generates the same list of EntityViews
                            //therefore I always use the same EntityDescriptor. However if the 
                            //different enemies had to create different EntityViews for different
                            //engines, this would have been a good example where EntityDescriptorHolder
                            //could have been used to exploit the the kind of polymorphism explained
                            //in my articles.
                            EnemyAttackStruct enemyAttackstruct = new EnemyAttackStruct
                            {
                                attackDamage      = enemyAttackData[i].enemyAttackData.attackDamage,
                                timeBetweenAttack = enemyAttackData[i].enemyAttackData.timeBetweenAttacks
                            };

                            //has a compatible entity previously disabled and can be reused?
                            //Note, pooling make sense only for Entities that use implementors.
                            //A pure struct based entity doesn't need pooling because it 
                            //never allocates.
                            var fromGroupId = ECSGroups.EnemyGroup[spawnData.enemySpawnData.targetType];
                            if (entitiesDB.HasAny<EnemyEntityViewStruct>(fromGroupId))
                            {
                                ReuseEnemy(fromGroupId, ref spawnData);
                            }
                            else
                            {
                                _enemyFactory.Build(spawnData.enemySpawnData, ref enemyAttackstruct);
                            }

                            spawningTimes[i] = spawnData.enemySpawnData.spawnTime;
                            _numberOfEnemyToSpawn--;
                        }

                        spawningTimes[i] -= SECONDS_BETWEEN_SPAWNS;
                    }
                }
            }
        }

        void ReuseEnemy(int fromGroupId, ref JSonEnemySpawnData spawnData)
        {
            //take an entity (with all its entity views and implementors) from the group
            var egid = _entityFunctions.SwapFirstEntityGroup(fromGroupId);
            
            //reset some components
            entitiesDB.ExecuteOnEntity(egid,
                                          (ref HealthEntityStruct healthStruct) => { healthStruct.currentHealth = 100; });
            entitiesDB.ExecuteOnEntity(egid, ref spawnData,
                                          (ref EnemyEntityViewStruct    entityView,
                                           ref JSonEnemySpawnData spawnDataInfo) =>
                                          {
                                              int spawnPointIndex =
                                                  UnityEngine
                                                     .Random
                                                     .Range(0,
                                                            spawnDataInfo.enemySpawnData.spawnPoints
                                                                         .Length);

                                              var spawnInfo =
                                                  spawnDataInfo.enemySpawnData.spawnPoints
                                                      [spawnPointIndex];

                                              entityView.transformComponent.position =
                                                  spawnInfo.position;
                                              entityView.transformComponent.rotation =
                                                  spawnInfo.rotation;

                                              entityView.animationComponent.reset();
                                              entityView.movementComponent.navMeshEnabled = true;
                                              entityView.movementComponent.setCapsuleAsTrigger = false;
                                          });
        }

        static JSonEnemySpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(UnityEngine.Application.persistentDataPath + "/EnemySpawningData.json");
            
            JSonEnemySpawnData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemySpawnData>(json);
            
            return enemiestoSpawn;
        }
        
        static JSonEnemyAttackData[] ReadEnemyAttackDataServiceRequest()
        {
            string json = File.ReadAllText(UnityEngine.Application.persistentDataPath + "/EnemyAttackData.json");
            
            JSonEnemyAttackData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemyAttackData>(json);
            
            return enemiestoSpawn;
        }
        
        public void Step(ref EGID token, int condition)
        {
            _numberOfEnemyToSpawn++;
        }

        readonly WaitForSecondsEnumerator   _waitForSecondsEnumerator = new WaitForSecondsEnumerator(SECONDS_BETWEEN_SPAWNS);
        
        readonly IEnemyFactory _enemyFactory;
        readonly IEntityFunctions _entityFunctions;

        int     _numberOfEnemyToSpawn;
        
        const int SECONDS_BETWEEN_SPAWNS = 1;
    }
}
