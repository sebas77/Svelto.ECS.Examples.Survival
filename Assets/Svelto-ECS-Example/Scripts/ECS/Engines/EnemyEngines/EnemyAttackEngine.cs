using System.Collections;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.ECS.Example.Survive.Others;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAttackEngine : MultiEntityViewsEngine<EnemyEntityView, EnemyTargetEntityView>, IQueryingEntityViewEngine
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

        protected override void Add(EnemyEntityView entity)
        {
            entity.targetTriggerComponent.entityInRange += CheckTarget;
        }

        protected override void Remove(EnemyEntityView entity)
        {
            entity.targetTriggerComponent.entityInRange -= CheckTarget;
        }

        protected override void Add(EnemyTargetEntityView entity)
        {
            _targetEntityView = entity;
            _taskRoutine.Start();
        }

        protected override void Remove(EnemyTargetEntityView obj)
        {
            _taskRoutine.Stop();
            _targetEntityView = null;
        }

        IEnumerator CheckIfHittingEnemyTarget()
        {
            while (true)
            {
                var enemiesAttackList = entityViewsDB.QueryEntityViews<EnemyEntityView>();

                for (int enemyIndex = enemiesAttackList.Count - 1; enemyIndex >= 0; --enemyIndex)
                {
                    var enemyAttackEntityView = enemiesAttackList[enemyIndex];
                    if (enemyAttackEntityView.attackComponent.targetInRange == true)
                    {
                        var attackDamageComponent = enemyAttackEntityView.attackDamageComponent;
                        attackDamageComponent.timer += _time.deltaTime;

                        if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                        {
                            attackDamageComponent.timer = 0.0f;

                            var damageInfo = new DamageInfo(attackDamageComponent.damage, Vector3.zero,
                                _targetEntityView.ID, EntityDamagedType.EnemyTarget);

                            _targetDamageSequence.Next(this, ref damageInfo);
                        }
                    }
                }

                yield return null;
            }
        }

        /// <summary>
        /// Logic for when the Unity OnTrigger is enable
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="enemyID"></param>
        /// <param name="inRange"></param>
        void CheckTarget(int targetID, int enemyID, bool inRange)
        {
            if (_targetEntityView == null)
                return;

            if (targetID == _targetEntityView.ID)
            {
                var enemyEntityView = entityViewsDB.QueryEntityView<EnemyEntityView>(enemyID);
                var component = enemyEntityView.targetTriggerComponent;

                if (inRange)
                    component.targetInRange = true;
                else
                    component.targetInRange = false;
            }
        }

        EnemyTargetEntityView _targetEntityView;
        ISequencer             _targetDamageSequence;
        ITime                 _time;
        ITaskRoutine          _taskRoutine;
    }
}
