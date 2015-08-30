using EnemyComponents;
using UnityEngine;

namespace CompleteProject
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawnerComponent
    {
        public GameObject enemy;                // The enemy prefab to be spawned.
        public float spawnTime = 3f;            // How long between each spawn.
        public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

        GameObject IEnemySpawnerComponent.enemyPrefab { get { return enemy; } }
        float IEnemySpawnerComponent.spawnTime { get { return spawnTime; } }
        Transform[] IEnemySpawnerComponent.spawnPoints { get { return spawnPoints; } }
    }
}
