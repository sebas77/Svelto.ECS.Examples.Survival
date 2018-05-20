using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
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
                var enemyTargetEntityViews = entityViewsDB.QueryEntities<EnemyTargetEntityView>();

                if (enemyTargetEntityViews.Count > 0)
                {
                    var targetEntityView = enemyTargetEntityViews[0];

                    var enemies = entityViewsDB.QueryEntities<EnemyEntityView>();

                    for (var i = 0; i < enemies.Count; i++)
                    {
                        var component = enemies[i].movementComponent;

                        component.navMeshDestination = targetEntityView.targetPositionComponent.position;
                    }
                }

                yield return null;
            }
        }

        void StopEnemyOnDeath(EGID targetID)
        {
            EnemyEntityView entityView;
            entityViewsDB.TryQueryEntityView(targetID, out entityView);
            
            entityView.movementComponent.navMeshEnabled = false;
            entityView.movementComponent.setCapsuleAsTrigger = true;
            entityView.rigidBodyComponent.isKinematic = true;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            StopEnemyOnDeath(token.entityDamagedID);
        }
    }
}
