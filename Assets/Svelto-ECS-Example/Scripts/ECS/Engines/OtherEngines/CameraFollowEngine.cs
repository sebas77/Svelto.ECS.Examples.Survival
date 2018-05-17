using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Camera
{
    //First step identify the entity type we want the engine to handle: CameraEntity
    //Second step name the engine according the behaviour and the entity: I.E.: CameraFollowTargetEngine
    //Third step start to write the code and create classes/fields as needed using refactoring tools 
    public class CameraFollowTargetEngine : MultiEntityViewsEngine<CameraEntityView, CameraTargetEntityView>
    {
        public CameraFollowTargetEngine(ITime time)
        {
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicUpdate()).SetScheduler(StandardSchedulers.physicScheduler);
            _taskRoutine.Start();
        }
        
        protected override void Add(CameraEntityView entityView)
        {
            _cameraEntityView = entityView;
        }

        protected override void Remove(CameraEntityView entityView)
        {
            _taskRoutine.Stop();
            _cameraEntityView = null;
        }

        protected override void Add(CameraTargetEntityView entityView)
        {
            _cameraTargetEntityView = entityView;
        }

        protected override void Remove(CameraTargetEntityView entityView)
        {
            _taskRoutine.Stop();
            _cameraTargetEntityView = null;
        }
        
        IEnumerator PhysicUpdate()
        {
            while (_cameraEntityView == null || _cameraTargetEntityView == null)
                yield return null; //skip a frame

            float smoothing = 5.0f;
            
            Vector3 offset = _cameraEntityView.positionComponent.position - _cameraTargetEntityView.targetComponent.position;
            
            while (true)
            {
                Vector3 targetCameraPos = _cameraTargetEntityView.targetComponent.position + offset;

                _cameraEntityView.transformComponent.position = Vector3.Lerp(
                    _cameraEntityView.positionComponent.position, targetCameraPos, smoothing * _time.deltaTime);
                
                yield return null;
            }
        }

        readonly ITime         _time;
        CameraTargetEntityView _cameraTargetEntityView;
        CameraEntityView       _cameraEntityView;
        readonly ITaskRoutine  _taskRoutine;
    }
}
