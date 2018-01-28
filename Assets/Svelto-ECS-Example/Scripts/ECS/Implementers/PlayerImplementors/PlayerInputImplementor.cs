using System.Collections;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerInputImplementor:IPlayerInputComponent, IImplementor
    {
        public PlayerInputImplementor()
        {
            ReadInput().Run();
        }

        IEnumerator ReadInput()
        {
            while (true)
            {
                float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
                float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

                input = new Vector3(h, 0f, v);
                camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                yield return null;
            }
        }

        public Vector3 input { get; private set; }
        public Ray camRay { get; private set; }
    }
}