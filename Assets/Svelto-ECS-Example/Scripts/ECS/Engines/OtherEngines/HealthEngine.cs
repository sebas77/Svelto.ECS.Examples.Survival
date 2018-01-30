using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Health
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public void Ready()
        { }

        public HealthEngine(IEntityFunctions entityFunctions, ISequencer playerDamageSequence)
        {
            _damageSequence = playerDamageSequence;
            _entityfunctions = entityFunctions;
        }

        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Step(ref DamageInfo token, int condition)
        {
            TriggerDamage(ref token);
        }

        void TriggerDamage(ref DamageInfo damage)
        {
            var entityView = entityViewsDB.QueryEntityView<HealthEntityView>(damage.entityDamaged);
            var healthComponent = entityView.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
            {
                _damageSequence.Next(this, ref damage, DamageCondition.dead);

                RemoveEntityAtTheEndOfTheFrame(damage, entityView).Run();
            }
            else
                _damageSequence.Next(this, ref damage, DamageCondition.damage);
        }

        IEnumerator RemoveEntityAtTheEndOfTheFrame(DamageInfo damage, HealthEntityView entityView)
        {
            yield return _endOfFrame;

            _entityfunctions.RemoveEntity(damage.entityDamaged, entityView.removeEntityComponent);
        }

        ISequencer         _damageSequence;
        IEntityFunctions  _entityfunctions;
        WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    }
}
