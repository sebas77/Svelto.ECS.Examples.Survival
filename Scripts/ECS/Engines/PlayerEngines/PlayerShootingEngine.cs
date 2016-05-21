using System;
using Svelto.ES;
using Svelto.Ticker;
using UnityEngine;
using Nodes.Player;
using Components.Damageable;
using Observables.Enemies;
using Nodes.Gun;

namespace Engines.Player.Gun
{
    public class PlayerGunShootingEngine : INodesEngine, ITickable, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable)
        {
            _enemyKilledObservable = enemyKilledObservable;
        }

        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj)
        {
            if (obj is GunNode)
                _playerGunNode = obj as GunNode;
            else
            if (obj is PlayerTargetNode)
            {
                var targetNode = obj as PlayerTargetNode;

                targetNode.healthComponent.isDead.subscribers += OnTargetDead;
            }
            else
            if (obj is PlayerNode)
            {
                (obj as PlayerNode).healthComponent.isDead.subscribers += OnPlayerDead;
            }
        }

        public void Remove(INode obj)
        {
            if (obj is GunNode)
                _playerGunNode = null;
            else
            if (obj is PlayerTargetNode)
            {
                var targetNode = obj as PlayerTargetNode;

                targetNode.healthComponent.isDead.subscribers -= OnTargetDead;
            }
            else
            if (obj is PlayerNode)
            {
                (obj as PlayerNode).healthComponent.isDead.subscribers -= OnPlayerDead;
            }
        }

        private void OnPlayerDead(int ID)
        {
            _playerGunNode = null;
        }

        public void Tick(float deltaSec)
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

            if (Physics.Raycast(playerGunComponent.shootRay, out shootHit, playerGunComponent.range, SHOOTABLE_MASK | ENEMY_MASK))
            {
                var hitGO = shootHit.collider.gameObject;

                PlayerTargetNode targetComponent = null;
                
                if (hitGO.layer == ENEMY_LAYER && nodesDB.QueryNode<PlayerTargetNode>(hitGO.GetInstanceID(), out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, shootHit.point);
                    targetComponent.damageEventComponent.damageReceived.Dispatch(ref damageInfo);

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

        readonly Type[] _acceptedNodes = { typeof(PlayerTargetNode), typeof(GunNode), typeof(PlayerNode) };

        GunNode                 _playerGunNode;
        EnemyKilledObservable   _enemyKilledObservable;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
    }
}
