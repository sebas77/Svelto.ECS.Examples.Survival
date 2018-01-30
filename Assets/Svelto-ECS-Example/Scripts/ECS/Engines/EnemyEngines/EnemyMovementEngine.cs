using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using System.Collections;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Engines.Enemies
{
    public class EnemyMovementEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var enemyTargetEntityViews = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>();

                if (enemyTargetEntityViews.Count > 0)
                {
                    var targetEntityView = enemyTargetEntityViews[0];

                    if (targetEntityView == null) yield break;

                    var enemies = entityViewsDB.QueryEntityViews<EnemyEntityView>();

                    for (var i = 0; i < enemies.Count; i++)
                    {
                        var component = enemies[i].movementComponent;

                        if (component.isNavMeshActiveAndEnabled)
                            component.navMeshDestination = targetEntityView.targetPositionComponent.position;
                    }
                }

                yield return null;
            }
        }

        void StopEnemyOnDeath(int targetID)
        {
            EnemyEntityView entityView = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);

            entityView.movementComponent.navMeshEnabled = false;
            entityView.movementComponent.setCapsuleAsTrigger = true;
            entityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            StopEnemyOnDeath(token.entityDamaged);
        }
    }
}
