using System;
using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Observables.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            Tick().Run();
        }

        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable, Sequencer damageSequence, RayCaster rayCaster)
        {
            _enemyKilledObservable = enemyKilledObservable;
            _enemyDamageSequence = damageSequence;
            _rayCaster = rayCaster;
        }

        IEnumerator Tick()
        {
            var playerGunEntityView = entityViewsDB.QueryEntityViews<GunEntityView>()[0];
            var playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];

            while (playerGunEntityView == null || playerEntityView == null)
            {
                yield return null;
                
                playerGunEntityView = entityViewsDB.QueryEntityViews<GunEntityView>()[0];
                playerEntityView = entityViewsDB.QueryEntityViews<PlayerEntityView>()[0];
            }

            var then = DateTime.Now;
            
            while (true)
            {
                var playerGunComponent = playerGunEntityView.gunComponent;

                playerGunComponent.timer += (float)(DateTime.Now - then).TotalSeconds;
                then = DateTime.Now;
                
                if (playerEntityView.inputComponent.fire &&
                    playerGunComponent.timer >= playerGunEntityView.gunComponent.timeBetweenBullets &&
                    Time.timeScale != 0)
                    Shoot(playerGunEntityView);

                yield return null;
            }
        }

        void Shoot(GunEntityView playerGunEntityView)
        {
            var playerGunComponent = playerGunEntityView.gunComponent;
            var playerGunHitComponent = playerGunEntityView.gunHitTargetComponent;

            playerGunComponent.timer = 0;

            Vector3 point;
            var entityHit = _rayCaster.CheckHit(playerGunComponent.shootRay, playerGunComponent.range, ENEMY_LAYER, SHOOTABLE_MASK | ENEMY_MASK, out point);
            
            if (entityHit != -1)
            {
                PlayerTargetEntityView targetComponent;
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (entityViewsDB.TryQueryEntityView(entityHit, out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, point, entityHit);
                    _enemyDamageSequence.Next(this, ref damageInfo);

                    playerGunComponent.lastTargetPosition = point;
                    playerGunHitComponent.targetHit.value = true;

                    return;
                }
            }

            playerGunHitComponent.targetHit.value = false;
        }

        void OnTargetDead(int targetID)
        {
            var playerTarget = entityViewsDB.QueryEntityView<PlayerTargetEntityView>(targetID);
            var targetType = playerTarget.targetTypeComponent.targetType;

            _enemyKilledObservable.Dispatch(ref targetType);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            OnTargetDead(token.entityDamaged);
        }

        readonly EnemyKilledObservable   _enemyKilledObservable;
        readonly Sequencer               _enemyDamageSequence;
        readonly RayCaster               _rayCaster;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
    }
}
