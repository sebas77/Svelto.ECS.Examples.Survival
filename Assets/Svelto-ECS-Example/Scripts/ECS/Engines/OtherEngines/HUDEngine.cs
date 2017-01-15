using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Nodes.HUD;
using Svelto.Ticker.Legacy;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Engines.HUD
{
    public class HUDEngine : INodesEngine, ITickable, IQueryableNodeEngine, IStep<PlayerDamageInfo>
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
        }

        public void Remove(INode obj)
        {
            if (obj is HUDNode)
                _guiNode = null;
        }

        public void Tick(float deltaSec)
        {
            if (_guiNode == null) return;

            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageComponent.flashSpeed * deltaSec);
        }

        void OnDamageEvent(ref PlayerDamageInfo damaged)
        {
            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = damageComponent.flashColor;

            _guiNode.healthSliderComponent.healthSlider.value = nodesDB.QueryNode<HUDDamageEventNode>(damaged.entityDamaged).healthComponent.currentHealth;
        }

        void OnDeadEvent()
        {
            _guiNode.HUDAnimator.hudAnimator.SetTrigger("GameOver");
        }

        public void Step(ref PlayerDamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.damage)
                OnDamageEvent(ref token);
            else
            if ((DamageCondition)condition == DamageCondition.dead)
                OnDeadEvent();
                
        }

        readonly Type[] _acceptedNodes = { typeof(HUDNode) };

        HUDNode         _guiNode;
    }
}

