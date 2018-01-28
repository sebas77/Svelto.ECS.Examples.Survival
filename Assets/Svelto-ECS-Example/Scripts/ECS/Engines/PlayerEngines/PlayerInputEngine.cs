using System.Collections;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    /// <summary>
    /// if you need to test input, you can mock this class
    /// alternativaly you can mock the implementor.
    /// </summary>
    public class PlayerInputEngine:SingleEntityViewEngine<PlayerEntityView>
    {
        public PlayerInputEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(ReadInput());
        }

        IEnumerator ReadInput()
        {
            while (true)
            {
                float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
                float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

                _playerEntityView.inputComponent.input = new Vector3(h, 0f, v);
                _playerEntityView.inputComponent.camRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                _playerEntityView.inputComponent.fire = Input.GetButton("Fire1");
                
                yield return null;
            }
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
            _taskRoutine.Start();
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerEntityView = null;
        }
        
        ITaskRoutine _taskRoutine;
        PlayerEntityView _playerEntityView;
    }
}