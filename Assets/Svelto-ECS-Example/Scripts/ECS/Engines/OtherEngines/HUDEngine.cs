using Svelto.Tasks.Enumerators;
using System.Collections;
using Svelto.ECS.Example.Survive.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svelto.ECS.Example.Survive.HUD
{
    /// <summary>
    /// 
    /// You may wonder why I use QueryEntityViews instead to hold a reference
    /// of the only HudEntityView existing in the game or just using QueryEntityView.
    /// This is for learning purposes. An engine shouldn't really have the concept
    /// of how many entities are created. Using the QueryEntityView could be awkward
    /// because the entity ID is needed.
    /// Therefore using the Add/Remove callbacks is not wrong, but I try to not
    /// promote their use. 
    /// </summary>
    public class HUDEngine : IQueryingEntityViewEngine, IStep<DamageInfo, DamageCondition>
    {
        public IEntityDB EntityDb { set; private get; }

        public HUDEngine(ITime time)
        {
            _time = time;
        }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                int hudEntityViewsCount;
                var hudEntityViews = EntityDb.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
                for (int i = 0; i < hudEntityViewsCount; i++)
                {
                    var damageComponent = hudEntityViews[i].damageImageComponent;

                    damageComponent.imageColor = Color.Lerp(damageComponent.imageColor, Color.clear,
                        damageComponent.speed * _time.deltaTime);
                }
                
                yield return null;
            }
        }

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnDeadEvent()
        {
            int hudEntityViewsCount;
            var hudEntityViews = EntityDb.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
                hudEntityViews[i].healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }

        void UpdateSlider(DamageInfo damaged)
        {
            int hudEntityViewsCount;
            var hudEntityViews = EntityDb.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
            {
                var guiEntityView = hudEntityViews[i];
                var damageComponent = guiEntityView.damageImageComponent;

                damageComponent.imageColor = damageComponent.flashColor;

                uint index;
                guiEntityView.healthSliderComponent.value = EntityDb.QueryEntitiesAndIndex<HealthEntityStruct>(damaged.entityDamagedID, out index)[index].currentHealth;
            }
        }

        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            int hudEntityViewsCount;
            var hudEntityViews = EntityDb.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
                hudEntityViews[i].HUDAnimator.playAnimation = "GameOver";

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            if (condition == DamageCondition.Damage)
                OnDamageEvent(token);
            else
            if (condition == DamageCondition.Dead)
                OnDeadEvent();
        }

        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime                     _time;
    }
}

