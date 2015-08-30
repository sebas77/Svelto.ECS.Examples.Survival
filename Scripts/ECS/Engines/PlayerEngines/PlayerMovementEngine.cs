using System;
using Svelto.ES;
using Svelto.Ticker;
using UnitySampleAssets.CrossPlatformInput;
using UnityEngine;
using SharedComponents;

namespace PlayerEngines
{
    public class PlayerMovementEngine : INodeEngine, IPhysicallyTickable
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            _playerNode = obj as PlayerNode;

            _playerNode.healthComponent.isDead.observers += StopMovementOnDeath;
        }

        public void Remove(INode obj)
        {
            _playerNode.healthComponent.isDead.observers -= StopMovementOnDeath;
            _playerNode = null;
        }

        public void PhysicsTick(float deltaSec)
        {
            if (_playerNode == null) return;

            Movement();
            Turning();
        }

        private void Movement()
        {
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3();

            movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * _playerNode.speedComponent.speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            _playerNode.rigidBodyComponent.rigidbody.MovePosition(_playerNode.positionComponent.position + movement);
        }

        private void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - _playerNode.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                _playerNode.rigidBodyComponent.rigidbody.MoveRotation(newRotatation);
            }
        }

        void StopMovementOnDeath(IHealthComponent arg1, GameObject arg2)
        {
            _playerNode.rigidBodyComponent.rigidbody.isKinematic = true;

            Remove(_playerNode);
        }

        Type[]              _acceptedNodes = new Type[1] { typeof(PlayerNode) };
        PlayerNode          _playerNode;

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.
    }
}
