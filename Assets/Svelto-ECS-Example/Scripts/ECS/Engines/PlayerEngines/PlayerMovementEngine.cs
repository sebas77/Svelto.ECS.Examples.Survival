using System.Collections;
using Svelto.ECS.Example.Survive.HUD;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Characters.Player
{
    public class PlayerMovementEngine : SingleEntityEngine<PlayerEntityViewStruct>, IQueryingEntitiesEngine, IStep<PlayerDeathCondition>
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {}
        
        public PlayerMovementEngine(IRayCaster raycaster, ITime time)
        {
            _rayCaster = raycaster;
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicsTick()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(ref PlayerEntityViewStruct entityView)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(ref PlayerEntityViewStruct entityView)
        {
            _taskRoutine.Stop();
        }
        
        IEnumerator PhysicsTick()
        {  
            int targetsCount;
            var playerEntityViews = entitiesDB.QueryEntities<PlayerEntityViewStruct>(out targetsCount);
            var playerInputDatas = entitiesDB.QueryEntities<PlayerInputDataStruct>(out targetsCount);

            while (true)
            {   
                Movement(ref playerInputDatas[0], ref playerEntityViews[0]);
                Turning(ref playerInputDatas[0], ref playerEntityViews[0]);

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
        /// <param name="entityView"></param>
        void Movement(ref PlayerInputDataStruct playerEntityView, ref PlayerEntityViewStruct entityView)
        {
            // Store the input axes.
            Vector3 input = playerEntityView.input;
            
            // Normalise the movement vector and make it proportional to the speed per second.
            Vector3 movement = input.normalized * entityView.speedComponent.movementSpeed * _time.deltaTime;

            // Move the player to it's current position plus the movement.
            entityView.transformComponent.position = entityView.positionComponent.position + movement;
        }

        void Turning(ref PlayerInputDataStruct playerEntityView, ref PlayerEntityViewStruct entityView)
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = playerEntityView.camRay;
            
            // Perform the raycast and if it hits something on the floor layer...
            Vector3 point;
            if (_rayCaster.CheckHit(camRay, camRayLength, floorMask, out point) != -1)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = point - entityView.positionComponent.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                entityView.transformComponent.rotation = newRotatation;
            }
        }

        void StopMovementOnDeath(EGID ID)
        {
            int count;
            var playerEntityView = entitiesDB.QueryEntities<PlayerEntityViewStruct>(out count)[0]; 
            playerEntityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            StopMovementOnDeath(id);
        }

        readonly int floorMask = LayerMask.GetMask("Floor");    // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        const float camRayLength = 100f;                        // The length of the ray from the camera into the scene.

        IRayCaster   _rayCaster;
        ITaskRoutine _taskRoutine;
        ITime        _time;
    }
}
