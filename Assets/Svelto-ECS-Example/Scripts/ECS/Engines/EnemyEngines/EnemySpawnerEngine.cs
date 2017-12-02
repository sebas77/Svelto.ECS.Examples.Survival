using Svelto.ECS.Example.Survive.Components.Enemies;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using System.Collections;
using Svelto.Tasks.Enumerators;
using System;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemySpawnerEngine : SingleNodeEngine<EnemySpawningNode>, IStep<DamageInfo>
    {
        public EnemySpawnerEngine(Factories.IGameObjectFactory factory, IEntityFactory entityFactory)
        {
            _factory = factory;
            _entityFactory = entityFactory;
            _numberOfEnemyToSpawn = 15;

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
                            var go = _factory.Build(spawnData.enemyPrefab);
                            _entityFactory.BuildEntity(go.GetInstanceID(),
                                                       go.GetComponent<IEntityDescriptorHolder>()
                                                         .BuildDescriptorType());
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

        protected override void Add(EnemySpawningNode node)
        {
            _enemiestoSpawn = node.spawnerComponent.enemySpawnData;
        }

        protected override void Remove(EnemySpawningNode node)
        {}

        public void Step(ref DamageInfo token, Enum condition)
        {
            _numberOfEnemyToSpawn++;
        }

        EnemySpawnData[]                    _enemiestoSpawn;
        Svelto.Factories.IGameObjectFactory _factory;
        IEntityFactory                      _entityFactory;
        WaitForSecondsEnumerator            _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
        int                                 _numberOfEnemyToSpawn;
    }
}
