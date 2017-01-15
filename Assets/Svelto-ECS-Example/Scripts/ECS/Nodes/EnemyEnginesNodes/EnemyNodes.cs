using Svelto.ECS.Example.Components.Base;
using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Components.Enemy;

namespace Svelto.ECS.Example.Nodes.Enemies
{
    public class EnemyNode: NodeWithID
    {
        public IEnemyAttackComponent      attackComponent;
        public IEnemyAttackDataComponent  attackDamageComponent;
        public IEnemyTriggerComponent     targetTriggerComponent;
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;

        public IHealthComponent           healthComponent;
        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
    }

    public class EnemySpawningNode : NodeWithID
    {
        public IEnemySpawnerComponent[] spawnerComponents;
    }

    public class EnemyTargetNode: NodeWithID
    {
        public IPositionComponent       targetPositionComponent;
        public IHealthComponent         healthComponent;
    }
}
