using Svelto.Context;
using Svelto.ECS.Example.Flock.Engines;
using Svelto.ECS.NodeSchedulers;
using UnityEngine;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
namespace Svelto.ECS.Example.Flock
{
    public class CompositionRoot : ICompositionRoot
    {
        public CompositionRoot()
        {
            QualitySettings.vSyncCount = -1;

            SetupEnginesAndComponents();
        }

        void SetupEnginesAndComponents()
        {
            _entityFactory = _enginesRoot = new EnginesRoot(new UnitySumbmissionNodeScheduler());

            var boidEngine = new BoidsEngine();

            _enginesRoot.AddEngine(boidEngine);
        }

        void ICompositionRoot.OnContextCreated(UnityContext contextHolder)
        {
            var main = contextHolder.GetComponentInChildren<Main>();

            main.Init();
            main.Run(_entityFactory);

            var boxes = contextHolder.GetComponentsInChildren<BoxCollider>();
            foreach (var box in boxes)
            {
         //       _entityFactory.BuildEntity(0, new ObstacleDescriptor());
            }
        }

        void ICompositionRoot.OnContextInitialized()
        { }

        void ICompositionRoot.OnContextDestroyed()
        { }

        
        EnginesRoot _enginesRoot;
        IEntityFactory _entityFactory;
    }

    //A GameObject containing UnityContext must be present in the scene
    //All the monobehaviours present in the scene statically that need
    //to notify the Context, must belong to GameObjects children of UnityContext.
    public class MainContextFlock : UnityContext<Svelto.ECS.Example.Flock.CompositionRoot>
    { }
}

