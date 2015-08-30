using UnityEngine;

namespace SharedComponents
{
    public interface IAnimationComponent
    {
        Animator animation { get; }
    }

    public interface IPositionComponent
    {
        Vector3 position { get; }
    }

    public interface ITransformComponent
    {
        Transform transform { get; }
    }

    public interface IRigidBodyComponent
    {
        Rigidbody rigidbody { get; }
    }

    public interface ISpeedComponent
    {
        float speed { get; }
    }

    public interface IDamageSoundComponent
    {
        AudioSource audioSource { get; }
        AudioClip   death { get;  }
        AudioClip   damage { get;  }
    }
}
