using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerAnimationEngine : SingleEntityViewEngine<PlayerEntityView>, IStep<DamageInfo>
    {
        public PlayerAnimationEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicsTick()).SetScheduler(StandardSchedulers.physicScheduler);
        }
        
        IEnumerator PhysicsTick()
        {
            while (true)
            {
                var input = _playerEntityView.inputComponent.input;

                // Create a boolean that is true if either of the input axes is non-zero.
                bool walking = input.x != 0f || input.z != 0f;

                // Tell the animator whether or not the player is walking.
                _playerEntityView.animationComponent.setBool("IsWalking", walking);

                yield return null;
            }
        }

        void TriggerDeathAnimation(int targetID)
        {
            _playerEntityView.animationComponent.trigger = "Die";
        }

        public void Step(ref DamageInfo token, int condition)
        {
            TriggerDeathAnimation(token.entityDamagedID);
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
        
        PlayerEntityView _playerEntityView;
        readonly ITaskRoutine _taskRoutine;
    }
}
