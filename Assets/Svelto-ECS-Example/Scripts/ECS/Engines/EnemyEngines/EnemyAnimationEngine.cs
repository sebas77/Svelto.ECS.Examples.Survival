using Svelto.ECS.Example.Survive.HUD;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntitiesEngine
                                      , IStep<PlayerDeathCondition>
                                      , IStep<EnemyDeathCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {}

        public void Step(EnemyDeathCondition condition, EGID id)
        {
            TriggerDeathAnimation(id);
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            //is player is dead, the enemy cheers
            TriggerTargetDeathAnimation();
        }

        void Damaged(EGID id)
        {
            entitiesDB.ExecuteOnEntity(id,
                                       (ref EnemyEntityViewStruct entity) =>
                                       {
                                   //        entity.vfxComponent.position   = damage.damagePoint;
                                           entity.vfxComponent.play.value = true;
                                       });
        }

        void TriggerTargetDeathAnimation()
        {
            int count;
            var entity = entitiesDB.QueryEntities<EnemyEntityViewStruct>(out count);

            for (var i = 0; i < count; i++)
                entity[i].animationComponent.playAnimation = "PlayerDead";
        }

        void TriggerDeathAnimation(EGID targetID)
        {
            entitiesDB.ExecuteOnEntity(targetID,
                                       (ref EnemyEntityViewStruct entity) =>
                                           entity.animationComponent.playAnimation = "Dead");
        }
    }
}