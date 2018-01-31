using Svelto.ECS.Example.Survive.Components.Shared;
using Svelto.ECS.Example.Survive.Components.Gun;
using Svelto.ECS.Example.Survive.Components.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.EntityViews.Player
{
    public class PlayerEntityView : EntityView
    {
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
        public IPlayerInputComponent   inputComponent;
        public ITransformComponent     transformComponent;
    }

    public interface IPlayerInputComponent
    {
        Vector3 input { get; set; }
        Ray camRay { get; set; }
        bool fire { get; set; }
    }

    public class PlayerTargetEntityView : EntityView
    {
        public ITargetTypeComponent     targetTypeComponent;
    }
}

namespace Svelto.ECS.Example.Survive.EntityViews.Gun
{
    public class GunEntityView : EntityView
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
