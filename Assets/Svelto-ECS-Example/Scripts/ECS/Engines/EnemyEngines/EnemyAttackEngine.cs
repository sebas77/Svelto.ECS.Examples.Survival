using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.Tasks;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAttackEngine : MultiNodesEngine<EnemyNode, EnemyTargetNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public EnemyAttackEngine(Sequencer enemyrDamageSequence)
        {
            _targetDamageSequence = enemyrDamageSequence;

            TaskRunner.Instance.Run(new TimedLoopActionEnumerator(Tick));
        }

        protected override void AddNode(EnemyNode obj)
        {
            EnemyNode enemyNode = (obj as EnemyNode);

            enemyNode.targetTriggerComponent.entityInRange += CheckTarget;
        }

        protected override void RemoveNode(EnemyNode obj)
        {
            EnemyNode enemyNode = (obj as EnemyNode);

                enemyNode.targetTriggerComponent.entityInRange -= CheckTarget;
        }

        protected override void AddNode(EnemyTargetNode obj)
        {
            _targetNode = obj as EnemyTargetNode;
        }

        protected override void RemoveNode(EnemyTargetNode obj)
        {
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

        EnemyTargetNode _targetNode;
        Sequencer _targetDamageSequence;
    }
}
