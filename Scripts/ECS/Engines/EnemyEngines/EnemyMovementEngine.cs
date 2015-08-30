using SharedComponents;
using Svelto.ES;
using Svelto.Ticker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyEngines
{
    public class EnemyMovementEngine : INodeEngine, ITickable
    {
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

                healthEventsComponent.isDead.observers += StopEnemyOnDeath;

                _enemyNodes.Add(healthEventsComponent, enemyNode);
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

                RemoveComponent(healthEventsComponent);
            }
            else
                _targetNode = null;
        }

        public void Tick(float deltaSec)
        {
            for (var en = _enemyNodes.Values.GetEnumerator(); en.MoveNext();)
            {
                var component = en.Current.movementComponent;

                component.navMesh.SetDestination(_targetNode.targetPositionComponent.position);
            }
        }

        void RemoveComponent(IHealthComponent healthEventsComponent)
        {
            if (healthEventsComponent != null)
                healthEventsComponent.isDead.observers -= StopEnemyOnDeath;

            _enemyNodes.Remove(healthEventsComponent);
        }

        void StopEnemyOnDeath(IHealthComponent sender, GameObject target)
        {
            EnemyNode node = _enemyNodes[sender];

            node.movementComponent.navMesh.enabled = false;
            var capsuleCollider = node.movementComponent.capsuleCollider;
            capsuleCollider.isTrigger = true;
            capsuleCollider.GetComponent<Rigidbody>().isKinematic = true;

            RemoveComponent(sender);
        }

        Type[] _acceptedNodes = new Type[2] { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode                           _targetNode;
        Dictionary<IHealthComponent, EnemyNode>   _enemyNodes = new Dictionary<IHealthComponent, EnemyNode>();
    }
}
