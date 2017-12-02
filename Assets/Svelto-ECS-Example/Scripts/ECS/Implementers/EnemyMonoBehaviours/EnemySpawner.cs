using Svelto.ECS.Example.Survive.Others;
using Svelto.ECS.Example.Survive.Components.Enemies;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemySpawningImplementor:IEnemySpawnerComponent
    {
        public EnemySpawnData[] enemySpawnData { get { return _enemySpawnData; }}

        public EnemySpawningImplementor(EnemySpawnDataSource[] data)
        {
            _enemySpawnData = new EnemySpawnData[data.Length];

            for (int i = 0; i < data.Length; i++)
                _enemySpawnData[i] = data[i].spawnData;
        }
        
        EnemySpawnData[] _enemySpawnData;
    }
}