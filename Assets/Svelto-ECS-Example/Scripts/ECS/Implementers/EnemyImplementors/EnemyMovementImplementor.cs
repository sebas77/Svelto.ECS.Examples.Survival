using System;
using UnityEngine;
using UnityEngine.AI;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyMovementImplementor : MonoBehaviour, IImplementor, IEnemyMovementComponent, ITransformComponent, IRigidBodyComponent
    {
        NavMeshAgent    _nav;                       // Reference to the nav mesh agent.
        CapsuleCollider _capsuleCollider;           // Reference to the capsule collider.
        Transform       _transform;
        Action          _removeAction;
        Rigidbody       _rigidBody;

        public bool navMeshEnabled { set { _nav.enabled = value; } }
        public Vector3 navMeshDestination { set { _nav.destination = value;} }
        public bool setCapsuleAsTrigger { set {_capsuleCollider.isTrigger = value; } }

        void Awake ()
        {
            _nav = GetComponent <NavMeshAgent> ();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
        }

        public Vector3 position
        {
            get { return _transform.position; }
            set { _transform.position = value; }
        }

        public Quaternion rotation
        {
            set { _transform.rotation = value; }
        }

        public bool isKinematic { set {_rigidBody.isKinematic = value; } }
    }
}
