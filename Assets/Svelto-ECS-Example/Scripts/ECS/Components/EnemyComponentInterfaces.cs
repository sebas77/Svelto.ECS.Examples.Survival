using UnityEngine;

namespace Svelto.ECS.Example.Components.Enemy
{
    public interface IEnemyAttackComponent: IComponent
    {
        bool targetInRange { get; }
    }

    public interface IEnemyAttackDataComponent: IComponent
    {
        int   damage            { get; }
        float attackInterval    { get; }
        float timer             { get; set; }
    }

    public interface IEnemyMovementComponent: IComponent
    {
        UnityEngine.AI.NavMeshAgent navMesh            { get; }
        float sinkSpeed                 { get; }
        CapsuleCollider capsuleCollider { get; }
    }

    public interface IEnemySpawnerComponent: IComponent
    {
        GameObject enemyPrefab  { get; }
        Transform[] spawnPoints { get; }
        float spawnTime         { get; }
    }

    public interface IEnemyTriggerComponent: IComponent
    {
        event System.Action<int, int, bool> entityInRange;

        bool targetInRange { set; }
    }

    public interface IEnemyVFXComponent: IComponent
    {
        ParticleSystem hitParticles { get; }
    }
}
