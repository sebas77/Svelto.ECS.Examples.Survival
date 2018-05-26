namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerTargetTypeEntityStruct: IEntityStruct
    {
        public PlayerTargetType targetType;
        public EGID ID { get; set; }
    }
}
