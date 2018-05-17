using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerMovementEngine : SingleEntityViewEngine<PlayerEntityView>, IStep<DamageInfo>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {}
        
        public PlayerMovementEngine(IRayCaster raycaster, ITime time)
        {
            _rayCaster = raycaster;
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicsTick()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
        }
        
        IEnumerator PhysicsTick()
        {  
            //some safe assumption here: I assume that the player entity is created
            //and added in the EnginesRoot when this code runs.
            //I assume that there is just one player entity in the array of entities.
            var playerEntityViews = entityViewsDB.QueryEntities<PlayerEntityView>();
            var playerEntityView = playerEntityViews[0];
            
            while (true)
            {   
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
            Vector3 input = playerEntityView.inputComponent.input;
            
            // Normalise the movement vector and make it proportional to the speed per second.
            Vector3 movement = input.normalized * playerEntityView.speedComponent.movementSpeed * _time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerEntityView.transformComponent.position = playerEntityView.positionComponent.position + movement;
        }

        void Turning(PlayerEntityView playerEntityView)
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = playerEntityView.inputComponent.camRay;
            
            // Perform the raycast and if it hits something on the floor layer...
            Vector3 point;
            if (_rayCaster.CheckHit(camRay, camRayLength, floorMask, out point) != -1)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = point - playerEntityView.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerEntityView.transformComponent.rotation = newRotatation;
            }
        }

        void StopMovementOnDeath(EGID ID)
        {
            var playerEntityView = entityViewsDB.QueryEntities<PlayerEntityView>()[0]; 
            playerEntityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            StopMovementOnDeath(token.entityDamagedID);
        }

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.

        IRayCaster   _rayCaster;
        ITaskRoutine _taskRoutine;
        ITime        _time;
    }
}
