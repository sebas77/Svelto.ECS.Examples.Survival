using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class AnimatorHUDImplementor: MonoBehaviour, IImplementor, IAnimationComponent
    {
        Animator        animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void setBool(string name, bool value)
        {
            animator.SetBool(name, value);
        }

        public string playAnimation { set { animator.SetTrigger(value);} }
    }
}