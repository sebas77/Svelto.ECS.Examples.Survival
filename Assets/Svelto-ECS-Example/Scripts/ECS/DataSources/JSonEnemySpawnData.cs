using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Others
{
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
    public class EnemySpawnData
    {
        public GameObject    enemyPrefab;
        public Transform[]   spawnPoints;
        public float         spawnTime;
    }

    [Serializable]
    public struct SpawningStruct
    {
        public Vector3     position;
        public Quaternion  rotation;
    }
}