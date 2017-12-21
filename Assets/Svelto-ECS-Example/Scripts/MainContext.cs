using Svelto.ECS.Example.Survive.Engines.Enemies;
using Svelto.ECS.Example.Survive.Engines.Health;
using Svelto.ECS.Example.Survive.Engines.HUD;
using Svelto.ECS.Example.Survive.Engines.Player;
using Svelto.ECS.Example.Survive.Engines.Player.Gun;
using Svelto.ECS.Example.Survive.Engines.Sound.Damage;
using Svelto.ECS.Example.Survive.Observables.Enemies;
using Svelto.ECS.Example.Survive.Observers.HUD;
using Svelto.Context;
using UnityEngine;
using Steps = System.Collections.Generic.Dictionary<Svelto.ECS.IEngine, System.Collections.Generic.Dictionary<System.Enum, Svelto.ECS.IStep[]>>;
using System.Collections.Generic;
using Svelto.ECS.Schedulers;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
namespace Svelto.ECS.Example.Survive
{
    public class Main : ICompositionRoot
    {
        public Main()
        {
            SetupEnginesAndEntities();
        }

        void SetupEnginesAndEntities()
        {
            //The Engines Root is the core of Svelto.ECS. You must NEVER inject the EngineRoot
            //as it is, however you may inject it as IEntityFactory. In fact, you can build entity
            //inside other engines or factories as well.
            //the UnitySumbmissionEntityViewScheduler is the scheduler that is used by the Root to know
            //when to inject the EntityViews. You shouldn't use a custom one unless you know what you 
            //are doing, so let's assume it's part of the pattern right now.
            _enginesRoot = new EnginesRoot(new UnitySumbmissionEntityViewScheduler());
            _entityFactory = _enginesRoot.GenerateEntityFactory();
            var entityFunctions = _enginesRoot.GenerateEntityFunctions();

            GameObjectFactory factory = new GameObjectFactory();

            var enemyKilledObservable = new EnemyKilledObservable();
            var scoreOnEnemyKilledObserver = new ScoreOnEnemyKilledObserver(enemyKilledObservable);

            Sequencer playerDamageSequence = new Sequencer();
            Sequencer enemyDamageSequence = new Sequencer();

            //Player related engines
            var playerHealthEngine = new HealthEngine(entityFunctions, playerDamageSequence);
            var playerShootingEngine = new PlayerGunShootingEngine(enemyKilledObservable, enemyDamageSequence);
            var playerMovementEngine = new PlayerMovementEngine();
            var playerAnimationEngine = new PlayerAnimationEngine();

            //Enemy related engines
            var enemyAnimationEngine = new EnemyAnimationEngine();
            var enemyHealthEngine = new HealthEngine(entityFunctions, enemyDamageSequence);
            var enemyAttackEngine = new EnemyAttackEngine(playerDamageSequence);
            var enemyMovementEngine = new EnemyMovementEngine();

            //hud and sound engines
            var hudEngine = new HUDEngine();
            var damageSoundEngine = new DamageSoundEngine();
            EnemySpawnerEngine enemySpawnerEngine = new EnemySpawnerEngine(factory, _entityFactory);

            playerDamageSequence.SetSequence(
                new Steps() //sequence of steps
                { 
                    { //first step
                        enemyAttackEngine, //this step can be triggered only by this engine through the Next function
                        new Dictionary<System.Enum, IStep[]>() //this step can lead only to one branch
                        { 
                            {  Condition.always, new [] { playerHealthEngine }  }, //these engines will be called when the Next function is called with the Condition.always set
                        }  
                    },
                    { //second step
                        playerHealthEngine, //this step can be triggered only by this engine through the Next function
                        new Dictionary<System.Enum, IStep[]>() //this step can branch in two paths
                        { 
                            {  DamageCondition.damage, new IStep[] { hudEngine, damageSoundEngine }  }, //these engines will be called when the Next function is called with the DamageCondition.damage set
                            {  DamageCondition.dead, new IStep[] { hudEngine, damageSoundEngine, playerMovementEngine, playerAnimationEngine, enemyAnimationEngine }  }, //these engines will be called when the Next function is called with the DamageCondition.dead set
                        }  
                    }  
                }
            );

            enemyDamageSequence.SetSequence(
                new Steps()
                { 
                    { 
                        playerShootingEngine, 
                        new Dictionary<System.Enum, IStep[]>()
                        { 
                            {  Condition.always, new [] { enemyHealthEngine }  },
                        }  
                    },
                    { 
                        enemyHealthEngine, 
                        new Dictionary<System.Enum, IStep[]>()
                        { 
                            {  DamageCondition.damage, new IStep[] { enemyAnimationEngine }  },
                            {  DamageCondition.dead, new IStep[] { enemyMovementEngine, enemyAnimationEngine, playerShootingEngine, enemySpawnerEngine }  },
                        }  
                    }  
                }
            );

            AddEngine(playerMovementEngine);
            AddEngine(playerAnimationEngine);
            AddEngine(playerShootingEngine);
            AddEngine(playerHealthEngine);
            AddEngine(new PlayerGunShootingFXsEngine());

            AddEngine(enemySpawnerEngine);
            AddEngine(enemyAttackEngine);
            AddEngine(enemyMovementEngine);
            AddEngine(enemyAnimationEngine);
            AddEngine(enemyHealthEngine);

            AddEngine(damageSoundEngine);
            AddEngine(hudEngine);
            AddEngine(new ScoreEngine(scoreOnEnemyKilledObserver));
        }

        void AddEngine(IEngine engine)
        {
            _enginesRoot.AddEngine(engine);
        }

        void ICompositionRoot.OnContextCreated(UnityContext contextHolder)
        {
            IEntityDescriptorHolder[] entities = contextHolder.GetComponentsInChildren<IEntityDescriptorHolder>();

            for (int i = 0; i < entities.Length; i++)
            {
                var entityDescriptorHolder = entities[i];
                var entityDescriptor = entityDescriptorHolder.RetrieveDescriptor();
                _entityFactory.BuildEntity
                    (((MonoBehaviour) entityDescriptorHolder).gameObject.GetInstanceID(), 
                    entityDescriptor,
                    (entityDescriptorHolder as MonoBehaviour).GetComponentsInChildren<IImplementor>());
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

    public class MainContext : UnityContext<Main>
    { }

}