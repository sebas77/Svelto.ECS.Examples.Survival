using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Components.HUD
{
    public interface IAnimatorHUDComponent: IComponent
    {
        Animator hudAnimator { get; }
    }

    public interface IDamageHUDComponent: IComponent
    {
        Image damageImage { get; }
        float flashSpeed { get; }
        Color flashColor { get; }
    }

    public interface IHealthSliderComponent: IComponent
    {
        Slider healthSlider { get; }
    }

    public interface IScoreComponent: IComponent
    {
        int score { set; get; }
    }
}
