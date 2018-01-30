using Svelto.ECS.Example.Survive.EntityViews.Player;
using System.Collections;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Others;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    public class PlayerMovementEngine : SingleEntityViewEngine<PlayerEntityView>, IStep<DamageInfo>
    {
        public PlayerMovementEngine(IRayCaster raycaster, ITime time)
        {
            _rayCaster = raycaster;
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicsTick()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
            _taskRoutine.Start();
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerEntityView = null;
        }
        
        IEnumerator PhysicsTick()
        {
            while (true)
            {   //this is not a defensive if. Engines are meant to handle every case
                //including no entities added or left
                Movement(_playerEntityView);
                Turning();

                yield return null; //don't forget to yield or you will enter in an infinite loop!
            }
        }

        /// <summary>
        /// In order to keep the class testable, we need to reduce the number of
        /// dependencies injected through static classes at its minimum.
        /// Implementors are the place where platform dependencies can be transformed into
        /// entity components, so that here we can use inputComponent instead of
        /// the class Input.
        /// </summary>
        /// <param name="playerEntityView"></param>
        void Movement(PlayerEntityView playerEntityView)
        {
            // Store the input axes.
            Vector3 movement = playerEntityView.inputComponent.input;
            
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * playerEntityView.speedComponent.movementSpeed * _time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerEntityView.transformComponent.position = playerEntityView.positionComponent.position + movement;
        }

        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = _playerEntityView.inputComponent.camRay;
            
            // Perform the raycast and if it hits something on the floor layer...
            Vector3 point;
            if (_rayCaster.CheckHit(camRay, camRayLength, floorMask, out point) != -1)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = point - _playerEntityView.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                _playerEntityView.transformComponent.rotation = newRotatation;
            }
        }

        void StopMovementOnDeath(int ID)
        {
            _playerEntityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            StopMovementOnDeath(token.entityDamaged);
        }

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.

        IRayCaster _rayCaster;
        PlayerEntityView _playerEntityView;
        ITaskRoutine _taskRoutine;
        ITime _time;
    }
}
