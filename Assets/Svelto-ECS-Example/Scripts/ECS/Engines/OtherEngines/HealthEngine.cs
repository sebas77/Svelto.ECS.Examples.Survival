using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.DamageableEntities;
using System;

namespace Svelto.ECS.Example.Survive.Engines.Health
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<PlayerDamageInfo>
    {
        public void Ready()
        { }

        public HealthEngine(IEntityFunctions entityFunctions, Sequencer playerDamageSequence)
        {
            _damageSequence = playerDamageSequence;
            _entityfunctions = entityFunctions;
        }

        public IEngineEntityViewDB entityViewsDB { set; private get; }

        public void Step(ref PlayerDamageInfo token, Enum condition)
        {
            TriggerDamage(ref token);
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            TriggerDamage(ref token);
        }

        void TriggerDamage<T>(ref T damage) where T:IDamageInfo
        {
            var EntityView = entityViewsDB.QueryEntityView<HealthEntityView>(damage.entityDamaged);
            var healthComponent = EntityView.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
            {
                var entityTemplate = EntityView.removeEntityComponent.entityDescriptor;
                _entityfunctions.RemoveEntity(damage.entityDamaged, entityTemplate);

                _damageSequence.Next(this, ref damage, DamageCondition.dead);
                
            }
            else
                _damageSequence.Next(this, ref damage, DamageCondition.damage);
        }

        Sequencer _damageSequence;
        IEntityFunctions _entityfunctions;
    }
}
