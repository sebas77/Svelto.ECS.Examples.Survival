using System;
using UnityEngine;
using Components.Enemy;

namespace Implementators.Enemies
{
    public class EnemyTrigger : MonoBehaviour, IEnemyTriggerComponent, IEnemyAttackComponent
    {
        public GameObject ID { get { return gameObject; } }

        public event Action<int, bool> entityInRange;

        bool IEnemyTriggerComponent.targetInRange { set { _playerInRange = value; } }
        bool IEnemyAttackComponent.targetInRange { get { return _playerInRange; } }

        void OnTriggerEnter(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject.GetInstanceID(), false);
        }

        bool    _playerInRange;
    }
}
