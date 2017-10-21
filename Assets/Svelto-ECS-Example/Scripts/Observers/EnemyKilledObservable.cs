using Svelto.ECS.Example.Survive.Components.Player;
using Svelto.Observer;

namespace Svelto.ECS.Example.Survive.Observables.Enemies
{
    public class EnemyKilledObservable : Observable<PlayerTargetType>
    { }
}
