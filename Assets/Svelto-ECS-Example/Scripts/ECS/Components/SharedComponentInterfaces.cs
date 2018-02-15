using UnityEngine;

namespace Svelto.ECS.Example.Survive
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
        string trigger { set; }
    }

    public interface IPositionComponent: IComponent
    {
        Vector3 position { get; }
    }

    public interface ITransformComponent: IPositionComponent
    {
        Vector3 position { set; }
        Quaternion rotation { set; }
    }

    public interface IRigidBodyComponent: IComponent
    {
        bool isKinematic { set; }
    }

    public interface ISpeedComponent: IComponent
    {
        float movementSpeed { get; }
    }

    public interface IDamageSoundComponent: IComponent
    {
        AudioType playOneShot { set; }
    }
}
