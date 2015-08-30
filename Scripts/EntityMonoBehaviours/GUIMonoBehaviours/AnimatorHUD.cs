using GUIComponents;
using UnityEngine;

namespace CompleteProject
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
