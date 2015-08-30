using System;
using UnityEngine;
using EnemyComponents;

namespace CompleteProject
{
    public class EnemyTrigger : MonoBehaviour, IEnemyTriggerComponent, IEnemyAttackComponent
    {
        public GameObject ID { get { return gameObject; } }

        public event Action<GameObject, bool> entityInRange;

        bool IEnemyTriggerComponent.playerInRange { set { _playerInRange = value; } }
        bool IEnemyAttackComponent.playerInRange { get { return _playerInRange; } }

        void OnTriggerEnter(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject, true);
        }

        void OnTriggerExit(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject, false);
        }

        bool    _playerInRange;
    }
}
