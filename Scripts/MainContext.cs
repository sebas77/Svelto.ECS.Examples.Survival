using EnemyEngines;
using EnemyObservables;
using GUIEngines;
using PlayerEngines;
using ScoreObservers;
using SharedEngines;
using Soundengines;
using Svelto.Context;
using Svelto.ES;
using Svelto.Ticker;
using UnityEngine;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
namespace CompleteProject
{
    public class Main : ICompositionRoot, IUnityContextHierarchyChangedListener
    {
        public Main()
        {
            SetupEnginesAndComponents();
        }

        void SetupEnginesAndComponents()
        {
            _enginesRoot = new UnityEnginesRoot();
            _tickEngine = new UnityTicker();

            var enemyKilledObservable = new EnemyKilledObservable();
            var scoreOnEnemyKilledObserver = new ScoreOnEnemyKilledObserver(enemyKilledObservable);

            GameObjectFactory factory = new GameObjectFactory(this);

            AddEngine(new PlayerMovementEngine());
            AddEngine(new PlayerAnimationEngine());
            AddEngine(new PlayerShootingEngine(enemyKilledObservable));
            AddEngine(new PlayerShootingFXsEngine());

            AddEngine(new EnemySpawnerEngine(factory));
            AddEngine(new EnemyAttackEngine());
            AddEngine(new EnemyMovementEngine());
            AddEngine(new EnemyAnimationEngine());
            
            AddEngine(new DamageSoundEngine());
            AddEngine(new HealthEngine());
            AddEngine(new HudEngine());
            AddEngine(new ScoreEngine(scoreOnEnemyKilledObserver));
        }

        void AddEngine(IEngine engine)
        {
            if (engine is ITickableBase)
                _tickEngine.Add(engine as ITickableBase);

            _enginesRoot.AddEngine(engine);
        }

        public void OnContextInitialized()
        {}

        public void OnContextDestroyed()
        {}

        public void OnMonobehaviourAdded(MonoBehaviour component)
        {}

        public void OnMonobehaviourRemoved(MonoBehaviour component)
        {}

        public void OnGameObjectAdded(GameObject entity)
        {
            _enginesRoot.AddGameObjectEntity(entity);
        }

        public void OnGameObjectRemoved(GameObject entity)
        {
            _enginesRoot.RemoveGameObjectEntity(entity);
        }

        UnityEnginesRoot    _enginesRoot;
        UnityTicker         _tickEngine;
    }

    //A GameObject containing UnityContext must be present in the scene
    //All the monobehaviours present in the scene statically that need 
    //to notify the Context, must belong to GameObjects children of UnityContext.

    public class MainContext : UnityContext<Main>
    {
    }
}
