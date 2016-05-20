using Nodes.Enemies;
using Svelto.ES;
using Svelto.Ticker;
using System;
using UnityEngine;

namespace Engines.Enemies
{
    public class EnemyMovementEngine : INodesEngine, ITickable, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is EnemyNode)
            {
                var enemyNode = obj as EnemyNode;
                var healthEventsComponent = enemyNode.healthComponent;

                healthEventsComponent.isDead.subscribers += StopEnemyOnDeath;
            }
            else
                _targetNode = obj as EnemyTargetNode;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                var enemyNode = obj as EnemyNode;
                var healthEventsComponent = enemyNode.healthComponent;

                healthEventsComponent.isDead.subscribers -= StopEnemyOnDeath;
            }
            else
                _targetNode = null;
        }

        public void Tick(float deltaSec)
        {
            var enemies = nodesDB.QueryNodes<EnemyNode>();

            for (var i = 0; i < enemies.Count; i++)
            {
                var component = enemies[i].movementComponent;

                if (component.navMesh.isActiveAndEnabled)
                    component.navMesh.SetDestination(_targetNode.targetPositionComponent.position);
            }
        }

        void StopEnemyOnDeath(int sender, int targetID)
        {
            EnemyNode node = nodesDB.QueryNode<EnemyNode>(sender);

            node.movementComponent.navMesh.enabled = false;
            var capsuleCollider = node.movementComponent.capsuleCollider;
            capsuleCollider.isTrigger = true;
            capsuleCollider.GetComponent<Rigidbody>().isKinematic = true;
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode              _targetNode;
    }
}
