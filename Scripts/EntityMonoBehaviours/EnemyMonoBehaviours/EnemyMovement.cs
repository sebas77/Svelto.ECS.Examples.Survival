using EnemyComponents;
using SharedComponents;
using UnityEngine;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour, IEnemyMovementComponent, ITransformComponent
    {
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.

        NavMeshAgent nav;                           // Reference to the nav mesh agent.
        CapsuleCollider capsuleCollider;            // Reference to the capsule collider.

        CapsuleCollider IEnemyMovementComponent.capsuleCollider { get { return capsuleCollider; } }
        NavMeshAgent IEnemyMovementComponent.navMesh { get { return nav; } }

        float IEnemyMovementComponent.sinkSpeed { get { return sinkSpeed; } }
        Transform ITransformComponent.transform { get { return transform; }}

        void Awake ()
        {
            nav = GetComponent <NavMeshAgent> ();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
    }
}
