namespace Svelto.ECS.Example.Survive.Enemies
{
    public struct EnemyEntityViewStruct:IEntityViewStruct
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
    
    public struct EnemyAttackEntityView:IEntityViewStruct
    {
        public IEnemyTriggerComponent    targetTriggerComponent;
        public EGID ID { get; set; }
    }

    public struct EnemyTargetEntityViewStruct : IEntityViewStruct
    {
        public IPositionComponent         targetPositionComponent;
        public EGID ID { get; set; }
    }
}
