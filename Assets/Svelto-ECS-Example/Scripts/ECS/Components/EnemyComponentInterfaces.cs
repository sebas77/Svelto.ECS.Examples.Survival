using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Components.Enemies
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

    [Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public Transform[] spawnPoints;
        public float spawnTime;
        [HideInInspector]
        public float timeLeft;
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
