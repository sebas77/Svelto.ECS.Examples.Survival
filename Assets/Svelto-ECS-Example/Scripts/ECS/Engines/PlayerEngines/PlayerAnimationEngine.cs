using Svelto.Ticker.Legacy;
using Svelto.ECS.Example.Nodes.Player;
using UnityStandardAssets.CrossPlatformInput;

namespace Svelto.ECS.Example.Engines.Player
{
    public class PlayerAnimationEngine : SingleNodeEngine<PlayerNode>, IPhysicallyTickable
    {
        protected override void Add(PlayerNode playerNode)
        {
            _playerNode = playerNode;

            _playerNode.healthComponent.isDead.NotifyOnDataChange(TriggerDeathAnimation);
        }

        protected override void Remove(PlayerNode playerNode)
        {
            _playerNode = null;
        }

        public void PhysicsTick(float deltaSec)
        {
            if (_playerNode == null) return;
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            _playerNode.animationComponent.animation.SetBool("IsWalking", walking);
        }

        void TriggerDeathAnimation(int targetID, bool isDead)
        {
            _playerNode.animationComponent.animation.SetTrigger("Die");
        }

        PlayerNode _playerNode;
    }
}
