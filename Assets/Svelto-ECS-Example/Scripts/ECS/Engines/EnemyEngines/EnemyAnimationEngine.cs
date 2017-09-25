using System;
using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAnimationEngine : IEngine, IQueryableNodeEngine, IStep<DamageInfo>, IStep<PlayerDamageInfo>
    {
        public IEngineNodeDB nodesDB { set; private get; }

        void EntityDamaged(DamageInfo damageInfo)
        {
            var node = nodesDB.QueryNode<EnemyNode>(damageInfo.entityDamaged);

            node.vfxComponent.hitParticles.transform.position = damageInfo.damagePoint;
            node.vfxComponent.hitParticles.Play();
        }

        void OnTargetDead(int targetID)
        {
            var nodes = nodesDB.QueryNodes<EnemyNode>();

            for (int i = 0; i < nodes.Count; i++)
                nodes[i].animationComponent.animation.SetTrigger("PlayerDead");
        }

        void TriggerDeathAnimation(int targetID)
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

        public void Step(ref DamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.dead)
                TriggerDeathAnimation(token.entityDamaged);
            else
                EntityDamaged(token);
        }

        public void Step(ref PlayerDamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.dead)
                OnTargetDead(token.entityDamaged);
        }
    }
}
