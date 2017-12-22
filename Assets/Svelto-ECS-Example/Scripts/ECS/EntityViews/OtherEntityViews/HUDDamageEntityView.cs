using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.EntityViews.HUD
{
	public class HUDDamageEntityView: EntityView<HUDDamageEntityView>
    {
        public IHealthComponent        healthComponent;
    }
}
