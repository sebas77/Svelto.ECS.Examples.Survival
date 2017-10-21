using UnityEngine;

namespace Svelto.ECS.Example.Parallelism
{
    public interface IBoidComponent
    {
        Vector3 velocity { get; set; }
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }
    }
}