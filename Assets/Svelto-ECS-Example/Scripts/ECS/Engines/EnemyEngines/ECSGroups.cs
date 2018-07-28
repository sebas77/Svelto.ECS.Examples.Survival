using System.Collections.Generic;
using Svelto.ECS.Example.Survive.Characters.Player;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    static class ECSGroups
    {
        public static ExclusiveGroup enemyDisabledGroups = new ExclusiveGroup(3);
    }
}