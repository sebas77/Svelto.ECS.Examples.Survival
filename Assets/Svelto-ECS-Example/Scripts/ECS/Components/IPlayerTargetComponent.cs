namespace Svelto.ECS.Example.Survive.Player
{
    public interface IPlayerTargetComponent: IComponent
    {
        PlayerTargetType targetType { get; }
    }

    public enum PlayerTargetType
    {
        Bunny,
        Bear,
        Hellephant
    }
}
