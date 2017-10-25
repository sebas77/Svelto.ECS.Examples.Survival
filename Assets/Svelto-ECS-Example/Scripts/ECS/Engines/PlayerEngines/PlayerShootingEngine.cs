using System;
using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Player;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Observables.Enemies;
using Svelto.ECS.Example.Survive.Nodes.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingEngine : MultiNodesEngine<GunNode, PlayerNode>, IQueryableNodeEngine, IStep<DamageInfo>
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable, Sequencer damageSequence)
        {
            _enemyKilledObservable = enemyKilledObservable;
            _enemyDamageSequence = damageSequence;

            TaskRunner.Instance.Run(new Tasks.TimedLoopActionEnumerator(Tick));
        }

        protected override void AddNode(GunNode node)
        {
            _playerGunNode = node;
        }

        protected override void RemoveNode(GunNode node)
        {}

        protected override void AddNode(PlayerNode node)
        {}

        protected override void RemoveNode(PlayerNode node)
        {
            //the gun is never removed (because the level reloads on death), 
            //so remove on playerdeath
            _playerGunNode = null; 
        }

        void Tick(float deltaSec)
        {
            if (_playerGunNode == null) return;

            var playerGunComponent = _playerGunNode.gunComponent;

            playerGunComponent.timer += deltaSec;

            if (Input.GetButton("Fire1") && playerGunComponent.timer >= _playerGunNode.gunComponent.timeBetweenBullets && Time.timeScale != 0)
                Shoot();
        }

        void Shoot()
        {
            RaycastHit shootHit;
            var playerGunComponent = _playerGunNode.gunComponent;
            var playerGunHitComponent = _playerGunNode.gunHitTargetComponent;

            playerGunComponent.timer = 0;

            if (Physics.Raycast(playerGunComponent.shootRay, 
                out shootHit, playerGunComponent.range, SHOOTABLE_MASK | ENEMY_MASK))
            {
                var hitGO = shootHit.collider.gameObject;

                PlayerTargetNode targetComponent = null;
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (hitGO.layer == ENEMY_LAYER && nodesDB.TryQueryNode(hitGO.GetInstanceID(), out targetComponent))
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
            var playerTarget = nodesDB.QueryNode<PlayerTargetNode>(targetID);
            var targetType = playerTarget.targetTypeComponent.targetType;

            _enemyKilledObservable.Dispatch(ref targetType);
        }

        public void Step(ref DamageInfo token, Enum condition)
        {
            OnTargetDead(token.entityDamaged);
        }

        GunNode                 _playerGunNode;

        readonly EnemyKilledObservable   _enemyKilledObservable;
        readonly Sequencer               _enemyDamageSequence;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
        
    }
}
