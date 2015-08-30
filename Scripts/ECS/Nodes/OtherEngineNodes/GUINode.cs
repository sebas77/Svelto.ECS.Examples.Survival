using GUIComponents;
using Svelto.ES;

namespace GUIEngines
{
    public class GUINode: INode
    {
        public IAnimatorHUDComponent    HUDAnimator; 
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
    }
}
