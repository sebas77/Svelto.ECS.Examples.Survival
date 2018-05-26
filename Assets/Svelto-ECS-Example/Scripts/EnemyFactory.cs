using Svelto.ECS.Example.Survive.Enemies;
using Svelto.ECS.Example.Survive.Player;
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
                                                       
            var playerTargetTypeEntityStruct = new PlayerTargetTypeEntityStruct { targetType = enemySpawnData.targetType};
            var healthEntityStruct = new HealthEntityStruct { currentHealth = 100 };

            initializer.Init(ref enemyAttackstruct);
            initializer.Init(ref healthEntityStruct);
            initializer.Init(ref playerTargetTypeEntityStruct);

            var transform = go.transform;
            var spawnInfo = enemySpawnData.spawnPoints[spawnPointIndex];
                            
            transform.position = spawnInfo.position;
            transform.rotation = spawnInfo.rotation;
        }

        readonly IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory     _entityFactory;
    }
}