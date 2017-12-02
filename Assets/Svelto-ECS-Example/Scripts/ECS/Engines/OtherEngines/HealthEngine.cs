using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Nodes.DamageableEntities;
using System;

namespace Svelto.ECS.Example.Survive.Engines.Health
{
    public class HealthEngine : IEngine, IQueryableNodeEngine, IStep<DamageInfo>, IStep<PlayerDamageInfo>
    {
        Sequencer _damageSequence;

        public HealthEngine(Sequencer playerDamageSequence)
        {
            _damageSequence = playerDamageSequence;
        }

        public IEngineNodeDB nodesDB { set; private get; }

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
            var node = nodesDB.QueryNode<HealthNode>(damage.entityDamaged);
            var healthComponent = node.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
            {
                _damageSequence.Next(this, ref damage, DamageCondition.dead);

                node.removeEntityComponent.removeEntity();
            }
            else
                _damageSequence.Next(this, ref damage, DamageCondition.damage);
        }
    }
}
