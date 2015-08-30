using PlayerComponents;
using Svelto.Observer;
using System.Collections.Generic;

namespace ScoreObservers
{
    public class ScoreOnEnemyKilledObserver:Observer<PlayerTargetType, ScoreActions>
    {
        public ScoreOnEnemyKilledObserver(EnemyObservables.EnemyKilledObservable observable): base(observable)
        {}

        protected override ScoreActions TypeMap(PlayerTargetType dispatchNotification)
        {
            return _targetTypeToScoreAction[dispatchNotification];
        }

        Dictionary<PlayerTargetType, ScoreActions> _targetTypeToScoreAction = new Dictionary<PlayerTargetType, ScoreActions>() 
        { 
            { PlayerTargetType.Bear, ScoreActions.bearKilled },
            { PlayerTargetType.Bunny, ScoreActions.bunnyKilled },
            { PlayerTargetType.Hellephant, ScoreActions.HellephantKilled },
        };
    }

    public enum ScoreActions
    {
        bunnyKilled,
        bearKilled,
        HellephantKilled
    }
}
