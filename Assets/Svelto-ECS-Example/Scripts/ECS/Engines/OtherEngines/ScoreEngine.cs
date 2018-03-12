using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class ScoreEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {}
        
        public void Step(ref DamageInfo token, int condition)
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();

            if (hudEntityViews.Count > 0)
            {
                var guiEntityView = hudEntityViews[0];
                
                var playerTarget = entityViewsDB.QueryEntityView<PlayerTargetEntityView>(token.entityDamagedID);
                var targetType   = playerTarget.playerTargetComponent.targetType;
                
                switch (targetType)
                {
                    case PlayerTargetType.Bunny:
                        guiEntityView.scoreComponent.score += 10;
                        break;
                    case PlayerTargetType.Bear:
                        guiEntityView.scoreComponent.score += 20;
                        break;
                    case PlayerTargetType.Hellephant:
                        guiEntityView.scoreComponent.score += 30;
                        break;
                }
            }
        }

        ISequencer _damageSequence;
    }
}


