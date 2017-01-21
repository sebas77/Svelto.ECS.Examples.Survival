using Svelto.ECS.Example.Components.Base;
using UnityEngine;

namespace Svelto.ECS.Example.Implementers.Player
{
    public class PlayerMovement : MonoBehaviour,
        IRigidBodyComponent,
        IPositionComponent,
        IAnimationComponent,
        ISpeedComponent
    {
        public float speed = 6f;            // The speed that the player will move at.

        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        Transform playerTransform;

        Vector3     IPositionComponent.position { get { return playerTransform.position; } }
        Rigidbody   IRigidBodyComponent.rigidbody { get { return playerRigidbody; } }
        float       ISpeedComponent.speed { get { return speed; } }
        Animator    IAnimationComponent.animation { get { return anim; } }

        void Awake ()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
            playerTransform = transform;
        }
    }
}
