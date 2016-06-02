using Engines.Enemies;
using Engines.Health;
using Engines.HUD;
using Engines.Player;
using Engines.Player.Gun;
using Engines.Sound.Damage;
using Observables.Enemies;
using Observers.HUD;
using Svelto.Context;
using Svelto.ES;
using Svelto.Ticker;
using UnityEngine;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
public class Main : ICompositionRoot
    {
        public Main()
        {
            SetupEnginesAndComponents();
        }

        void SetupEnginesAndComponents()
        {
			_tickEngine = new UnityTicker();
            _entityFactory = _enginesRoot = new EnginesRoot(_tickEngine);
            
			GameObjectFactory factory = new GameObjectFactory();

            var enemyKilledObservable = new EnemyKilledObservable();
            var scoreOnEnemyKilledObserver = new ScoreOnEnemyKilledObserver((EnemyKilledObservable)enemyKilledObservable);

			AddEngine(new PlayerMovementEngine());
            AddEngine(new PlayerAnimationEngine());

            AddEngine(new PlayerGunShootingEngine(enemyKilledObservable));
            AddEngine(new PlayerGunShootingFXsEngine());

            AddEngine(new EnemySpawnerEngine(factory, _entityFactory));
            AddEngine(new EnemyAttackEngine());
            AddEngine(new EnemyMovementEngine());
            AddEngine(new EnemyAnimationEngine());

            AddEngine(new HealthEngine());
            AddEngine(new DamageSoundEngine());

            AddEngine(new HUDEngine());
            AddEngine(new ScoreEngine(scoreOnEnemyKilledObserver));
        }

        void AddEngine(IEngine engine)
        {
            if (engine is ITickableBase)
                _tickEngine.Add(engine as ITickableBase);

            _enginesRoot.AddEngine(engine);
        }

        void ICompositionRoot.OnContextCreated(UnityContext contextHolder)
        {
			IEntityDescriptorHolder[] entities = contextHolder.GetComponentsInChildren<IEntityDescriptorHolder>();

			for (int i = 0; i < entities.Length; i++)
				_entityFactory.BuildEntity((entities[i] as MonoBehaviour).gameObject.GetInstanceID(), entities[i].BuildDescriptorType());
        }

        void ICompositionRoot.OnContextInitialized()
        {}

        void ICompositionRoot.OnContextDestroyed()
        {}

		EnginesRoot     _enginesRoot;
		IEntityFactory  _entityFactory;
        UnityTicker 	_tickEngine;
    }

    //A GameObject containing UnityContext must be present in the scene
    //All the monobehaviours present in the scene statically that need
    //to notify the Context, must belong to GameObjects children of UnityContext.

    public class MainContext : UnityContext<Main>
    {}

