using System;
using Svelto.ECS.Example.Survive.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JSonEnemySpawnData
    {
        public GameObject enemyPrefab;
        public SpawningStruct[] spawnPoints;
        public float spawnTime;
        public PlayerTargetType targetType;
        
        public JSonEnemySpawnData(EnemySpawnData spawnData)
        {
            enemyPrefab = spawnData.enemyPrefab;
            spawnPoints = new SpawningStruct[spawnData.spawnPoints.Length];
            targetType = spawnData.targetType;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i].position = spawnData.spawnPoints[i].position;
                spawnPoints[i].rotation = spawnData.spawnPoints[i].rotation;
            }

            spawnTime = spawnData.spawnTime;
        }
    }
    
    [Serializable]
    public class EnemySpawnData
    {
        public GameObject    enemyPrefab;
        public Transform[]   spawnPoints;
        public float         spawnTime;
        public PlayerTargetType targetType;
    }

    [Serializable]
    public struct SpawningStruct
    {
        public Vector3     position;
        public Quaternion  rotation;
    }
}