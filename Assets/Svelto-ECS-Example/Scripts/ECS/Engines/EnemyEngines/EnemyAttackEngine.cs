using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyAttackEngine : MultiEntityViewsEngine<EnemyEntityView, EnemyTargetEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            TaskRunner.Instance.Run(new TimedLoopActionEnumerator(Tick));
        }

        public EnemyAttackEngine(Sequencer enemyrDamageSequence)
        {
            _targetDamageSequence = enemyrDamageSequence;
        }

        protected override void Add(EnemyEntityView obj)
        {
            EnemyEntityView enemyEntityView = (obj as EnemyEntityView);

            enemyEntityView.targetTriggerComponent.entityInRange += CheckTarget;
        }

        protected override void Remove(EnemyEntityView obj)
        {
            EnemyEntityView enemyEntityView = (obj as EnemyEntityView);

            enemyEntityView.targetTriggerComponent.entityInRange -= CheckTarget;
        }

        protected override void Add(EnemyTargetEntityView obj)
        {
            _targetEntityView = obj as EnemyTargetEntityView;
        }

        protected override void Remove(EnemyTargetEntityView obj)
        {
            _targetEntityView = null;
        }

        void Tick(float deltaSec)
        {
            if (_targetEntityView == null) return;

            var enemiesAttackList = entityViewsDB.QueryEntityViews<EnemyEntityView>();

            for (int enemyIndex = enemiesAttackList.Count - 1; enemyIndex >= 0 ; --enemyIndex)
            {
                var enemyAttackEntityView = enemiesAttackList[enemyIndex];

                if (enemyAttackEntityView.attackComponent.targetInRange == true)
                {
                    var attackDamageComponent = enemyAttackEntityView.attackDamageComponent;
					    attackDamageComponent.timer += deltaSec;

                    if (attackDamageComponent.timer >= attackDamageComponent.attackInterval)
                    {
                        attackDamageComponent.timer = 0.0f;

                        var damageInfo = new TargetDamageInfo(attackDamageComponent.damage, Vector3.zero, _targetEntityView.ID);

                        _targetDamageSequence.Next(this, ref damageInfo);
                    }
                }
            }
        }

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
        Sequencer _targetDamageSequence;
    }
}
