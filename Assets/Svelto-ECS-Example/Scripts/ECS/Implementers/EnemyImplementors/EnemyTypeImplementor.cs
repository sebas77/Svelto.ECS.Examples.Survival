using Svelto.ECS.Example.Survive.Components.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyTypeImplementor : MonoBehaviour, IImplementor 
        , ITargetTypeComponent
    {
        public PlayerTargetType targetType;

        PlayerTargetType ITargetTypeComponent.targetType { get { return targetType; } }
    }
}
