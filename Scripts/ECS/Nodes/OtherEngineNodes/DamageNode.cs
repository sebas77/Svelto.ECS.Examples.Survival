using Components.Damageable;
using Svelto.ES;

namespace Nodes.DamageableEntities
{
    public class DamageNode: NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
    }
}
