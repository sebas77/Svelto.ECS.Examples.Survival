using UnityEngine;

namespace Svelto.ECS.Example.Survive.Components.Base
{
    //in order to avoid referencing directly the
    //unity Animation class, we explicitly create
    //setters that wrap the Animation class functions
    //Functions can be created in components
    //as long as they are used as setter and getter
    //of pure data (without logic)
    //this is especially true if you need to pass
    //valuetype by reference
    public interface IAnimationComponent: IComponent
    {
        void setBool(string name, bool value);
        void setTrigger(string name);
    }

    public interface IPositionComponent: IComponent
    {
        Vector3 position { get; }
    }

    public interface ITransformComponent: IComponent
    {
        Vector3 position { get; set; }
    }

    //Avoid to return a third party class (in this
    //case the RigidBody) when you can wrap
    //setters and getters
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
