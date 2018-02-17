using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerInputImplementor:IPlayerInputComponent, IImplementor
    {
        public Vector3 input { get; set; }
        public Ray camRay { get; set; }
        public bool fire { get; set; }
    }
}