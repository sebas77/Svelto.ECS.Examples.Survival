using Svelto.ECS.Example.Survive.EntityViews.Player;
using UnityStandardAssets.CrossPlatformInput;
using System;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    public class PlayerAnimationEngine : SingleEntityViewEngine<PlayerEntityView>, IStep<TargetDamageInfo>
    {
        public PlayerAnimationEngine()
        {
            TaskRunner.Instance.RunOnSchedule(Tasks.StandardSchedulers.physicScheduler, new Tasks.TimedLoopActionEnumerator(PhysicsTick));
        }

        protected override void Add(PlayerEntityView playerEntityView)
        {
            _playerEntityView = playerEntityView;
        }

        protected override void Remove(PlayerEntityView playerEntityView)
        {
            _playerEntityView = null;
        }

        public void PhysicsTick(float deltaSec)
        {
            if (_playerEntityView == null) return;
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            _playerEntityView.animationComponent.animation.SetBool("IsWalking", walking);
        }

        void TriggerDeathAnimation(int targetID)
        {
            _playerEntityView.animationComponent.animation.SetTrigger("Die");
        }

        public void Step(ref TargetDamageInfo token, Enum condition)
        {
            TriggerDeathAnimation(token.entityDamaged);
        }

        PlayerEntityView _playerEntityView;
    }
}
