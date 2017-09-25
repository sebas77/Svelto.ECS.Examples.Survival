using System;
using Svelto.ECS.Example.Survive.Nodes.HUD;
using Svelto.ECS.Example.Survive.Observers.HUD;

namespace Svelto.ECS.Example.Survive.Engines.HUD
{
    public class ScoreEngine : INodesEngine
    {
        public ScoreEngine(ScoreOnEnemyKilledObserver scoreOnEnemyKilledObserver)
        {
            scoreOnEnemyKilledObserver.AddAction(AddScore);
        }

        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj) { _guiNode = obj as HUDNode; }
        public void Remove(INode obj) { _guiNode = null; }

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

        readonly Type[] _acceptedNodes = { typeof(HUDNode) };

        HUDNode _guiNode;
    }
}


