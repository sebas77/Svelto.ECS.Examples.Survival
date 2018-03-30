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

        protected override void Remove(EnemyTargetEntityView obj)
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
                var targetEntitiesView = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>();
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
                var enemiesAttackList = entityViewsDB.QueryEntityViews<EnemyEntityView>();

                for (int enemyIndex = enemiesAttackList.Count - 1; enemyIndex >= 0; --enemyIndex)
                {
                    var enemyAttackEntityView = enemiesAttackList[enemyIndex];
                    var enemyCollisionData    = enemyAttackEntityView.targetTriggerComponent.entityInRange;
                    
                    for (int enemyTargetIndex = 0; enemyTargetIndex < targetEntitiesView.Count; enemyTargetIndex++)
                    {
                        var targetEntityView = targetEntitiesView[enemyTargetIndex];
                        
                        //the IEnemyTriggerComponent implementors sets a the collides boolean
                        //whenever anything enters in the trigger range, but there is not more logic
                        //we have to check here if the colliding entity is actually an EnemyTarget
                        
                        if (enemyCollisionData.collides      == true &&
                            enemyCollisionData.otherEntityID.IsEqualTo(targetEntityView.ID))
                        {
                            var attackDamageComponent   = enemyAttackEntityView.attackDamageComponent;
                            attackDamageComponent.timer += _time.deltaTime;

                            if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                            {
                                attackDamageComponent.timer = 0.0f;

                                var damageInfo = new DamageInfo(attackDamageComponent.damage, Vector3.zero,
                                                                targetEntityView.ID, EntityDamagedType.Player);

                                _targetDamageSequence.Next(this, ref damageInfo);
                            }
                        }
                    }
                }

                yield return null;
            }
        }

        ISequencer            _targetDamageSequence;
        ITime                 _time;
        ITaskRoutine          _taskRoutine;
    }
}
