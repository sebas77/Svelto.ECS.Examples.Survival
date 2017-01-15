using Svelto.ECS.Example.Components.Player;
using Svelto.Observer;

namespace Svelto.ECS.Example.Observables.Enemies
{
    public class EnemyKilledObservable : Observable<PlayerTargetType>
    { }
}
