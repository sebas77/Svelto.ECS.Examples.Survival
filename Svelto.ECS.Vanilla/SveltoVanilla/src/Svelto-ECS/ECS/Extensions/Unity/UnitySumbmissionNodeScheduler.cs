#if UNITY_5 || UNITY_5_3_OR_NEWER
using System;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.EntityViewSchedulers
{
    //The EntityViewSubmissionScheduler has been introduced to make
    //the entityView submission logic platform indipendent.
    //Please don't be tempted to create your own submission to 
    //adapt to your game level code design. For example,
    //you may be tempted to write a submission logic to submit
    //the entityViews immediatly just because convenient for your game
    //logic. This is not how it works.
    
    public class UnitySumbmissionEntityViewScheduler : EntityViewSubmissionScheduler
    {
        public UnitySumbmissionEntityViewScheduler()
        {
            GameObject go = new GameObject("ECSScheduler");

            _scheduler = go.AddComponent<Scheduler>();
        }

        public override void Schedule(Action submitEntityViews)
        {
            _scheduler.OnTick += submitEntityViews;
        }

        class Scheduler : MonoBehaviour
        {
            IEnumerator Start()
            {
                while (true)
                {
                    yield return _wait;

                    OnTick();
                }
            }

            internal Action OnTick;
            WaitForEndOfFrame _wait = new WaitForEndOfFrame();
        }

        Scheduler _scheduler;
    }
}
#endif