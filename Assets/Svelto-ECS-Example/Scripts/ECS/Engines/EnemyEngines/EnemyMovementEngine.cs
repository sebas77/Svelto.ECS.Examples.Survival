using System.Collections;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyMovementEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                int count;
                //query all the enemies from the standard group (no disabled nor respawning)
                var enemyTargetEntityViews = entitiesDB.QueryEntities<EnemyTargetEntityViewStruct>(out count);

                if (count > 0)
                {
                    var targetEntityView = enemyTargetEntityViews[0];

                    var enemies = entitiesDB.QueryEntities<EnemyEntityViewStruct>(out count);

                    for (var i = 0; i < count; i++)
                    {
                        if ( enemies[i].movementComponent.navMeshEnabled == false)
                            Utility.Console.Log("why "+enemies[i].ID.entityID);           
                        {
                            enemies[i].movementComponent.navMeshDestination =
                                targetEntityView.targetPositionComponent.position;
                        }
                    }
                }

                yield return null;
            }
        }
    }
}
