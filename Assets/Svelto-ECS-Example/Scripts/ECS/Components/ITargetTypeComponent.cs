using Svelto.ES;

namespace Components.Player
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
