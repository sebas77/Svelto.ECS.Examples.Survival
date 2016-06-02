using System;
using Svelto.ES;
using UnityEngine;
using Components.Enemy;
using Components.Base;

namespace Implementators.Enemies
{
    public class EnemyMovement : MonoBehaviour, IEnemyMovementComponent, ITransformComponent, IRemoveEntityComponent
    {
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.

        NavMeshAgent    _nav;                       // Reference to the nav mesh agent.
        CapsuleCollider _capsuleCollider;           // Reference to the capsule collider.
        Transform       _transform;
        Action          _removeAction;

        CapsuleCollider IEnemyMovementComponent.capsuleCollider { get { return _capsuleCollider; } }
        NavMeshAgent IEnemyMovementComponent.navMesh { get { return _nav; } }

        float IEnemyMovementComponent.sinkSpeed { get { return sinkSpeed; } }
        Transform ITransformComponent.transform { get { return _transform; }}

        public Action removeEntity { get { return _removeAction; } set { _removeAction = value;  }  }

        void Awake ()
        {
            _nav = GetComponent <NavMeshAgent> ();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _transform = transform;
        }
    }
}
