using EnemyComponents;
using SharedComponents;
using Svelto.DataStructures;
using Svelto.ES;
using Svelto.Ticker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyEngines
{
    public class EnemyAttackEngine : INodeEngine, ITickable
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

                Action<GameObject, bool> callback = delegate (GameObject g, bool inRange) { CheckTarget(enemyNode.targetTriggerComponent, g, inRange); };

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

                if (enemyAttackNode.attackComponent.playerInRange == true)
                {
                    var attackDamageComponent = enemyAttackNode.attackDamageComponent;

                    attackDamageComponent.timer += deltaSec;

                    if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                    {
                        attackDamageComponent.timer = 0.0f;

                        if (_targetNode != null)
                            _targetNode.damageEventComponent.damageReceived.Dispatch(new DamageInfo(attackDamageComponent.damage, Vector3.zero));
                    }
                }
            }
        }

        void CheckTarget(IEnemyTriggerComponent component, GameObject obj, bool inRange)
        {
            if (_targetNode == null)
                return;

            if (obj == _targetNode.ID)
            {
                if (inRange)
                    component.playerInRange = true;
                else
                    component.playerInRange = false;
            }
        }

        Type[] _acceptedNodes = new Type[2] { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode         _targetNode; 

        FasterList<EnemyNode>                               _enemiesAttackList = new FasterList<EnemyNode>();
        Dictionary<GameObject, Action<GameObject, bool>>    _handlers = new Dictionary<GameObject, Action<GameObject, bool>>();
    }
}
