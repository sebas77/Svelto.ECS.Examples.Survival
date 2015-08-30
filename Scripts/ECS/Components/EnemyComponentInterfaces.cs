using UnityEngine;

namespace EnemyComponents
{
    public interface IEnemyAttackComponent
    {
        bool playerInRange { get; }
    }

    public interface IEnemyAttackDataComponent
    {
        int   damage            { get; }
        float attackInterval    { get; }
        float timer             { get; set; }
    }

    public interface IEnemyMovementComponent
    {
        NavMeshAgent navMesh            { get; }
        float sinkSpeed                 { get; }
        CapsuleCollider capsuleCollider { get; }
    }

    public interface IEnemySpawnerComponent
    {
        GameObject enemyPrefab  { get; }
        Transform[] spawnPoints { get; }
        float spawnTime         { get; }
    }

    public interface IEnemyTriggerComponent
    {
        event System.Action<GameObject, bool> entityInRange;

        bool playerInRange { set; }
    }

    public interface IEnemyVFXComponent
    {
        ParticleSystem hitParticles { get; }
    }
}
