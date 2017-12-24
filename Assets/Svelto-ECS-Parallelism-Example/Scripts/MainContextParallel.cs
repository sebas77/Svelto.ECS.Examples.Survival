//#define DONT_TRY_THIS_AT_HOME

using Svelto.Context;
using UnityEngine;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
namespace Svelto.ECS.Example.Parallelism
{
    public class NumberOfEntities
    {
        public const int value = 64 * 4000;
    }
    public class ParallelCompositionRoot : ICompositionRoot
    {
        public ParallelCompositionRoot()
        {
            QualitySettings.vSyncCount = -1;

            _contextNotifier = new ContextNotifier();
        }

        void ICompositionRoot.OnContextCreated(UnityContext contextHolder)
        {
            var tasksCount = NumberOfEntities.value;
#if DONT_TRY_THIS_AT_HOME
            for (int i = 0; i < tasksCount; i++)
            {
                GameObject crazyness = new GameObject();
                crazyness.AddComponent<UnityWay>();
            }
#else
            _enginesRoot = new EnginesRoot(new Schedulers.UnitySumbmissionEntityViewScheduler());
            IEntityFactory entityFactory = _enginesRoot.GenerateEntityFactory();

            var boidsEngine = new BoidsEngine();
            _enginesRoot.AddEngine(boidsEngine);

            _contextNotifier.AddFrameworkDestructionListener(boidsEngine);

            var implementorArray = new object[1];

            for (int i = 0; i < tasksCount; i++)
            {
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
                implementorArray[0] = new Boid();
                entityFactory.BuildEntity<BoidEntityDescriptor>(i, implementorArray);

#else
                entityFactory.BuildEntity<BoidEntityDescriptor>(i);
#endif
            }

            entityFactory.BuildEntity<GUITextEntityDescriptor>(0, 
                contextHolder.GetComponentsInChildren<PrintIteration>());
#endif
        }
        
        void ICompositionRoot.OnContextInitialized()
        {}

        void ICompositionRoot.OnContextDestroyed()
        {
            _contextNotifier.NotifyFrameworkDeinitialized();       
            TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks();
        }

        IContextNotifer _contextNotifier;
        EnginesRoot _enginesRoot;
    }

    class GUITextEntityDescriptor:GenericEntityDescriptor<PrintTimeEntityView>
    {}

    //A GameObject containing UnityContext must be present in the scene
    //All the monobehaviours present in the scene statically that need
    //to notify the Context, must belong to GameObjects children of UnityContext.
    public class MainContextParallel : UnityContext<ParallelCompositionRoot>
    { }
}

