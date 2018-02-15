using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
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
        public IPlayerTargetComponent     playerTargetComponent;
    }
}

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class GunEntityView : EntityView
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
