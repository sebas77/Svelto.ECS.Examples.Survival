using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.EntityViews.DamageableEntities
{
    public class HealthEntityView: EntityView
    {
        public IHealthComponent healthComponent;
        public IRemoveEntityComponent removeEntityComponent;
    }
}
