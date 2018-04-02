using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyAttackEngine : SingleEntityViewEngine<EnemyTargetEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        public EnemyAttackEngine(ISequencer enemyrDamageSequence, ITime time)
        {
            _targetDamageSequence = enemyrDamageSequence;
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(CheckIfHittingEnemyTarget()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(EnemyTargetEntityView entity)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(EnemyTargetEntityView entity)
        {
            _taskRoutine.Stop();
        }

        IEnumerator CheckIfHittingEnemyTarget()
        {
            while (true)
            {
                // Pay attention to this bit. The engine is querying a
                // EnemyTargetEntityView and not a PlayerEntityView.
                // this is more than a sophistication, it actually the implementation
                // of the rule that every engine must use its own set of
                // EntityViews to promote encapsulation and modularity
                var targetEntitieViews = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>();
                while (targetEntitieViews.Count == 0)
                {
                    yield return null;
                    targetEntitieViews = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>();
                }

                //there is a sneaky bug that can be caused by this routine. It can be solved in several
                //ways once it has been understood.
                //the targetDamageSequence.Next can trigger a sequence that could lead to the immediate
                //death of the player, this would mean that the inner loop should stop when the 
                //enemytarget (the player) dies. However this engine doens't know when the player dies
                //We can solve this problem in several ways including
                //- iterating over the enemy target, if the entity has been removed because dead, the for will be skipped
                //(which is the solution I chose here)
                //- removing the entity the frame after and not immediatly (a bit hacky)
                //- add this engine in the sequencer to know when the player is death to stop
                //this taskroutine
                var enemies = entityViewsDB.QueryEntityViews<EnemyAttackEntityView>();
                while (enemies.Count == 0)
                {
                    yield return null;
                    enemies = entityViewsDB.QueryEntityViews<EnemyAttackEntityView>();
                }

                int count;
                var enemiesAttackData = entityViewsDB.QueryEntityViewsAsArray<EnemyAttackStruct>(out count);
                
                //this is more complex than needed code is just to show how you can use entity structs
                //this case is banal, entity structs should be use to handle hundreds or thousands
                //of entities in a cache friendly and multi threaded code. However entity structs would allow
                //the creation of entity without any allocation, so they can be handy for
                //cases where entity should be built fast! Theoretically is possible to create
                //a game using only entity structs, but entity structs make sense ONLY if they
                //hold value types, so they come with a lot of limitations

                for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; --enemyIndex)
                {
                    var enemyAttackEntityView = enemies[enemyIndex];
                    
                    enemiesAttackData[enemyIndex].entityInRange = enemyAttackEntityView.targetTriggerComponent.entityInRange;
                }

                for (int enemyTargetIndex = 0; enemyTargetIndex < targetEntitieViews.Count; enemyTargetIndex++)
                {
                    var targetEntityView = targetEntitieViews[enemyTargetIndex];

                    for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; --enemyIndex)
                    {
                        if (enemiesAttackData[enemyIndex].entityInRange.collides == true)
                        {
                            //the IEnemyTriggerComponent implementors sets a the collides boolean
                            //whenever anything enters in the trigger range, but there is not more logic
                            //we have to check here if the colliding entity is actually an EnemyTarget
                            if (enemiesAttackData[enemyIndex].entityInRange.otherEntityID.IsEqualTo(targetEntityView.ID))
                            {
                                enemiesAttackData[enemyIndex].timer += _time.deltaTime;
                                
                                if (enemiesAttackData[enemyIndex].timer >= enemiesAttackData[enemyIndex].timeBetweenAttack)
                                {
                                    enemiesAttackData[enemyIndex].timer = 0.0f;

                                    var damageInfo = new DamageInfo(enemiesAttackData[enemyIndex].attackDamage, Vector3.zero,
                                                                    targetEntityView.ID, EntityDamagedType.Player);

                                    _targetDamageSequence.Next(this, ref damageInfo);
                                }
                            }
                        }
                    }
                }

                yield return null;
            }
        }


        readonly ISequencer            _targetDamageSequence;
        readonly ITime                 _time;
        readonly ITaskRoutine          _taskRoutine;
    }
}
