using Components.Damageable;
using Nodes.DamageableEntities;
using Svelto.ES;

namespace Engines.Health
{
    public class HealthEngine : SingleNodeEngine<DamageNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        override protected void Add(DamageNode node)
        {
            var healthComponent = node.damageEventComponent;

            healthComponent.damageReceived.subscribers += TriggerDamage;
        }

        override protected void Remove(DamageNode node)
        {
            var healthComponent = node.damageEventComponent;

           healthComponent.damageReceived.subscribers -= TriggerDamage;
        }

        private void TriggerDamage(int ID, DamageInfo damage)
        {
            var node = nodesDB.QueryNode<DamageNode>(ID);

            var healthComponent = node.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            if (healthComponent.currentHealth <= 0)
                Death(node);
            else
                healthComponent.isDamaged.Dispatch(ref damage);
        }

        void Death(DamageNode node)
        {
            var healthComponent = node.healthComponent;
            var ID = node.ID;

            healthComponent.isDead.Dispatch(ref ID);

            Remove(node);
        }
    }
}
