namespace Svelto.ECS.Example.Survive.Enemies
{
    public struct EnemyEntityViewStruct:IEntityView
    {
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;
    
        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
        public IPositionComponent         positionComponent;
        public IEnemySinkComponent        sinkSpeedComponent;
        public IRigidBodyComponent        rigidBodyComponent;
        public EGID ID { get; set; }
    }
    
    public struct EnemyAttackEntityView:IEntityView
    {
        public IEnemyTriggerComponent    targetTriggerComponent;
        public EGID ID { get; set; }
    }

    public struct EnemyTargetEntityViewStruct : IEntityView
    {
        public IPositionComponent         targetPositionComponent;
        public EGID ID { get; set; }
    }
}
