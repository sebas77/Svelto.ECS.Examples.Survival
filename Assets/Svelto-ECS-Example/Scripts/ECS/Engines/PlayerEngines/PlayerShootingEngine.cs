using System;
using UnityEngine;
using Svelto.ECS.Example.Survive.EntityViews.Player;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Observables.Enemies;
using Svelto.ECS.Example.Survive.EntityViews.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingEngine : MultiEntityViewsEngine<GunEntityView, PlayerEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            TaskRunner.Instance.Run(new Tasks.TimedLoopActionEnumerator(Tick));
        }

        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable, Sequencer damageSequence)
        {
            _enemyKilledObservable = enemyKilledObservable;
            _enemyDamageSequence = damageSequence;
        }

        protected override void Add(GunEntityView EntityView)
        {
            _playerGunEntityView = EntityView;
        }

        protected override void Remove(GunEntityView EntityView)
        {}

        protected override void Add(PlayerEntityView EntityView)
        {}

        protected override void Remove(PlayerEntityView EntityView)
        {
            //the gun is never removed (because the level reloads on death), 
            //so remove on playerdeath
            _playerGunEntityView = null; 
        }

        void Tick(float deltaSec)
        {
            if (_playerGunEntityView == null) return;

            var playerGunComponent = _playerGunEntityView.gunComponent;

            playerGunComponent.timer += deltaSec;

            if (Input.GetButton("Fire1") && playerGunComponent.timer >= _playerGunEntityView.gunComponent.timeBetweenBullets && Time.timeScale != 0)
                Shoot();
        }

        void Shoot()
        {
            RaycastHit shootHit;
            var playerGunComponent = _playerGunEntityView.gunComponent;
            var playerGunHitComponent = _playerGunEntityView.gunHitTargetComponent;

            playerGunComponent.timer = 0;

            if (Physics.Raycast(playerGunComponent.shootRay, 
                out shootHit, playerGunComponent.range, SHOOTABLE_MASK | ENEMY_MASK))
            {
                var hitGO = shootHit.collider.gameObject;

                PlayerTargetEntityView targetComponent = null;
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (hitGO.layer == ENEMY_LAYER && entityViewsDB.TryQueryEntityView(hitGO.GetInstanceID(), out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, shootHit.point, hitGO.GetInstanceID());
                    _enemyDamageSequence.Next(this, ref damageInfo);

                    playerGunComponent.lastTargetPosition = shootHit.point;
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

        GunEntityView                 _playerGunEntityView;

        readonly EnemyKilledObservable   _enemyKilledObservable;
        readonly Sequencer               _enemyDamageSequence;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
        
    }
}
