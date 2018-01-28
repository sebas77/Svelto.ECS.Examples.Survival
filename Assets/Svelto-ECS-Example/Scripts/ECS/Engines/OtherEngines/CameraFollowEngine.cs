using System;
using System.Collections;
using Svelto.ECS.Example.Survive.EntityViews.Camera;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Camera
{
    //First step identify the entity type we want the engine to handle: CameraEntity
    //Second step name the engine according the behaviour and the entity: I.E.: CameraFollowTargetEngine
    //Third step identify secondary entities needed if necessary: CameraTargetEntity
    public class CameraFollowTargetEngine : MultiEntityViewsEngine<CameraEntityView, CameraTargetEntityView>
    {
        public CameraFollowTargetEngine()
        {
            PhysicUpdate().RunOnSchedule(StandardSchedulers.updateScheduler);
        }
        
        protected override void Add(CameraEntityView entityView)
        {
            _cameraEntityView = entityView;
        }

        protected override void Remove(CameraEntityView entityView)
        {
        }

        protected override void Add(CameraTargetEntityView entityView)
        {
            _cameraTargetEntityView = entityView;
        }

        protected override void Remove(CameraTargetEntityView entityView)
        {
        }

        IEnumerator PhysicUpdate()
        {
            float smoothing = 5.0f;
            
            DateTime then = DateTime.Now;

            while (_cameraEntityView == null || _cameraTargetEntityView == null)
                yield return null;
            
            Vector3 offset = _cameraEntityView.transformComponent.position - _cameraTargetEntityView.targetComponent.position;
            
            while (true)
            {
                Vector3 targetCameraPos = _cameraTargetEntityView.targetComponent.position + offset;

                _cameraEntityView.transformComponent.position = Vector3.Lerp(
                    _cameraEntityView.transformComponent.position, targetCameraPos, (smoothing * (float)(DateTime.Now - then).TotalSeconds));
                
                then = DateTime.Now;

                yield return null;
            }
        }
        
        CameraEntityView       _cameraEntityView;
        CameraTargetEntityView _cameraTargetEntityView;
    }
}
