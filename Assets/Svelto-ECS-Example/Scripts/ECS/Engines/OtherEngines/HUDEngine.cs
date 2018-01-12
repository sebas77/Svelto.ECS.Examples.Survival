using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.HUD;
using Svelto.Tasks.Enumerators;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svelto.ECS.Example.Survive.Engines.HUD
{
    public class HUDEngine : SingleEntityViewEngine<HUDEntityView>, IQueryingEntityViewEngine, IStep<TargetDamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
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

        void OnDamageEvent(ref TargetDamageInfo damaged)
        {
            var damageComponent = _guiEntityView.damageImageComponent;
            var damageImage = damageComponent.damageImage;

            damageImage.color = damageComponent.flashColor;

            _guiEntityView.healthSliderComponent.healthSlider.value = entityViewsDB.QueryEntityView<HUDDamageEntityView>(damaged.entityDamaged).healthComponent.currentHealth;
        }

        void OnDeadEvent()
        {
            RestartLevelAfterFewSeconds().Run();
        }

        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            _guiEntityView.HUDAnimator.hudAnimator.SetTrigger("GameOver");

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Step(ref TargetDamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.damage)
                OnDamageEvent(ref token);
            else
            if ((DamageCondition)condition == DamageCondition.dead)
                OnDeadEvent();
        }

        HUDEntityView            _guiEntityView;
        WaitForSecondsEnumerator _waitForSeconds = new WaitForSecondsEnumerator(5);
    }
}

