namespace Svelto.ECS.Example.Components.Player
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

	public enum PlayerDamagedType
	{
		Hurt,
		Killed,
	}
}
