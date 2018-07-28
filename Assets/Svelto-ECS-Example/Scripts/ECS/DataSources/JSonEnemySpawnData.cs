using System;
using Svelto.ECS.Example.Survive.Characters.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JSonEnemySpawnData
    {
        public EnemySpawnData enemySpawnData;
        
        public JSonEnemySpawnData(EnemySpawnData spawnData)
        {
            enemySpawnData = spawnData;
        }
    }
    
    [Serializable]
    public class EnemySpawnData
    {
        public GameObject        enemyPrefab;
        public Transform[]       spawnPoints;
        public float             spawnTime;
        public PlayerTargetType  targetType;
    }
}