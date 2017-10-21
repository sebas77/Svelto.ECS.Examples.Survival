using System;
using Svelto.ECS.Example.Survive.Nodes.HUD;
using Svelto.ECS.Example.Survive.Observers.HUD;

namespace Svelto.ECS.Example.Survive.Engines.HUD
{
    public class ScoreEngine : SingleNodeEngine<HUDNode>
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
                    _guiNode.scoreComponent.score += 10;
                    break;
                case ScoreActions.bearKilled:
                    _guiNode.scoreComponent.score += 20;
                    break;
                case ScoreActions.HellephantKilled:
                    _guiNode.scoreComponent.score += 30;
                    break;
            }
        }

        protected override void Add(HUDNode node)
        {
            _guiNode = node;
        }

        protected override void Remove(HUDNode node)
        {
            _guiNode = null;
        }

        HUDNode _guiNode;
    }
}


