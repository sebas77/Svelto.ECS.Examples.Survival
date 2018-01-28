using System.Collections;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    /// <summary>
    /// if you need to test input, you can mock the PlayerInputEngine
    /// </summary>
    public class PlayerInputEngine:IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {
            ReadInput().Run();
        }

        IEnumerator ReadInput()
        {
            PlayerEntityView entityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];

            while (entityView == null)
            {
                yield return null;
                
                entityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            } 
            
            while (true)
            {
                float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
                float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

                entityView.inputComponent.input = new Vector3(h, 0f, v);
                entityView.inputComponent.camRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                
                yield return null;
            }
        }
    }
}