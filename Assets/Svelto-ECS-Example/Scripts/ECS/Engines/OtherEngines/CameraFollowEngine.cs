using System;
using System.Collections;
using Svelto.ECS.Example.Survive.EntityViews.Camera;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Camera
{
    //First step identify the entity type we want the engine to handle: CameraEntity
    //Second step name the engine according the behaviour and the entity: I.E.: CameraFollowTargetEngine
    //Third step start to write the code and create classes/fields as needed using refactoring tools 
    public class CameraFollowTargetEngine : IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {
            PhysicUpdate().RunOnSchedule(StandardSchedulers.updateScheduler);
        }
        
        IEnumerator PhysicUpdate()
        {
            var cameraEntityView = entityViewsDB.QueryEntityViews<CameraEntityView>()[0];
            var cameraTargetEntityView = entityViewsDB.QueryEntityViews<CameraTargetEntityView>()[0];
            
            while (cameraEntityView == null || cameraTargetEntityView == null)
            {
                yield return null; //skip a frame
                
                cameraEntityView = entityViewsDB.QueryEntityViews<CameraEntityView>()[0];
                cameraTargetEntityView = entityViewsDB.QueryEntityViews<CameraTargetEntityView>()[0];
            }

            float smoothing = 5.0f;
            
            DateTime then = DateTime.Now;
            
            Vector3 offset = cameraEntityView.transformComponent.position - cameraTargetEntityView.targetComponent.position;
            
            while (true)
            {
                Vector3 targetCameraPos = cameraTargetEntityView.targetComponent.position + offset;

                cameraEntityView.transformComponent.position = Vector3.Lerp(
                    cameraEntityView.transformComponent.position, targetCameraPos, smoothing * (float)(DateTime.Now - then).TotalSeconds);
                
                then = DateTime.Now;

                yield return null;
            }
        }
    }
}
