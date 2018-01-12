using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Enemies;

namespace Svelto.ECS.Example.Survive.EntityViews.Enemies
{
    public class EnemyEntityView:EntityView
    {
        public IEnemyAttackComponent      attackComponent;
        public IEnemyAttackDataComponent  attackDamageComponent;
        public IEnemyTriggerComponent     targetTriggerComponent;
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;

        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
    }

    public class EnemyTargetEntityView : EntityView
    {
        public IPositionComponent         targetPositionComponent;
    }
}
