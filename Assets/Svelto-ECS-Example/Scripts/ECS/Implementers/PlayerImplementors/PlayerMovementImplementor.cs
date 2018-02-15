using Svelto.ECS.Example.Survive.Camera;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerMovementImplementor : MonoBehaviour, IImplementor,
        IRigidBodyComponent,
        IPositionComponent,
        IAnimationComponent,
        ICameraTargetComponent,
        ISpeedComponent,
        ITransformComponent
    {
        public float speed = 6f;            // The speed that the player will move at.

        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        Transform playerTransform;

        public bool isKinematic { set { playerRigidbody.isKinematic = value; } }
        public Quaternion rotation { set {playerRigidbody.MoveRotation(value);} }

        public float       movementSpeed { get { return speed; } }
        
        public void setBool(string name, bool value)
        {
            anim.SetBool(name, value);
        }

        public string trigger { set {anim.SetTrigger(value);} }

        void Awake ()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
            playerTransform = transform;
        }

        public Vector3 position { get { return playerTransform.position; }  set {playerRigidbody.MovePosition(value);} }
    }
}
