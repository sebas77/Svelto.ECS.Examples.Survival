namespace Svelto.ECS.Example.Survive.Enemies
{
    public struct EnemyEntityView:IEntityData
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
    
    public struct EnemyAttackEntityView:IEntityData
    {
        public IEnemyTriggerComponent    targetTriggerComponent;
        public EGID ID { get; set; }
    }

    public struct EnemyTargetEntityView : IEntityData
    {
        public IPositionComponent         targetPositionComponent;
        public EGID ID { get; set; }
    }
}
