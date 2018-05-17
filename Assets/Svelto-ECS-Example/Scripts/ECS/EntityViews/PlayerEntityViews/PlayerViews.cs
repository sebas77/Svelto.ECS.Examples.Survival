using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerEntityView : IEntityData
    {
        public IPlayerInputComponent inputComponent;
        
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
        public ITransformComponent     transformComponent;
        public EGID ID { get; set; }
    }

    public interface IPlayerInputComponent
    {
        Vector3 input { get; set; }
        Ray camRay { get; set; }
        bool fire { get; set; }
    }

    public struct PlayerTargetEntityView : IEntityData
    {
        public IPlayerTargetComponent     playerTargetComponent;
        public EGID ID { get; set; }
    }
}

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public struct GunEntityView : IEntityData
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
        public EGID ID { get; set; }
    }
}
