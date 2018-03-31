namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerTargetTypeImplementor : IImplementor 
        , IPlayerTargetComponent
    {
        readonly PlayerTargetType _targetType;

        public PlayerTargetTypeImplementor(PlayerTargetType targetType)
        {
            _targetType = targetType;
        }

        public PlayerTargetType targetType { get { return _targetType; } }
    }
}
