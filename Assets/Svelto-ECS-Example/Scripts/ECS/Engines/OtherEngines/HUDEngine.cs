using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.HUD;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.HUD
{
    public class HUDEngine : SingleEntityViewEngine<HUDEntityView>, IQueryingEntityViewEngine, IStep<PlayerDamageInfo>
    {
        public IEngineEntityViewDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        public HUDEngine()
        {
            TaskRunner.Instance.Run(new Tasks.TimedLoopActionEnumerator(Tick));
        }

        protected override void Add(HUDEntityView EntityView)
        {
            _guiEntityView = EntityView;
        }

        protected override void Remove(HUDEntityView EntityView)
        {
            _guiEntityView = null;
        }

        void Tick(float deltaSec)
        {
            if (_guiEntityView == null) return;

            var damageComponent = _guiEntityView.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageComponent.flashSpeed * deltaSec);
        }

        void OnDamageEvent(ref PlayerDamageInfo damaged)
        {
            var damageComponent = _guiEntityView.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = damageComponent.flashColor;

            _guiEntityView.healthSliderComponent.healthSlider.value = entityViewsDB.QueryEntityView<HUDDamageEntityView>(damaged.entityDamaged).healthComponent.currentHealth;
        }

        void OnDeadEvent()
        {
            _guiEntityView.HUDAnimator.hudAnimator.SetTrigger("GameOver");
        }

        public void Step(ref PlayerDamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.damage)
                OnDamageEvent(ref token);
            else
            if ((DamageCondition)condition == DamageCondition.dead)
                OnDeadEvent();
                
        }

        HUDEntityView         _guiEntityView;
    }
}

