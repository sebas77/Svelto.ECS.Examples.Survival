using Svelto.ECS.Example.Survive.Components.Shared;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class AnimatorHUDImplementor: MonoBehaviour, IImplementor, IAnimationComponent
    {
        Animator animator;
        public Animator hudAnimator { get { return animator; } }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void setBool(string name, bool value)
        {
            throw new System.NotImplementedException();
        }

        public string trigger { get; set; }
    }
}
