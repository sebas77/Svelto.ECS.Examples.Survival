using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyTriggerImplementor : MonoBehaviour, IImplementor, IEnemyTriggerComponent
    {
        public EnemyCollisionData entityInRange { get; private set; }

        void OnTriggerEnter(Collider other)
        {
            entityInRange = new EnemyCollisionData(other.gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            entityInRange = new EnemyCollisionData(other.gameObject.GetInstanceID(), false);
        }

        bool    _targetInRange;
    }
}
