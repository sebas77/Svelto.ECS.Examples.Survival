using System;
using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.ECS.Example.Components.Damageable;

namespace Svelto.ECS.Example.Engines.Enemies
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

                healthEventsComponent.isDead.NotifyOnDataChange(TriggerDeathAnimation);
                healthEventsComponent.isDamaged.subscribers += EntityDamaged;
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.NotifyOnDataChange(OnTargetDead);
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                var healthEventsComponent = (obj as EnemyNode).healthComponent;

                if (healthEventsComponent != null)
                {
                    healthEventsComponent.isDead.StopNotifyOnDataChange(TriggerDeathAnimation);
                    healthEventsComponent.isDamaged.subscribers -= EntityDamaged;
                }
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.StopNotifyOnDataChange(OnTargetDead);
        }

        void EntityDamaged(int sender, DamageInfo damageInfo)
        {
            var node = nodesDB.QueryNode<EnemyNode>(sender);

            node.vfxComponent.hitParticles.transform.position = damageInfo.damagePoint;
            node.vfxComponent.hitParticles.Play();
        }

        void OnTargetDead(int targetID, bool isDead)
        {
            var nodes = nodesDB.QueryNodes<EnemyNode>();

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                node.animationComponent.animation.SetTrigger("PlayerDead");
            }
        }

        void TriggerDeathAnimation(int targetID, bool isDead)
        {
            var node = nodesDB.QueryNode<EnemyNode>(targetID);

            node.animationComponent.animation.SetTrigger("Dead");

            TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Sink(node.transformComponent.transform, node.movementComponent.sinkSpeed)).Start();
        }

        IEnumerator Sink(Transform transform, float sinkSpeed)
        {
            DateTime AfterTwoSec = DateTime.UtcNow.AddSeconds(2);

            while (DateTime.UtcNow < AfterTwoSec)
            {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);

                yield return null;
            }

            UnityEngine.Object.Destroy(transform.gameObject);
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };
    }
}
