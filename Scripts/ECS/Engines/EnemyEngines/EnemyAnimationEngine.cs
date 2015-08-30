using System;
using Svelto.ES;
using SharedComponents;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EnemyEngines
{
    public class EnemyAnimationEngine : INodeEngine
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

                healthEventsComponent.isDead.observers += TriggerDeathAnimation;
                healthEventsComponent.isDamaged.observers += EntityDamaged;

                _enemyNodes[healthEventsComponent] = enemyNode;
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.observers += OnTargetDead;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                var healthEventsComponent = (obj as EnemyNode).healthComponent;

                if (healthEventsComponent != null)
                {
                    healthEventsComponent.isDead.observers -= TriggerDeathAnimation;
                    healthEventsComponent.isDamaged.observers -= EntityDamaged;
                }

                _enemyNodes.Remove(healthEventsComponent);
            }
            else
                (obj as EnemyTargetNode).healthComponent.isDead.observers -= OnTargetDead;
        }

        void EntityDamaged(IHealthComponent sender, DamageInfo damageInfo)
        {
            var node = _enemyNodes[sender];

            node.vfxComponent.hitParticles.transform.position = damageInfo.damagePoint;

            node.vfxComponent.hitParticles.Play();
        }

        void OnTargetDead(IHealthComponent arg1, GameObject arg2)
        {
            var values = _enemyNodes.Values.GetEnumerator();

            while (values.MoveNext())
            {
                var node = values.Current;

                node.animationComponent.animation.SetTrigger("PlayerDead");
            }
        }

         void TriggerDeathAnimation(IHealthComponent sender, GameObject target)
        {
            var node = _enemyNodes[sender];

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

            GameObject.Destroy(node.ID);
        }

        Type[] _acceptedNodes = new Type[2] { typeof(EnemyNode), typeof(EnemyTargetNode) };

        Dictionary<IHealthComponent, EnemyNode> _enemyNodes = new Dictionary<IHealthComponent, EnemyNode>();
    }
}
