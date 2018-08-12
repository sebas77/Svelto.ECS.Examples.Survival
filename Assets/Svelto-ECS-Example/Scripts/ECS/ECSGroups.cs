namespace Svelto.ECS.Example.Survive
{
    /// <summary>
    ///Create the Exclusive Groups use to build Entities in separate groups.
    ///you don't need to put all the ExclusiveGroup inside a single class
    ///you can decide to use them as you wish and create them wherever you want
    /// </summary>
    static class ECSGroups
    {
        /// <summary>
        /// Reserving 3 exclusive groups, one for each enemy type
        /// </summary>
        public static readonly ExclusiveGroup EnemiesToRecycleGroups = new ExclusiveGroup(3);
        public static readonly ExclusiveGroup DisabledEnemiesGroups = new ExclusiveGroup(3);
        /// <summary>
        /// while the active enemies share the same group to optimize the memory layout
        /// </summary>
        public static readonly ExclusiveGroup ActiveEnemiesGroup = new ExclusiveGroup();
        public static readonly ExclusiveGroup PlayerGroup = new ExclusiveGroup();
        public static readonly ExclusiveGroup ExtraStuffGroup = new ExclusiveGroup();
        
        public static readonly ExclusiveGroup PlayerTargetsGroup = ActiveEnemiesGroup;
        
        public static readonly ExclusiveGroup[] TargetGroups = { ActiveEnemiesGroup, PlayerGroup };
    }
}