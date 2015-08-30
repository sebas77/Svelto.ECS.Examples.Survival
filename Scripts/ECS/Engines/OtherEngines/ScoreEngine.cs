using System;
using Svelto.ES;
using ScoreObservers;

namespace GUIEngines
{
    public class ScoreEngine : INodeEngine
    {
        public ScoreEngine(ScoreOnEnemyKilledObserver scoreOnEnemyKilledObserver)
        {
            scoreOnEnemyKilledObserver.AddAction(AddScore);
        }

        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj) { _guiNode = obj as GUINode; }
        public void Remove(INode obj) { _guiNode = null; }

        private void AddScore(ScoreActions item)
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

        Type[] _acceptedNodes = new Type[1] { typeof(GUINode) };

        GUINode _guiNode;
    }
}


