namespace Svelto.ECS.Example.Survive.Components.Player
{
    public interface ITargetTypeComponent: IComponent
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
