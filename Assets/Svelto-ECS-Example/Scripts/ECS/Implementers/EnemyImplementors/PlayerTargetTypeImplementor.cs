using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerTargetTypeImplementor : MonoBehaviour, IImplementor 
        , IPlayerTargetComponent
    {
        public PlayerTargetType targetType;

        PlayerTargetType IPlayerTargetComponent.targetType { get { return targetType; } }
    }
}
