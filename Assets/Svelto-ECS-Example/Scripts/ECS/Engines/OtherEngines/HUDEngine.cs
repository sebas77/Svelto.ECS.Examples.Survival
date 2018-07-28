using System;
using Svelto.Tasks.Enumerators;
using System.Collections;
using Svelto.ECS.Example.Survive.Characters;
using Svelto.ECS.Example.Survive.Characters.Player;
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
    public class HUDEngine : IQueryingEntitiesEngine, IStep<PlayerDeathCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

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
                entitiesDB.ExecuteOnEntities(ref _time, (ref HUDEntityView guiEntityView, ref ITime time) =>
                      {
                          var damageComponent = guiEntityView.damageImageComponent;

                          damageComponent.imageColor =
                              Color.Lerp(damageComponent.imageColor, Color.clear,
                                         damageComponent.speed * time.deltaTime);
                      });
                
                var value = entitiesDB;
                FlashOnDamage(value);
                
                yield return null;
            }
        }

        //static so it helps to not capture this in the lambdas
        static void FlashOnDamage(IEntitiesDB entitiesDb)
        {
            int numberOfPlayers;
            var players = entitiesDb.QueryEntities<DamageablePlayerEntityStruct>(out numberOfPlayers);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                uint index;

                if (players[i].damaged == false) return;

                var health =
                    entitiesDb.QueryEntitiesAndIndex<HealthEntityStruct>
                        (players[i].ID, out index)[index].currentHealth;

                entitiesDb.ExecuteOnEntities(ref health,
                                              (ref HUDEntityView guiEntityView,
                                               ref int           refhealth) =>
                                              {
                                                  var damageComponent = guiEntityView.damageImageComponent;
                                                  damageComponent.imageColor = damageComponent.flashColor;

                                                  guiEntityView.healthSliderComponent.value = refhealth;
                                              });
            }
        }
        
        //static so it helps to not capture this in the lambdas
        void FlashOnDamageBinding(EGID id)
        {
            uint index;
            var health =
                    entitiesDB.QueryEntitiesAndIndex<HealthEntityStruct>
                        (id, out index)[index].currentHealth;

                entitiesDB.ExecuteOnEntities(ref health,
                                             (ref HUDEntityView guiEntityView,
                                              ref int           refhealth) =>
                                             {
                                                 var damageComponent = guiEntityView.damageImageComponent;
                                                 damageComponent.imageColor = damageComponent.flashColor;

                                                 guiEntityView.healthSliderComponent.value = refhealth;
                                             });
        }

        void OnPlayerDeadEvent()
        {
            int hudEntityViewsCount;
            var hudEntityViews = entitiesDB.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
                hudEntityViews[i].healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }

        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            int hudEntityViewsCount;
            var hudEntityViews = entitiesDB.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
                hudEntityViews[i].HUDAnimator.playAnimation = "GameOver";

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            OnPlayerDeadEvent();
        }

        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        ITime                     _time;
    }

    class DataBindsAttribute : Attribute
    {
        public DataBindsAttribute(Type healthEntityStruct)
        {
            throw new NotImplementedException();
        }
    }
}

