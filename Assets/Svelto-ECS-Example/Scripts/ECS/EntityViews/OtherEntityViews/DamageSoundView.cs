using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.EntityViews.Sound
{
    public class DamageSoundEntityView: EntityView
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
    }
}
