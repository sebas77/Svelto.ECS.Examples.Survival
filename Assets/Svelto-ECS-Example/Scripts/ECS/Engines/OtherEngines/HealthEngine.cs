using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Nodes.DamageableEntities;

namespace Svelto.ECS.Example.Engines.Health
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
            {
                healthComponent.isDead.value = true;
                node.removeEntityComponent.removeEntity();
            }
            else
                healthComponent.isDamaged.Dispatch(ref damage);
        }
    }
}
