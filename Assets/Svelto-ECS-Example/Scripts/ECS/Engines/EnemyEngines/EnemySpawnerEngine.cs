using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
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
            var enemiestoSpawn = ReadEnemySpawningDataServiceRequest();

            while (true)
            {
//Svelto.Tasks allows to yield UnityYield instructions but this comes with a performance hit
//so the fastest solution is always to use custom enumerators. To be honest the hit is minimal
//but it's better to not abuse it.                
                yield return _waitForSecondsEnumerator;

                if (enemiestoSpawn != null)
                {
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = enemiestoSpawn[i];

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            // Find a random index between zero and one less than the number of spawn points.
                            int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);
                            var data = go.GetComponent<EnemyAttackDataHolder>();
                            
                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new EnemyAttackImplementor(data.timeBetweenAttacks, data.attackDamage));
                            _entityFactory.BuildEntity<EnemyEntityDescriptor>(
                                go.GetInstanceID(), implementors.ToArray());

                            var transform = go.transform;
                            var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;

                            spawnData.timeLeft = spawnData.spawnTime;
                            _numberOfEnemyToSpawn--;
                        }

                        spawnData.timeLeft -= 1.0f;
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

        public void Step(ref DamageInfo token, int condition)
        {
            _numberOfEnemyToSpawn++;
        }

        readonly Factories.IGameObjectFactory   _gameobjectFactory;
        readonly IEntityFactory                 _entityFactory;
        readonly WaitForSecondsEnumerator       _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);

        int     _numberOfEnemyToSpawn;
    }
}
