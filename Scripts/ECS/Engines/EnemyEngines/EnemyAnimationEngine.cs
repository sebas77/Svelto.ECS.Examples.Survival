using System;
using Svelto.ES;
using System.Collections;
using UnityEngine;
using Nodes.Enemies;
using Components.Damageable;

namespace Engines.Enemies
{
    public class EnemyAnimationEngine : INodesEngine, IQueryableNodeEngine
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

                healthEventsComponent.isDead.subscribers += TriggerDeathAnimation;
                healthEventsComponent.isDamaged.subscribers += EntityDamaged;
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.subscribers += OnTargetDead;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                var healthEventsComponent = (obj as EnemyNode).healthComponent;

                if (healthEventsComponent != null)
                {
                    healthEventsComponent.isDead.subscribers -= TriggerDeathAnimation;
                    healthEventsComponent.isDamaged.subscribers -= EntityDamaged;
                }
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.subscribers -= OnTargetDead;
        }

        void EntityDamaged(int sender, DamageInfo damageInfo)
        {
            var node = nodesDB.QueryNode<EnemyNode>(sender);

            node.vfxComponent.hitParticles.transform.position = damageInfo.damagePoint;
            node.vfxComponent.hitParticles.Play();
        }

        void OnTargetDead(int targetID)
        {
            var nodes = nodesDB.QueryNodes<EnemyNode>();

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                node.animationComponent.animation.SetTrigger("PlayerDead");
            }
        }

        void TriggerDeathAnimation(int targetID)
        {
            var node = nodesDB.QueryNode<EnemyNode>(targetID);

            node.animationComponent.animation.SetTrigger("Dead");

            TaskRunner.Instance.CreateEmptyTask().Start(Sink(node), true);
        }

        IEnumerator Sink(EnemyNode node)
        {
            DateTime AfterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < AfterTwoSec)
            {
                node.transformComponent.transform.Translate(-Vector3.up * node.movementComponent.sinkSpeed * Time.deltaTime);

                yield return null;
            }

            node.removeEntityComponent.removeEntity();
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };
    }
}
