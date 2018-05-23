namespace Svelto.ECS.Example.Survive.Enemies
{
    public interface IEnemyFactory
    {
        void Build(EnemySpawnData spawnDataEnemySpawnData, ref EnemyAttackStruct enemyAttackstruct);
    }
}