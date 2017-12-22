using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.EntityViews.DamageableEntities
{
    public class HealthEntityView: EntityView<HealthEntityView>
    {
        public IHealthComponent healthComponent;
        public IRemoveEntityComponent removeEntityComponent;
    }
}
