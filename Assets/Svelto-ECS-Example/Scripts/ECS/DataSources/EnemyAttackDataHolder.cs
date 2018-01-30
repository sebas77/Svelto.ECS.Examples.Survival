using UnityEngine;

namespace Svelto.ECS.Example.Survive.DataSources
{
    public class EnemyAttackDataHolder : MonoBehaviour 
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.
    }
}
