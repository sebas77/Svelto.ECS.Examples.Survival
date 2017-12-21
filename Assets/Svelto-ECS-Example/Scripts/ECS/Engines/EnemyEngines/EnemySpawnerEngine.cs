using Svelto.ECS.Example.Survive.Components.Enemies;
using Svelto.ECS.Example.Survive.Components.Damageable;
using System.Collections;
using Svelto.Tasks.Enumerators;
using System;
using System.IO;
using Svelto.ECS.Example.Survive.EntityDescriptors.Enemies;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemySpawnerEngine : IEngine, IStep<DamageInfo>
    {
        public EnemySpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            _numberOfEnemyToSpawn = 15;
            
            var json = File.ReadAllText("EnemySpawningData.json");
            
            _enemiestoSpawn = JsonHelper.getJsonArray<EnemySpawnData>(json);

            TaskRunner.Instance.Run(IntervaledTick);
        }

        IEnumerator IntervaledTick()
        {
            while (true)
            {
                yield return _waitForSecondsEnumerator;
                
                if (_enemiestoSpawn != null)
                {
                    for (int i = _enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = _enemiestoSpawn[i];

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            // Find a random index between zero and one less than the number of spawn points.
                            int spawnPointIndex = UnityEngine.Random.Range(0, spawnData.spawnPoints.Length);

                            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);
                            _entityFactory.BuildEntity<EnemyEntityDescriptor>(go.GetInstanceID(),
                                                       go.GetComponentsInChildren<IComponent>());

                            var transform = go.transform;
                            var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;

                            spawnData.timeLeft = spawnData.spawnTime;
                        }

                        spawnData.timeLeft -= 1.0f;
                        _numberOfEnemyToSpawn--;
                    }
                }
            }
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            _numberOfEnemyToSpawn++;
        }

        EnemySpawnData[]                    _enemiestoSpawn;
        Svelto.Factories.IGameObjectFactory _gameobjectFactory;
        IEntityFactory                      _entityFactory;
        WaitForSecondsEnumerator            _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
        int                                 _numberOfEnemyToSpawn;
         }
}
