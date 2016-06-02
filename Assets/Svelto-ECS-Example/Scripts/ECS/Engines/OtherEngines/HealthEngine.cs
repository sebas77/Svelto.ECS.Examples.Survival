using Components.Damageable;
using Nodes.DamageableEntities;
using Svelto.ES;

namespace Engines.Health
{
    public class HealthEngine : SingleNodeEngine<HealthNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        override protected void Add(HealthNode node)
        {
            var healthComponent = node.damageEventComponent;

            healthComponent.damageReceived.subscribers += TriggerDamage;
        }

        override protected void Remove(HealthNode node)
        {
            var healthComponent = node.damageEventComponent;

            healthComponent.damageReceived.subscribers -= TriggerDamage;
        }

        void TriggerDamage(int ID, DamageInfo damage)
        {
            var node = nodesDB.QueryNode<HealthNode>(ID);
            var healthComponent = node.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
                healthComponent.isDead.Dispatch();
            else
                healthComponent.isDamaged.Dispatch(ref damage);
        }
    }
}
