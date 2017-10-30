//#define DONT_TRY_THIS_AT_HOME

using Svelto.Context;
using Svelto.ECS.NodeSchedulers;
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
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
            var boidDescriptor = new BoidEntityDescriptor(new[]{ new Boid() });
#else
            var boidDescriptor = new BoidEntityDescriptor();
#endif
#if DONT_TRY_THIS_AT_HOME
            for (int i = 0; i < tasksCount; i++)
            {
                GameObject crazyness = new GameObject();
                crazyness.AddComponent<UnityWay>();
            }
#else
            IEnginesRoot enginesRoot;
            IEntityFactory entityFactory = (enginesRoot = new EnginesRoot(new UnitySumbmissionNodeScheduler())) as IEntityFactory;

            var boidsEngine = new BoidsEngine();
            enginesRoot.AddEngine(boidsEngine);
            _contextNotifier.AddFrameworkDestructionListener(boidsEngine);

            for (int i = 0; i < tasksCount; i++)
                entityFactory.BuildEntity(i, boidDescriptor);

            entityFactory.BuildEntity(0, new GenericEntityDescriptor<PrintTimeNode>(contextHolder.GetComponentInChildren<PrintIteration>()));
#endif
        }

        void ICompositionRoot.OnContextInitialized()
        {
        }

        void ICompositionRoot.OnContextDestroyed()
        {
            _contextNotifier.NotifyFrameworkDeinitialized();       
            TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks();
        }

        IContextNotifer _contextNotifier;
    }

    //A GameObject containing UnityContext must be present in the scene
    //All the monobehaviours present in the scene statically that need
    //to notify the Context, must belong to GameObjects children of UnityContext.
    public class MainContextParallel : UnityContext<ParallelCompositionRoot>
    { }
}

