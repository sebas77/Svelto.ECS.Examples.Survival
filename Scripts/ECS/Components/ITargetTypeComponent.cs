namespace PlayerComponents
{
    public interface ITargetTypeComponent
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
