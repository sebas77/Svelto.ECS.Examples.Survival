using UnityEngine;

namespace Svelto.ECS.Example.Flock
{
    public interface IThreadSafeTransformComponent
    {
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }
    }
}