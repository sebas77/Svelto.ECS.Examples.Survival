using System;
using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<TargetDamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void EntityDamaged(DamageInfo damageInfo)
        {
            var EntityView = entityViewsDB.QueryEntityView<EnemyEntityView>(damageInfo.entityDamaged);

            EntityView.vfxComponent.hitParticles.transform.position = damageInfo.damagePoint;
            EntityView.vfxComponent.hitParticles.Play();
        }

        void TriggerTargetDeathAnimation(int targetID)
        {
            var EntityViews = entityViewsDB.QueryEntityViews<EnemyEntityView>();

            for (int i = 0; i < EntityViews.Count; i++)
                EntityViews[i].animationComponent.animation.SetTrigger("PlayerDead");
        }

        void TriggerDeathAnimation(int targetID)
        {
            var EntityView = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);
            EntityView.animationComponent.animation.SetTrigger("Dead");

            TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Sink(EntityView.transformComponent.transform, EntityView.movementComponent.sinkSpeed)).Start();
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

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.dead)
                TriggerDeathAnimation(token.entityDamaged);
            else
                EntityDamaged(token);
        }

        public void Step(ref TargetDamageInfo token, int condition)
        {
            if (condition == DamageCondition.dead)
                TriggerTargetDeathAnimation(token.entityDamaged);
        }
    }
}
