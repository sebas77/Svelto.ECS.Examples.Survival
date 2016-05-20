using Components.Damageable;
using Nodes.HUD;
using Svelto.ES;
using Svelto.Ticker;
using System;
using UnityEngine;

namespace Engines.HUD
{
    public class HUDEngine : INodesEngine, ITickable, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is HUDNode)
                _guiNode = obj as HUDNode;
            else
            {
                var damageEventNode = obj as HUDDamageEventNode;

				damageEventNode.healthComponent.isDamaged.subscribers += OnDamageEvent;
                damageEventNode.healthComponent.isDead.subscribers += OnDeadEvent;
            }
        }

        public void Remove(INode obj)
        {
            if (obj is HUDNode)
                _guiNode = null;
            else
            {
                var damageEventNode = obj as HUDDamageEventNode;

				damageEventNode.healthComponent.isDamaged.subscribers -= OnDamageEvent;
				damageEventNode.healthComponent.isDead.subscribers -= OnDeadEvent;
            }
        }

        private void OnDamageEvent(int sender, DamageInfo damaged)
        {
            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = damageComponent.flashColor;

            _guiNode.healthSliderComponent.healthSlider.value = nodesDB.QueryNode<HUDDamageEventNode>(sender).healthComponent.currentHealth;
        }

        private void OnDeadEvent(int healthSender, int targetID)
        {
            _guiNode.HUDAnimator.hudAnimator.SetTrigger("GameOver");
        }

        public void Tick(float deltaSec)
        {
            if (_guiNode == null) return;

            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageComponent.flashSpeed * deltaSec);
        }

        readonly Type[] _acceptedNodes = { typeof(HUDNode), typeof(HUDDamageEventNode) };

        HUDNode             _guiNode;
    }
}

