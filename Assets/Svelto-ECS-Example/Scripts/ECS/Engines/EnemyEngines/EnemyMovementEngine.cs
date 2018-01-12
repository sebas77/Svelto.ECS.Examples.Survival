using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyMovementEngine : SingleEntityViewEngine<EnemyTargetEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        public EnemyMovementEngine()
        {
            TaskRunner.Instance.Run(new Tasks.TimedLoopActionEnumerator(Tick));
        }

        protected override void Add(EnemyTargetEntityView EntityView)
        {
            _targetEntityView = EntityView;
        }

        protected override void Remove(EnemyTargetEntityView EntityView)
        {
            _targetEntityView = null;
        }

        void Tick(float deltaSec)
        {
            if (_targetEntityView == null)
                return;
            
            var enemies = entityViewsDB.QueryEntityViews<EnemyEntityView>();

            for (var i = 0; i < enemies.Count; i++)
            {
                var component = enemies[i].movementComponent;

                if (component.navMesh.isActiveAndEnabled)
                    component.navMesh.SetDestination(_targetEntityView.targetPositionComponent.position);
            }
        }

        void StopEnemyOnDeath(int targetID)
        {
            EnemyEntityView EntityView = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);

            EntityView.movementComponent.navMesh.enabled = false;
            var capsuleCollider = EntityView.movementComponent.capsuleCollider;
            capsuleCollider.isTrigger = true;
            capsuleCollider.GetComponent<Rigidbody>().isKinematic = true;
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            StopEnemyOnDeath(token.entityDamaged);
        }

        EnemyTargetEntityView   _targetEntityView;
    }
}
