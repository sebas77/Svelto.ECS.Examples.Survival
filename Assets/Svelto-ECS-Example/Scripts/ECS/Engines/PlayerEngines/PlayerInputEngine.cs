using System.Collections;
using Svelto.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Svelto.ECS.Example.Survive.Player
{
    /// <summary>
    /// if you need to test input, you can mock this class
    /// alternativaly you can mock the implementor.
    /// </summary>
    public class PlayerInputEngine:SingleEntityEngine<PlayerEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {}
        public PlayerInputEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(ReadInput());
        }

        IEnumerator ReadInput()
        {
            while (entityViewsDB.Has<PlayerEntityView>() == false)
            {
                yield return null; //skip a frame
            }
            
            PlayerEntityView playerEntityView; entityViewsDB.Fetch(out playerEntityView);
           
            while (true)
            {
                float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
                float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

                playerEntityView.inputComponent.input = new Vector3(h, 0f, v);
                playerEntityView.inputComponent.camRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                playerEntityView.inputComponent.fire = Input.GetButton("Fire1");
                
                yield return null;
            }
        }

        protected override void Add(ref PlayerEntityView entityView)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(ref PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
        }
        
        ITaskRoutine _taskRoutine;
    }
}