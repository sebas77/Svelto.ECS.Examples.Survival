using Svelto.ECS.Example.Survive.Components.HUD;

namespace Svelto.ECS.Example.Survive.Nodes.HUD
{
    public class HUDNode: NodeWithID
    {
        public IAnimatorHUDComponent    HUDAnimator;
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
    }
}
