using Svelto.ECS.Example.Components.Damageable;

namespace Svelto.ECS.Example.Nodes.DamageableEntities
{
    public class HealthNode: NodeWithID
    {
        public IHealthComponent         healthComponent;

        public IRemoveEntityComponent   removeEntityComponent;
    }
}
