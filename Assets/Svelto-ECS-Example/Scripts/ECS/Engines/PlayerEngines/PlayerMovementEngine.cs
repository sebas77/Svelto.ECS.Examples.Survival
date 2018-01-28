using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using System.Collections;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    public class PlayerMovementEngine : SingleEntityViewEngine<PlayerEntityView>, IStep<TargetDamageInfo>
    {
        public PlayerMovementEngine()
        {
            PhysicsTick().RunOnSchedule(StandardSchedulers.physicScheduler);
        }

        protected override void Add(PlayerEntityView obj)
        {
            _playerEntityView = obj;
        }

        protected override void Remove(PlayerEntityView obj)
        {
            _playerEntityView = null;
        }

        IEnumerator PhysicsTick()
        {
            while (true)
            {   //this is not a defensive if. Engines are meant to handle every case
                //including no entities added or left
                if (_playerEntityView != null)
                {
                    Movement();
                    Turning();
                }

                yield return null; //don't forget to yield or you will enter in an infinite loop!
            }
        }

        void Movement()
        {
            // Store the input axes.
            Vector3 movement = _playerEntityView.inputComponent.input;
            
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * _playerEntityView.speedComponent.speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            _playerEntityView.rigidBodyComponent.position = _playerEntityView.positionComponent.position + movement;
        }

        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = _playerEntityView.inputComponent.camRay;
            
            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - _playerEntityView.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                _playerEntityView.rigidBodyComponent.rotation = newRotatation;
            }
        }

        void StopMovementOnDeath(int ID)
        {
            _playerEntityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref TargetDamageInfo token, int condition)
        {
            StopMovementOnDeath(token.entityDamaged);
        }

        PlayerEntityView      _playerEntityView;

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.
    }
}
