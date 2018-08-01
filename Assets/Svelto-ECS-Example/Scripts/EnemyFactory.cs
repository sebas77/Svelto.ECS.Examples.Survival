using Svelto.ECS.Example.Survive.Characters;
using Svelto.ECS.Example.Survive.Characters.Enemies;
using Svelto.ECS.Example.Survive.HUD;
using Svelto.Factories;

namespace Svelto.ECS.Example.Survive
{
    class EnemyFactory : IEnemyFactory
    {
        public EnemyFactory(IGameObjectFactory gameObjectFactory,
                            IEntityFactory entityFactory)
        {
            _gameobjectFactory = gameObjectFactory;
            _entityFactory = entityFactory;
        }
        
        public void Build(EnemySpawnData enemySpawnData, ref EnemyAttackStruct enemyAttackstruct)
        {
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = UnityEngine.Random.Range(0, enemySpawnData.spawnPoints.Length);
            // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
            var go = _gameobjectFactory.Build(enemySpawnData.enemyPrefab);
            var implementors = go.GetComponentsInChildren<IImplementor>();
            var initializer = _entityFactory.BuildEntity<EnemyEntityDescriptor>(go.GetInstanceID(), 
                                                                                implementors);
            initializer.Init(enemyAttackstruct);
            initializer.Init(new HealthEntityStruct { currentHealth = 100 });
            initializer.Init(new ScoreValueEntityStruct { scoreValue = (int)enemySpawnData.targetType * 10 });
            initializer.Init(new EnemyEntityStruct { enemyType = enemySpawnData.targetType});

            var transform = go.transform;
            var spawnInfo = enemySpawnData.spawnPoints[spawnPointIndex];
                            
            transform.position = spawnInfo.position;
            transform.rotation = spawnInfo.rotation;
        }

        readonly IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory     _entityFactory;
    }
}