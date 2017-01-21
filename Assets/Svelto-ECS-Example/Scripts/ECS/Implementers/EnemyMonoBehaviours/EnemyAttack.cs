using Svelto.ECS.Example.Components.Enemy;
using UnityEngine;

namespace Svelto.ECS.Example.Implementers.Enemies
{
    public class EnemyAttack : MonoBehaviour, IEnemyAttackDataComponent
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.

        float IEnemyAttackDataComponent.attackInterval { get { return timeBetweenAttacks; } }
        float IEnemyAttackDataComponent.timer { get; set; }
        int IEnemyAttackDataComponent.damage { get { return attackDamage; } }
    }
}
