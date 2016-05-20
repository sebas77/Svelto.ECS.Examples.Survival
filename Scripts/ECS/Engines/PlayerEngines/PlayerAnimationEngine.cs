using System;
using Svelto.ES;
using Svelto.Ticker;
using UnitySampleAssets.CrossPlatformInput;
using Nodes.Player;
using Components.Damageable;

namespace Engines.Player
{
    public class PlayerAnimationEngine : INodesEngine, IPhysicallyTickable
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            _playerNode = obj as PlayerNode;

            _playerNode.healthComponent.isDead.subscribers += TriggerDeathAnimation;
        }

        public void Remove(INode obj)
        {
            if (_playerNode.healthComponent != null)
                _playerNode.healthComponent.isDead.subscribers -= TriggerDeathAnimation;

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

        void TriggerDeathAnimation(int sender, int targetID)
        {
            _playerNode.animationComponent.animation.SetTrigger("Die");
        }

        readonly Type[] _acceptedNodes = { typeof(PlayerNode) };

        PlayerNode _playerNode;
    }
}
