using SharedComponents;
using Svelto.ES;
using Svelto.Ticker;
using System;
using UnityEngine;

namespace GUIEngines
{
    public class HudEngine : INodeEngine, ITickable
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is GUINode)
                _guiNode = obj as GUINode;
            else
            {
                var damageEventNode = obj as GUIDamageEventNode; 

                damageEventNode.healthComponent.isDamaged.observers += OnDamageEvent;
                damageEventNode.healthComponent.isDead.observers += OnDeadEvent;
            }
        }

        public void Remove(INode obj)
        {
            if (obj is GUINode)
                _guiNode = null;
            else
            {
                var damageEventNode = obj as GUIDamageEventNode; 

                damageEventNode.healthComponent.isDamaged.observers -= OnDamageEvent;
                damageEventNode.healthComponent.isDead.observers -= OnDeadEvent;
            }
        }

        private void OnDamageEvent(IHealthComponent sender, DamageInfo damaged)
        {
            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = damageComponent.flashColor;

            _guiNode.healthSliderComponent.healthSlider.value = sender.currentHealth;
        }

        private void OnDeadEvent(IHealthComponent arg1, GameObject arg2)
        {
            _guiNode.HUDAnimator.hudAnimator.SetTrigger("GameOver");
        }

        public void Tick(float deltaSec)
        {
            var damageComponent = _guiNode.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageComponent.flashSpeed * deltaSec);
        }

        Type[] _acceptedNodes = new Type[2] { typeof(GUINode), typeof(GUIDamageEventNode) };

        GUINode             _guiNode;
    }
}

