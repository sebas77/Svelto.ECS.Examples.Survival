using Svelto.ECS.Example.Survive.Player;
using Svelto.Observer;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyKilledObservable : Observable<PlayerTargetType>
    { }
}
