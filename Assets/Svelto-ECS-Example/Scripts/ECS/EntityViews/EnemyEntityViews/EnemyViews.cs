namespace Svelto.ECS.Example.Survive.Enemies
{
    public struct EnemyEntityView:IEntityView
    {
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;
    
        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
        public IPositionComponent         positionComponent;
        public IDestroyComponent          destroyComponent;
        public IEnemySinkComponent        sinkSpeedComponent;
        public IRigidBodyComponent        rigidBodyComponent;
        public EGID ID { get; set; }
    }
    
    public struct EnemyAttackEntityView:IEntityView
    {
        public IEnemyTriggerComponent    targetTriggerComponent;
        public EGID ID { get; set; }
    }

    public struct EnemyTargetEntityView : IEntityView
    {
        public IPositionComponent         targetPositionComponent;
        public EGID ID { get; set; }
    }
}
