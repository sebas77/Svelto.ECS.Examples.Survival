using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Camera
{
    //First step identify the entity type we want the engine to handle: CameraEntity
    //Second step name the engine according the behaviour and the entity: I.E.: CameraFollowTargetEngine
    //Third step start to write the code and create classes/fields as needed using refactoring tools 
    public class CameraFollowTargetEngine : MultiEntitiesEngine<CameraEntityView, CameraTargetEntityView>, IQueryingEntityViewEngine
    {
        public CameraFollowTargetEngine(ITime time)
        {
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicUpdate()).SetScheduler(StandardSchedulers.physicScheduler);
        }
        
        public void Ready()
        {
            _taskRoutine.Start();            
        }
        
        protected override void Add(ref CameraEntityView entityView)
        {}

        protected override void Remove(ref CameraEntityView entityView)
        {
            _taskRoutine.Stop();
        }

        protected override void Add(ref CameraTargetEntityView entityView)
        {}

        protected override void Remove(ref CameraTargetEntityView entityView)
        {
            _taskRoutine.Stop();
        }
        
        IEnumerator PhysicUpdate()
        {
            while (entityViewsDB.Has<CameraTargetEntityView>() == false || entityViewsDB.Has<CameraEntityView>() == false)
            {
                yield return null; //skip a frame
            }
            
            CameraTargetEntityView cameraTargetEntityView; entityViewsDB.Fetch(out cameraTargetEntityView);
            CameraEntityView cameraEntityView; entityViewsDB.Fetch(out cameraEntityView);

            float smoothing = 5.0f;
            
            Vector3 offset = cameraEntityView.positionComponent.position - cameraTargetEntityView.targetComponent.position;
            
            while (true)
            {
                Vector3 targetCameraPos = cameraTargetEntityView.targetComponent.position + offset;

                cameraEntityView.transformComponent.position = Vector3.Lerp(
                    cameraEntityView.positionComponent.position, targetCameraPos, smoothing * _time.deltaTime);
                
                yield return null;
            }
        }

        readonly ITime         _time;
        readonly ITaskRoutine  _taskRoutine;
        public IEntityViewsDB entityViewsDB { get; set; }
    }
}
