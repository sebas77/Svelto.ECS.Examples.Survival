using SharedComponents;
using Svelto.ES;
using System;
using System.Collections.Generic;

namespace SharedEngines
{
    public class HealthEngine : INodeEngine
    {
        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj)
        {
            var node = (obj as DamageNode);
            var healthComponent = (obj as DamageNode).damageEventComponent;

            healthComponent.damageReceived.observers += TriggerDamage;

            _enemyEventComponents[healthComponent] = node;
        }

        public void Remove(INode obj)
        {
           var healthComponent = (obj as DamageNode).damageEventComponent;

           if (healthComponent != null)
               healthComponent.damageReceived.observers -= TriggerDamage;

           _enemyEventComponents.Remove(healthComponent);
        }

        private void TriggerDamage(IDamageEventComponent component, DamageInfo damage)
        {
            var node = _enemyEventComponents[component];
            var healthComponent = node.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
                Death(node);
            else
                healthComponent.isDamaged.Dispatch(damage); //it must be responsability of the engine to decide if the entity has actually been damaged
        }

        void Death(DamageNode node)
        {
            var healthComponent = node.healthComponent;

            healthComponent.isDead.Dispatch(node.ID);

            Remove(node);
        }

        Type[] _acceptedNodes = new Type[1] { typeof(DamageNode) };

        Dictionary<IDamageEventComponent, DamageNode>     _enemyEventComponents = new Dictionary<IDamageEventComponent, DamageNode>();
    }
}
