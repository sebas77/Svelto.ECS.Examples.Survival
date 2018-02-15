using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyTriggerImplementor : MonoBehaviour, IImplementor, IEnemyTriggerComponent, IEnemyTargetComponent
    {
        public event Action<int, int, bool> entityInRange;

        bool IEnemyTriggerComponent.targetInRange { set { _targetInRange = value; } }
        bool IEnemyTargetComponent.targetInRange { get { return _targetInRange; } }

        void OnTriggerEnter(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject.GetInstanceID(), gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            if (entityInRange != null)
                entityInRange(other.gameObject.GetInstanceID(), gameObject.GetInstanceID(), false);
        }

        bool    _targetInRange;
    }
}
