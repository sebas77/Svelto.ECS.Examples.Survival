namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerEntityView : IEntityViewStruct
    {
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
        public ITransformComponent     transformComponent;
        public EGID ID { get; set; }
    }
}

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public struct GunEntityView : IEntityViewStruct
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
        public EGID ID { get; set; }
    }
}
