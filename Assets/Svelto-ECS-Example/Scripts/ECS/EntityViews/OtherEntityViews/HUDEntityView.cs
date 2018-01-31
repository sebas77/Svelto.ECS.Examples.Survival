using Svelto.ECS.Example.Survive.Components.HUD;
using Svelto.ECS.Example.Survive.Components.Shared;

namespace Svelto.ECS.Example.Survive.EntityViews.HUD
{
    public class HUDEntityView : EntityView
    {
        public IAnimationComponent       HUDAnimator;
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
    }
}
