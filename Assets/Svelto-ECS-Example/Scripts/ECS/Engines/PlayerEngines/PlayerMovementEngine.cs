using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using System.Collections;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    public class PlayerMovementEngine : IQueryingEntityViewEngine, IStep<TargetDamageInfo>
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        
        /// <summary>
        /// Start the routine that uses entityViewsDB only when we are sure
        /// that this is available
        /// </summary>
        public void Ready()
        {
            PhysicsTick().RunOnSchedule(StandardSchedulers.physicScheduler);
        }

        IEnumerator PhysicsTick()
        {
            //we are obviously assuming there is always going to be just a player entity only
            var playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];

            while (playerEntityView == null)
            {
                yield return null; //skip a frame
                
                playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            }

            while (true)
            {   //this is not a defensive if. Engines are meant to handle every case
                //including no entities added or left
                Movement(playerEntityView);
                Turning(playerEntityView);

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
            movement = movement.normalized * playerEntityView.speedComponent.speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerEntityView.rigidBodyComponent.position = playerEntityView.positionComponent.position + movement;
        }

        void Turning(PlayerEntityView playerEntityView)
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = playerEntityView.inputComponent.camRay;
            
            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - playerEntityView.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerEntityView.rigidBodyComponent.rotation = newRotatation;
            }
        }

        void StopMovementOnDeath(int ID)
        {
            var playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            
            playerEntityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref TargetDamageInfo token, int condition)
        {
            StopMovementOnDeath(token.entityDamaged);
        }

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.
    }
}
