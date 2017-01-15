using UnityEngine;

namespace Svelto.ECS.Example.Components.Base
{
    public interface IAnimationComponent: IComponent
    {
        Animator animation { get; }
    }

    public interface IPositionComponent: IComponent
    {
        Vector3 position { get; }
    }

    public interface ITransformComponent: IComponent
    {
        Transform transform { get; }
    }

    public interface IRigidBodyComponent: IComponent
    {
        Rigidbody rigidbody { get; }
    }

    public interface ISpeedComponent: IComponent
    {
        float speed { get; }
    }

    public interface IDamageSoundComponent: IComponent
    {
        AudioSource audioSource { get; }
        AudioClip   death       { get; }
        AudioClip   damage      { get; }
    }
}
