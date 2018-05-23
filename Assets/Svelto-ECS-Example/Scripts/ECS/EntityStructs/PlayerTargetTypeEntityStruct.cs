namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerTargetTypeEntityStruct: IEntityData
    {
        public readonly PlayerTargetType targetType;

        public PlayerTargetTypeEntityStruct(PlayerTargetType targetType):this()
        {
            this.targetType = targetType;
        }

        public EGID ID { get; set; }
    }
}
