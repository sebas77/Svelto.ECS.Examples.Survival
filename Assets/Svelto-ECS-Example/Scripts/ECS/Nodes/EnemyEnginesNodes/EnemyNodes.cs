using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Enemies;

namespace Svelto.ECS.Example.Survive.Nodes.Enemies
{
    public class EnemyNode: NodeWithID
    {
        public IEnemyAttackComponent      attackComponent;
        public IEnemyAttackDataComponent  attackDamageComponent;
        public IEnemyTriggerComponent     targetTriggerComponent;
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;

        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
    }

    public class EnemySpawningNode : NodeWithID
    {
        public IEnemySpawnerComponent     spawnerComponent;
    }

    public class EnemyTargetNode: NodeWithID
    {
        public IPositionComponent         targetPositionComponent;
    }
}
