using Components.HUD;
using Svelto.ES;

namespace Nodes.HUD
{
    public class HUDNode: NodeWithID
    {
        public IAnimatorHUDComponent    HUDAnimator;
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
    }
}
