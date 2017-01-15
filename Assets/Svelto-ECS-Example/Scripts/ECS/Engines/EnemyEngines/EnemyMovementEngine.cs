using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.Ticker.Legacy;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Engines.Enemies
{
    public class EnemyMovementEngine : INodesEngine, ITickable, IQueryableNodeEngine, IStep<DamageInfo>
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is EnemyTargetNode == true)
                _targetNode = obj as EnemyTargetNode;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyTargetNode == true)
                _targetNode = null;
        }

        public void Tick(float deltaSec)
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

        readonly Type[] _acceptedNodes = { typeof(EnemyTargetNode) };

        EnemyTargetNode   _targetNode;
    }
}
