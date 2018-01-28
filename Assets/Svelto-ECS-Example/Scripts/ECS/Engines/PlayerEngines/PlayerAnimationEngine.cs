using Svelto.ECS.Example.Survive.EntityViews.Player;
using System.Collections;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Engines.Player
{
    public class PlayerAnimationEngine : IQueryingEntityViewEngine, IStep<TargetDamageInfo>
    {
        
        public IEntityViewsDB entityViewsDB { get; set; }
        public void Ready()
        {
            PhysicsTick().RunOnSchedule(StandardSchedulers.physicScheduler);
        }
        
        IEnumerator PhysicsTick()
        {
            var playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];

            while (playerEntityView == null)
            {
                yield return null;
                
                playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            }
            
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

        void TriggerDeathAnimation(int targetID)
        {
            var playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            
            playerEntityView.animationComponent.setTrigger("Die");
        }

        public void Step(ref TargetDamageInfo token, int condition)
        {
            TriggerDeathAnimation(token.entityDamaged);
        }
    }
}
