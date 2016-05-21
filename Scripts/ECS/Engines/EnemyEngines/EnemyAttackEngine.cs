using Components.Damageable;
using Nodes.Enemies;
using Svelto.ES;
using Svelto.Ticker;
using System;
using UnityEngine;

namespace Engines.Enemies
{
    public class EnemyAttackEngine : INodesEngine, ITickable, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }
        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj)
        {
            if (obj is EnemyNode)
            {
                EnemyNode enemyNode = (obj as EnemyNode);

                enemyNode.targetTriggerComponent.entityInRange += CheckTarget;
            }
            else
                _targetNode = obj as EnemyTargetNode;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                EnemyNode enemyNode = (obj as EnemyNode);

                enemyNode.targetTriggerComponent.entityInRange -= CheckTarget;
            }
            else
                _targetNode = null;
        }

        public void Tick(float deltaSec)
        {
            var enemiesAttackList = nodesDB.QueryNodes<EnemyNode>();

            for (int enemyIndex = enemiesAttackList.Count - 1; enemyIndex >= 0 ; --enemyIndex)
            {
                var enemyAttackNode = enemiesAttackList[enemyIndex];

                if (enemyAttackNode.attackComponent.targetInRange == true)
                {
                    var attackDamageComponent = enemyAttackNode.attackDamageComponent;
					    attackDamageComponent.timer += deltaSec;

                    if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                    {
                        attackDamageComponent.timer = 0.0f;

                        if (_targetNode != null)
                        {
                            var damageInfo = new DamageInfo(attackDamageComponent.damage, Vector3.zero);

                            _targetNode.damageEventComponent.damageReceived.Dispatch(ref damageInfo);
                        }
                    }
                }
            }
        }

        void CheckTarget(int targetID, int enemyID, bool inRange)
        {
            if (_targetNode == null)
                return;

            if (targetID == _targetNode.ID)
            {
                var enemyNode = nodesDB.QueryNode<EnemyNode>(enemyID);
                var component = enemyNode.targetTriggerComponent;

                if (inRange)
                    component.targetInRange = true;
                else
                    component.targetInRange = false;
            }
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode _targetNode;
    }
}
