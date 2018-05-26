namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerEntityView : IEntityView
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
    public struct GunEntityView : IEntityView
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
        public EGID ID { get; set; }
    }
}
