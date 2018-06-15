using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class ScoreEngine : IQueryingEntityViewEngine, IStep<DamageInfo, DamageCondition>
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {}
        
        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            int hudEntityViewsCount;
            var hudEntityViews = entityViewsDB.QueryEntities<HUDEntityView>(out hudEntityViewsCount);

            if (hudEntityViewsCount > 0)
            {
                uint index;
                PlayerTargetTypeEntityStruct playerTarget =
                entityViewsDB.QueryEntitiesAndIndex<PlayerTargetTypeEntityStruct>(token.entityDamagedID, out index)[index];
                
                switch (playerTarget.targetType)
                {
                    case PlayerTargetType.Bunny:
                        hudEntityViews[0].scoreComponent.score += 10;
                        break;
                    case PlayerTargetType.Bear:
                        hudEntityViews[0].scoreComponent.score += 20;
                        break;
                    case PlayerTargetType.Hellephant:
                        hudEntityViews[0].scoreComponent.score += 30;
                        break;
                }
            }
        }

        ISequencer _damageSequence;
    }
}


