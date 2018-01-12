using Svelto.ECS.Example.Survive.Components.HUD;

namespace Svelto.ECS.Example.Survive.EntityViews.HUD
{
    public class HUDEntityView : EntityView
    {
        public IAnimatorHUDComponent    HUDAnimator;
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
    }
}
