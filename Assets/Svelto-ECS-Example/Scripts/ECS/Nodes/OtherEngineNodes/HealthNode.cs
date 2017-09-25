using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Nodes.DamageableEntities
{
    public class HealthNode: NodeWithID
    {
        public IHealthComponent         healthComponent;

        public IRemoveEntityComponent   removeEntityComponent;
    }
}
