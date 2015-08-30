using EnemyComponents;
using SharedComponents;
using Svelto.ES;
using UnityEngine;

namespace EnemyEngines
{
    public class EnemyNode: INodeWithReferenceID<GameObject>
    {
        public IEnemyAttackComponent      attackComponent;
        public IEnemyAttackDataComponent  attackDamageComponent;
        public IEnemyTriggerComponent     targetTriggerComponent;
        public IEnemyMovementComponent    movementComponent;
        public IEnemyVFXComponent         vfxComponent;
        
        public IHealthComponent           healthComponent;
        public IAnimationComponent        animationComponent;
        public ITransformComponent        transformComponent;
                
        public EnemyNode(GameObject ID) { this.ID = ID; }
        public GameObject ID { get; private set; }
    }

    public class EnemySpawningNode : INodeWithReferenceID<GameObject>
    {
        public IEnemySpawnerComponent[] spawnerComponents;

        public EnemySpawningNode(GameObject ID) { this.ID = ID; }
        public GameObject ID { get; private set; }
    }

    public class EnemyTargetNode: INodeWithReferenceID<GameObject>
    {
        public IDamageEventComponent    damageEventComponent;
        public IPositionComponent       targetPositionComponent;
        public IHealthComponent         healthComponent;

        public EnemyTargetNode(GameObject ID) { this.ID = ID; }
        public GameObject ID { get; private set; }
    }
}
