using Svelto.DataStructures;
using Svelto.ECS.Example.Survive.Enemies;
using Svelto.ECS.Example.Survive.Player;
using Svelto.Factories;

namespace Svelto.ECS.Example.Survive
{
    class EnemyFactory : IEnemyFactory
    {
        public EnemyFactory(GameObjectPool gameObjectPool, 
                            IGameObjectFactory gameObjectFactory,
                            IEntityFactory entityFactory)
        {
            _gameobjectPool = gameObjectPool;
            _gameobjectFactory = gameObjectFactory;
            _entityFactory = entityFactory;
        }
        
        public void Build(EnemySpawnData enemySpawnData, ref EnemyAttackStruct enemyAttackstruct)
        {
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = UnityEngine.Random.Range(0, enemySpawnData.spawnPoints.Length);
            // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
            var go = _gameobjectPool.Use(enemySpawnData.enemyPrefab.name, delegate
                                                                                    {
                                                                                        return
                                                                                            _gameobjectFactory
                                                                                               .Build(enemySpawnData
                                                                                                     .enemyPrefab);
                                                                                    });
                                                       
            var implementors = go.GetComponentsInChildren<IImplementor>();
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
            var playerTargetTypeEntityStruct = new PlayerTargetTypeEntityStruct(enemySpawnData.targetType);
            _entityFactory.BuildEntity(
                                       go.GetInstanceID(), 
                                       new DynamicEntityDescriptorInfo<EnemyEntityDescriptor>(
                                            new FasterList<IEntityViewBuilder>
                                            {
                                                new EntityViewBuilder<EnemyAttackStruct>(ref enemyAttackstruct),
                                                new EntityViewBuilder<PlayerTargetTypeEntityStruct>(ref playerTargetTypeEntityStruct),
                                            }), 
                                       implementors);

            var transform = go.transform;
            var spawnInfo = enemySpawnData.spawnPoints[spawnPointIndex];
                            
            transform.position = spawnInfo.position;
            transform.rotation = spawnInfo.rotation;
        }

        readonly GameObjectPool     _gameobjectPool;
        readonly IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory     _entityFactory;
    }
}