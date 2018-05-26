using System.Collections;
using UnityEngine;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class PlayerGunShootingEngine : MultiEntitiesEngine<GunEntityView, PlayerEntityView>, 
        IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            _taskRoutine.Start();
        }
        
        public PlayerGunShootingEngine(ISequencer damageSequence, IRayCaster rayCaster, ITime time)
        {
            _enemyDamageSequence   = damageSequence;
            _rayCaster             = rayCaster;
            _time                  = time;
            _taskRoutine           = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick())
                                               .SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(ref GunEntityView entityView)
        {}

        protected override void Remove(ref GunEntityView entityView)
        {
            _taskRoutine.Stop();
        }

        protected override void Add(ref PlayerEntityView entityView)
        {}

        protected override void Remove(ref PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
        }

        IEnumerator Tick()
        {
            while (entityViewsDB.HasAny<PlayerEntityView>() == false || entityViewsDB.HasAny<GunEntityView>() == false)
            {
                yield return null; //skip a frame
            }

            int count;
            var playerGunEntities = entityViewsDB.QueryEntities<GunEntityView>(out count);
            var playerEntities = entityViewsDB.QueryEntities<PlayerEntityView>(out count);
            
            while (true)
            {
                var playerGunComponent = playerGunEntities[0].gunComponent;

                playerGunComponent.timer += _time.deltaTime;
                
                if (playerEntities[0].inputComponent.fire &&
                    playerGunComponent.timer >= playerGunEntities[0].gunComponent.timeBetweenBullets)
                    Shoot(playerGunEntities[0]);

                yield return null;
            }
        }

        void Shoot(GunEntityView playerGunEntityView)
        {
            var playerGunComponent    = playerGunEntityView.gunComponent;
            var playerGunHitComponent = playerGunEntityView.gunHitTargetComponent;

            playerGunComponent.timer = 0;

            Vector3 point;
            var     entityHit = _rayCaster.CheckHit(playerGunComponent.shootRay, playerGunComponent.range, ENEMY_LAYER, SHOOTABLE_MASK | ENEMY_MASK, out point);
            
            if (entityHit != -1)
            {
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (entityViewsDB.Exists<PlayerTargetTypeEntityStruct>(new EGID(entityHit)))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, point, new EGID(entityHit), EntityDamagedType.Enemy);
                    _enemyDamageSequence.Next(this, ref damageInfo);

                    playerGunComponent.lastTargetPosition = point;
                    playerGunHitComponent.targetHit.value = true;

                    return;
                }
            }

            playerGunHitComponent.targetHit.value = false;
        }

        readonly ISequencer    _enemyDamageSequence;
        readonly IRayCaster    _rayCaster;
        readonly ITime         _time;
        readonly ITaskRoutine  _taskRoutine;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK     = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER    = LayerMask.NameToLayer("Enemies");
    }
}