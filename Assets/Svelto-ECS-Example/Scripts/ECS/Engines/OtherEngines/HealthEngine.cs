using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;
using System;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Health
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<TargetDamageInfo>
    {
        public void Ready()
        { }

        public HealthEngine(IEntityFunctions entityFunctions, Sequencer playerDamageSequence)
        {
            _damageSequence = playerDamageSequence;
            _entityfunctions = entityFunctions;
        }

        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Step(ref TargetDamageInfo token, Enum condition)
        {
            TriggerDamage(ref token);
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            TriggerDamage(ref token);
        }

        void TriggerDamage<T>(ref T damage) where T:IDamageInfo
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

        private IEnumerator RemoveEntityAtTheEndOfTheFrame<T>(T damage, HealthEntityView entityView) where T : IDamageInfo
        {
            yield return _endOfFrame;

            _entityfunctions.RemoveEntity(damage.entityDamaged, entityView.removeEntityComponent);
        }

        Sequencer        _damageSequence;
        IEntityFunctions _entityfunctions;
        WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    }
}
