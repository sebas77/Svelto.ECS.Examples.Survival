using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerAnimationEngine : SingleEntityEngine<PlayerEntityView>, IStep<DamageInfo>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {
            _taskRoutine.Start();
        }
        
        public PlayerAnimationEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(PhysicsTick()).SetScheduler(StandardSchedulers.physicScheduler);
        }
        
        IEnumerator PhysicsTick()
        {
            while (entityViewsDB.Has<PlayerEntityView>() == false)
            {
                yield return null; //skip a frame
            }
            
            PlayerEntityView playerEntityView; entityViewsDB.Fetch(out playerEntityView);
            
            while (true)
            {
                var input = playerEntityView.inputComponent.input;

                // Create a boolean that is true if either of the input axes is non-zero.
                bool walking = input.x != 0f || input.z != 0f;

                // Tell the animator whether or not the player is walking.
                playerEntityView.animationComponent.setBool("IsWalking", walking);

                yield return null;
            }
        }

        void TriggerDeathAnimation(EGID targetID)
        {
            PlayerEntityView playerEntityView; entityViewsDB.Fetch(out playerEntityView);
            
            playerEntityView.animationComponent.playAnimation = "Die";
        }

        public void Step(ref DamageInfo token, int condition)
        {
            TriggerDeathAnimation(token.entityDamagedID);
        }

        protected override void Add(ref PlayerEntityView entityView)
        {}

        protected override void Remove(ref PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
        }
        
        readonly ITaskRoutine _taskRoutine;
    }
}
