using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.Tasks;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Engines.Enemies
{
    public class EnemyAttackEngine : INodesEngine, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }
        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public EnemyAttackEngine(Sequencer enemyrDamageSequence)
        {
            _targetDamageSequence = enemyrDamageSequence;

            TaskRunner.Instance.Run(new TimedLoopActionEnumerator(Tick));
        }

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

        void Tick(float deltaSec)
        {
            if (_targetNode == null) return;

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

                        var damageInfo = new PlayerDamageInfo(attackDamageComponent.damage, Vector3.zero, _targetNode.ID);

                        _targetDamageSequence.Next(this, ref damageInfo);
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
        Sequencer _targetDamageSequence;
    }
}
