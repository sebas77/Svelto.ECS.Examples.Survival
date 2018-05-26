namespace Svelto.ECS.Example.Survive.Player
{
    public struct PlayerTargetTypeEntityStruct: IEntityData
    {
        public PlayerTargetType targetType;
        public EGID ID { get; set; }
    }
}
