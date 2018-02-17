namespace Svelto.ECS.Example.Survive.HUD
{
    public class ScoreEngine : SingleEntityViewEngine<HUDEntityView>
    {
        public ScoreEngine(ScoreOnEnemyKilledObserver scoreOnEnemyKilledObserver)
        {
            scoreOnEnemyKilledObserver.AddAction(AddScore);
        }

        void AddScore(ref ScoreActions item)
        {
            switch (item)
            {
                case ScoreActions.bunnyKilled:
                    _guiEntityView.scoreComponent.score += 10;
                    break;
                case ScoreActions.bearKilled:
                    _guiEntityView.scoreComponent.score += 20;
                    break;
                case ScoreActions.HellephantKilled:
                    _guiEntityView.scoreComponent.score += 30;
                    break;
            }
        }

        protected override void Add(HUDEntityView EntityView)
        {
            _guiEntityView = EntityView;
        }

        protected override void Remove(HUDEntityView EntityView)
        {
            _guiEntityView = null;
        }

        HUDEntityView _guiEntityView;
    }
}


