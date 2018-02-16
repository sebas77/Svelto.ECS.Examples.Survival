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
            var targetEntityView = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>()[0];
            
            while (true)
            {
                var enemiesAttackList = entityViewsDB.QueryEntityViews<EnemyEntityView>();

                for (int enemyIndex = enemiesAttackList.Count - 1; enemyIndex >= 0; --enemyIndex)
                {
                    var enemyAttackEntityView = enemiesAttackList[enemyIndex];
                    var enemyCollisionData = enemyAttackEntityView.targetTriggerComponent.entityInRange;
                    if (enemyCollisionData.collides == true &&
                        enemyCollisionData.otherEntityID == targetEntityView.ID)
                    {
                        var attackDamageComponent = enemyAttackEntityView.attackDamageComponent;
                        attackDamageComponent.timer += _time.deltaTime;

                        if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                        {
                            attackDamageComponent.timer = 0.0f;

                            var damageInfo = new DamageInfo(attackDamageComponent.damage, Vector3.zero,
                                targetEntityView.ID, EntityDamagedType.EnemyTarget);

                            _targetDamageSequence.Next(this, ref damageInfo);
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
