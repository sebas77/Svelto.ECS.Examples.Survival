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
    }
    
    [Serializable]
    public class JSonEnemySpawnData
    {
        public GameObject enemyPrefab;
        public SpawningStruct[] spawnPoints;
        public float spawnTime;
        public float timeLeft;

        public JSonEnemySpawnData(EnemySpawnData spawnData)
        {
            enemyPrefab = spawnData.enemyPrefab;
            spawnPoints = new SpawningStruct[spawnData.spawnPoints.Length];

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i].position = spawnData.spawnPoints[i].position;
                spawnPoints[i].rotation = spawnData.spawnPoints[i].rotation;
            }

            spawnTime = spawnData.spawnTime;
            timeLeft = spawnTime;
        }
    }

    [Serializable]
    public struct SpawningStruct
    {
        public Vector3 position;
        public Quaternion rotation;
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
