using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerAnimationEngine : SingleEntityEngine<PlayerEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo, DamageCondition>
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
            while (entityViewsDB.HasAny<PlayerEntityView>() == false)
            {
                yield return null; //skip a frame
            }

            int targetsCount;
            var playerEntityViews = entityViewsDB.QueryEntities<PlayerEntityView>(out targetsCount);
            var playerInputDatas = entityViewsDB.QueryEntities<PlayerInputDataStruct>(out targetsCount);
            
            while (true)
            {
                var input = playerInputDatas[0].input;

                // Create a boolean that is true if either of the input axes is non-zero.
                bool walking = input.x != 0f || input.z != 0f;

                // Tell the animator whether or not the player is walking.
                playerEntityViews[0].animationComponent.setState("IsWalking", walking);

                yield return null;
            }
        }

        void TriggerDeathAnimation(EGID targetID)
        {
            uint index;
            var playerEntityViews = entityViewsDB.QueryEntitiesAndIndex<PlayerEntityView>(targetID, out index);
            
            playerEntityViews[index].animationComponent.playAnimation = "Die";
        }

        public void Step(ref DamageInfo token, DamageCondition condition)
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
