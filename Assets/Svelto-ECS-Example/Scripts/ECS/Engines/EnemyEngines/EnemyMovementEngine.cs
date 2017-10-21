using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyMovementEngine : SingleNodeEngine<EnemyTargetNode>, IQueryableNodeEngine, IStep<DamageInfo>
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public EnemyMovementEngine()
        {
            TaskRunner.Instance.Run(new Tasks.TimedLoopActionEnumerator(Tick));
        }

        protected override void Add(EnemyTargetNode node)
        {
            _targetNode = node;
        }

        protected override void Remove(EnemyTargetNode node)
        {
            _targetNode = null;
        }

        void Tick(float deltaSec)
        {
            if (_targetNode == null)
                return;
            
            var enemies = nodesDB.QueryNodes<EnemyNode>();

            for (var i = 0; i < enemies.Count; i++)
            {
                var component = enemies[i].movementComponent;

                if (component.navMesh.isActiveAndEnabled)
                    component.navMesh.SetDestination(_targetNode.targetPositionComponent.position);
            }
        }

        void StopEnemyOnDeath(int targetID)
        {
            EnemyNode node = nodesDB.QueryNode<EnemyNode>(targetID);

            node.movementComponent.navMesh.enabled = false;
            var capsuleCollider = node.movementComponent.capsuleCollider;
            capsuleCollider.isTrigger = true;
            capsuleCollider.GetComponent<Rigidbody>().isKinematic = true;
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            StopEnemyOnDeath(token.entityDamaged);
        }

        EnemyTargetNode   _targetNode;
    }
}
