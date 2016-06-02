using Components.Base;
using Components.Damageable;
using Components.Enemy;
using Svelto.ES;

namespace Nodes.Enemies
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

        public IRemoveEntityComponent     removeEntityComponent;
    }

    public class EnemySpawningNode : NodeWithID
    {
        public IEnemySpawnerComponent[] spawnerComponents;
    }

    public class EnemyTargetNode: NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IPositionComponent       targetPositionComponent;
        public IHealthComponent         healthComponent;
    }
}
