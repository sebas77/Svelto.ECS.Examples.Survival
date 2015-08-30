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

        //this is an important example in the process of IoC code design.
        //I had to options here, write an observer which send an event when the 
        //damage image had to flash, or let the engine decide to flash when
        //the player is damaged.
        //if I chose to create an observer to communicate when the hud has to flash
        //it would have meant for the other engines to know the concept of flashing image
        //there is difference semantically between writing
        //observer.Dispatch(FlashImage);
        //and
        //observer.Dispatch(PlayerIsDamaged)
        //if the image flashes for other reasons, the external entites
        //would take control over the behaviour of the flashing image
        //dectacting when it had to flash
        //instead different entities must throw the event that makes sense
        //in their context and the hudengine will decide what to do with those
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

