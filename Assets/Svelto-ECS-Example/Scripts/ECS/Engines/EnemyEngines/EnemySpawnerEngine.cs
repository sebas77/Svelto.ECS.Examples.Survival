using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using Svelto.DataStructures;
using Svelto.ECS.Example.Survive.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemySpawnerEngine : IEngine, IStep<DamageInfo>
    {
        public EnemySpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            _numberOfEnemyToSpawn = 15;

            IntervaledTick().Run();
        }

        IEnumerator IntervaledTick()
        {
//OK this is of fundamental importance: Never create implementors as Monobehaviour just to hold 
//data. Data should always been retrieved through a service layer regardless the data source.
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
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        if (spawningTimes[i] <= 0.0f)
                        {
                            var spawnData = enemiestoSpawn[i];
                            // Find a random index between zero and one less than the number of spawn points.
                            int spawnPointIndex = Random.Range(0, spawnData.enemySpawnData.spawnPoints.Length);

                            // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
                            var go = _gameobjectFactory.Build(spawnData.enemySpawnData.enemyPrefab);
                                                       
                            //we are going to use a mix of implementors as Monobehaviours and
                            //normal implementors here:                           
                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new PlayerTargetTypeImplementor(spawnData.enemySpawnData.targetType));
                            
                            //In this example every kind of enemy generates the same list of EntityViews
                            //therefore I always use the same EntityDescriptor. However if the 
                            //different enemies had to create different EntityViews for differentaww
                            //engines, this would have been a good example where EntityDescriptorHolder
                            //could have been used to exploit the the kind of polymorphism explained
                            //in my articles.
                            EnemyAttackStruct initialize = new EnemyAttackStruct();
                            initialize.attackDamage = enemyAttackData[i].enemyAttackData.attackDamage;
                            initialize.timeBetweenAttack = enemyAttackData[i].enemyAttackData.timeBetweenAttacks;
                            
                            //The DynamicEntityDescriptorInfo here is just an excercise. This is a complex way
                            //to build an entity, please see the other BuildEntity for normal cases.
                            //DynamicEntityDescriptorInfo allows to extend the basic EntityDescriptor with
                            //extra IEntityViewBuilder. In this case, for the sake of experimentation, 
                            //I wanted the Enemy Entity to generate an EntityStruct too. EntityStructs are
                            //used for super fast cache friendly code, which is totally unnecessary for this 
                            //case, but at least I have an example to show their use.
                            //In this case, I want the struct to be initialized with specific values.
                            //I don't need a DynamicEntityDescriptorInfo to build EntityStructs, they
                            //can be declared through EntityViewStructBuilder statically in a MixedEntityDescriptor.
                            //however in this case I need it to be dynamic to pass the values to use for initialization!
                            _entityFactory.BuildEntity(
                                go.GetInstanceID(), 
                                    new DynamicEntityDescriptorInfo<EnemyEntityDescriptor>(
                                        new FasterList<IEntityViewBuilder> {new EntityViewStructBuilder<EnemyAttackStruct>(ref initialize)}), 
                                        implementors.ToArray());

                            var transform = go.transform;
                            var spawnInfo = spawnData.enemySpawnData.spawnPoints[spawnPointIndex];
                            
                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;

                            spawningTimes[i] = spawnData.enemySpawnData.spawnTime;
                            _numberOfEnemyToSpawn--;
                        }

                        spawningTimes[i] -= SECONDS_BETWEEN_SPAWNS;
                    }
                }
            }
        }

        static JSonEnemySpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemySpawningData.json");
            
            JSonEnemySpawnData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemySpawnData>(json);
            
            return enemiestoSpawn;
        }
        
        static JSonEnemyAttackData[] ReadEnemyAttackDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemyAttackData.json");
            
            JSonEnemyAttackData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemyAttackData>(json);
            
            return enemiestoSpawn;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            _numberOfEnemyToSpawn++;
        }

        readonly Factories.IGameObjectFactory   _gameobjectFactory;
        readonly IEntityFactory                 _entityFactory;
        readonly WaitForSecondsEnumerator       _waitForSecondsEnumerator = new WaitForSecondsEnumerator(SECONDS_BETWEEN_SPAWNS);

        int     _numberOfEnemyToSpawn;
        const int SECONDS_BETWEEN_SPAWNS = 1;
    }
}
