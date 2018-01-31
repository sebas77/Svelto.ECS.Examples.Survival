using Svelto.ECS.Example.Survive.Components.Shared;
using Svelto.ECS.Example.Survive.Components.Camera;

namespace Svelto.ECS.Example.Survive.EntityViews.Camera
{
    public class CameraTargetEntityView: EntityView
    {
        public ICameraTargetComponent targetComponent;
        }
}