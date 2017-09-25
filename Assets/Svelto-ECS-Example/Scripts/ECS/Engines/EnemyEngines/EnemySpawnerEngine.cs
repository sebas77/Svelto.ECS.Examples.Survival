using Svelto.ECS.Example.Survive.Components.Enemy;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.DataStructures;
using System;
using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemySpawnerEngine : INodesEngine
    {
        internal class EnemySpawnerData
        {
            internal float timeLeft;
            internal GameObject enemy;
            internal float spawnTime;
            internal Transform[] spawnPoints;

            internal EnemySpawnerData(IEnemySpawnerComponent spawnerComponent)
            {
                enemy = spawnerComponent.enemyPrefab;
                spawnTime = spawnerComponent.spawnTime;
                spawnPoints = spawnerComponent.spawnPoints;
                timeLeft = spawnTime;
            }
        }

        public EnemySpawnerEngine(Factories.IGameObjectFactory factory, IEntityFactory entityFactory)
        {
            _factory = factory;
            _entityFactory = entityFactory;
            TaskRunner.Instance.Run(IntervaledTick);
        }

        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            var spawnerComponents = (obj as EnemySpawningNode).spawnerComponents;

            for (int i = 0; i < spawnerComponents.Length; i++)
                _enemiestoSpawn.Add(new EnemySpawnerData(spawnerComponents[i]));
        }

        public void Remove(INode obj)
        {
            //remove is called on context destroyed, in this case the entire engine will be destroyed
        }

        IEnumerator IntervaledTick()
        {
            while (true)
            {
                yield return _waitForSecondsEnumerator;

                for (int i = _enemiestoSpawn.Count - 1; i >= 0; --i)
                {
                    var spawnData = _enemiestoSpawn[i];

                    if (spawnData.timeLeft <= 0.0f)
                    {
                        // Find a random index between zero and one less than the number of spawn points.
                        int spawnPointIndex = UnityEngine.Random.Range(0, spawnData.spawnPoints.Length);

                        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                        var go = _factory.Build(spawnData.enemy);
                        _entityFactory.BuildEntity(go.GetInstanceID(), go.GetComponent<IEntityDescriptorHolder>().BuildDescriptorType());
                        var transform = go.transform;
                        var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                        transform.position = spawnInfo.position;
                        transform.rotation = spawnInfo.rotation;

                        spawnData.timeLeft = spawnData.spawnTime;
                    }

                    spawnData.timeLeft -= 1.0f;
                }
            }
        }

        readonly Type[]                     _acceptedNodes = { typeof(EnemySpawningNode) };
        FasterList<EnemySpawnerData>        _enemiestoSpawn = new FasterList<EnemySpawnerData>();
        Svelto.Factories.IGameObjectFactory _factory;
        IEntityFactory                      _entityFactory;
        Tasks.WaitForSecondsEnumerator      _waitForSecondsEnumerator = new Tasks.WaitForSecondsEnumerator(1);
    }
}
