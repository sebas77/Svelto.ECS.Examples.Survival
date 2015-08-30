using UnityEngine;
using UnityEngine.UI;

namespace GUIComponents
{
    public interface IAnimatorHUDComponent
    {
        Animator hudAnimator { get; }
    }

    public interface IDamageHUDComponent
    {
        Image damageImage { get; }
        float flashSpeed { get; }
        Color flashColor { get; }
    }

    public interface IHealthSliderComponent
    {
        Slider healthSlider { get; }
    }

    public interface IScoreComponent
    {
        int score { set; get; }
    }
}
