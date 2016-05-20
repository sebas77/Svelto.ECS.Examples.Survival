using Components.HUD;
using UnityEngine;

namespace Implementators.HUD
{
    public class AnimatorHUD: MonoBehaviour, IAnimatorHUDComponent
    {
        Animator animator;
        Animator IAnimatorHUDComponent.hudAnimator { get { return animator; } }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}
