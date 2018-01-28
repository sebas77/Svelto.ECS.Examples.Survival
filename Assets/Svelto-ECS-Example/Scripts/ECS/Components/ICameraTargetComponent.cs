using UnityEngine;

namespace Svelto.ECS.Example.Survive.Components.Camera
{
    public interface ICameraTargetComponent
    {
        Vector3 position { get; }
    }
}