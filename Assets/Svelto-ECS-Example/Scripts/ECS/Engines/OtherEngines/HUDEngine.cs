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
            AnimateUI().Run();
            CheckForDamage().Run();
        }

        IEnumerator AnimateUI()
        {
            while (true)
            {
                entitiesDB.ExecuteOnEntities(ref _time, (ref HUDEntityView guiEntityView, ref ITime time, int index) =>
                      {
                          var damageComponent = guiEntityView.damageImageComponent;

                          damageComponent.imageColor =
                              Color.Lerp(damageComponent.imageColor, Color.clear,
                                         damageComponent.speed * time.deltaTime);
                      });
                
                
                yield return null;
            }
        }

        /// <summary>
        /// the damaged flag is polled. I am still torn about the
        /// poll vs push problem, so more investigation is needed 
        /// </summary>
        /// <param name="entitiesDb"></param>
        
        IEnumerator CheckForDamage()
        {
            while (true)
            {
                int numberOfPlayers;
                var players = entitiesDB.QueryEntities<PlayerEntityStruct>(out numberOfPlayers);
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    uint index;

                    if (entitiesDB.QueryEntitiesAndIndex<DamageableEntityStruct>(players[i].ID, out index)[index].damaged == false) continue;

                    var health =
                        entitiesDB.QueryEntitiesAndIndex<HealthEntityStruct>
                            (players[i].ID, out index)[index].currentHealth;

                    entitiesDB.ExecuteOnEntities(ref health,
                                                 (ref HUDEntityView guiEntityView,
                                                  ref int refhealth, int innerIndex) =>
                                                 {
                                                     var damageComponent = guiEntityView.damageImageComponent;
                                                     damageComponent.imageColor = damageComponent.flashColor;

                                                     guiEntityView.healthSliderComponent.value = refhealth;
                                                 });
                }

                yield return null;
            }
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
            int hudEntityViewsCount;
            var hudEntityViews = entitiesDB.QueryEntities<HUDEntityView>(out hudEntityViewsCount);
            for (int i = 0; i < hudEntityViewsCount; i++)
                hudEntityViews[i].healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }

        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        ITime                     _time;
    }
}

