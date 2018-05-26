using System.Collections.Generic;
using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive.Enemies
{
    static class ECSGroups
    {
        public static readonly Dictionary<PlayerTargetType, int> EnemyGroup = new Dictionary<PlayerTargetType, int>()
        {
            {PlayerTargetType.Bear, 0},
            {PlayerTargetType.Bunny, 1},
            {PlayerTargetType.Hellephant, 2}
        };
    }
}