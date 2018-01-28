using UnityEngine;

namespace Svelto.ECS.Example.Survive.Components.Base
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
        Vector3 position { get; set; }
    }

    public interface IRigidBodyComponent: IComponent
    {
        Vector3 position { set; }
        bool isKinematic { set; }
        Quaternion rotation { set; }
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
