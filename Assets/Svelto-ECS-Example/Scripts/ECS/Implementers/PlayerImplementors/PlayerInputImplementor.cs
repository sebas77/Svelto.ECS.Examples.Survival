using Svelto.ECS.Example.Survive.EntityViews.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerInputImplementor:IPlayerInputComponent, IImplementor
    {
        public Vector3 input { get; set; }
        public Ray camRay { get; set; }
    }
}