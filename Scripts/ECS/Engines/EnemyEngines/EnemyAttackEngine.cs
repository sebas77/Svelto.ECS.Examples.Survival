using Components.Damageable;
using Components.Enemy;
using Nodes.Enemies;
using Svelto.DataStructures;
using Svelto.ES;
using Svelto.Ticker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engines.Enemies
{
    public class EnemyAttackEngine : INodesEngine, ITickable
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is EnemyNode)
            {
                EnemyNode enemyNode = (obj as EnemyNode);

                Action<int, bool> callback = delegate (int ID, bool inRange) { CheckTarget(enemyNode.targetTriggerComponent, ID, inRange); };

                _handlers[enemyNode.ID] = callback;

                enemyNode.targetTriggerComponent.entityInRange += callback;

                _enemiesAttackList.Add(enemyNode);
            }
            else
                _targetNode = obj as EnemyTargetNode;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                EnemyNode enemyNode = (obj as EnemyNode);

                _enemiesAttackList.Remove(enemyNode);

                enemyNode.targetTriggerComponent.entityInRange -= _handlers[enemyNode.ID];

                _handlers.Remove(enemyNode.ID);
            }
            else
                _targetNode = null;
        }

        public void Tick(float deltaSec)
        {
            for (int enemyIndex = _enemiesAttackList.Count - 1; enemyIndex >= 0 ; --enemyIndex)
            {
                var enemyAttackNode = _enemiesAttackList[enemyIndex];

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

        void CheckTarget(IEnemyTriggerComponent component, int enemyID, bool inRange)
        {
            if (_targetNode == null)
                return;

            if (enemyID == _targetNode.ID)
            {
                if (inRange)
                    component.targetInRange = true;
                else
                    component.targetInRange = false;
            }
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode                       _targetNode;

        FasterList<EnemyNode>                 _enemiesAttackList = new FasterList<EnemyNode>();
        Dictionary<int, Action<int, bool>>    _handlers = new Dictionary<int, Action<int, bool>>();
    }
}
