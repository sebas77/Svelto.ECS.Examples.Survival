namespace Svelto.ECS.Example.Survive.Enemies
{
        public class EnemyEntityView:EntityView
        {
            public IEnemyMovementComponent    movementComponent;
            public IEnemyVFXComponent         vfxComponent;
    
            public IAnimationComponent        animationComponent;
            public ITransformComponent        transformComponent;
            public IPositionComponent         positionComponent;
            public IDestroyComponent          destroyComponent;
            public IEnemySinkComponent        sinkSpeedComponent;
            public IRigidBodyComponent        rigidBodyComponent;
        }
    
    public class EnemyAttackEntityView:EntityView
    {
        public IEnemyTriggerComponent    targetTriggerComponent;
    }

    public class EnemyTargetEntityView : EntityView
    {
        public IPositionComponent         targetPositionComponent;
    }
}
