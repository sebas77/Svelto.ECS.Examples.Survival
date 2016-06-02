using Components.Damageable;
using Svelto.ES;

namespace Nodes.DamageableEntities
{
    public class HealthNode: NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
    }
}
