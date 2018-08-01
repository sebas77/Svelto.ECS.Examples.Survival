namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    static class ECSGroups
    {
        /// <summary>
        /// Reserving 3 exclusive groups, one for each enemy type
        /// </summary>
        public static readonly ExclusiveGroup EnemyToRecycleGroups = new ExclusiveGroup(3);
        public static readonly ExclusiveGroup EnemyDisabledGroups = new ExclusiveGroup(3);
    }
}