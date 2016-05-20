using System;
using Svelto.ES;
using Svelto.Ticker;
using UnityEngine;
using System.Collections.Generic;
using Nodes.Player;
using Components.Damageable;
using Observables.Enemies;

namespace Engines.Player
{
    public class PlayerShootingEngine : INodesEngine, ITickable
    {
        public PlayerShootingEngine(EnemyKilledObservable enemyKilledObservable)
        {
            _enemyKilledObservable = enemyKilledObservable;
        }

        public Type[] AcceptedNodes() { return _acceptedNodes; }

        public void Add(INode obj)
        {
            if (obj is PlayerGunNode)
                _playerGunNode = obj as PlayerGunNode;
            else
            if (obj is PlayerTargetNode)
            {
                var targetNode = obj as PlayerTargetNode;

                targetNode.healthComponent.isDead.subscribers += OnTargetDead;

                _playerTargets[targetNode.ID] = targetNode;
            }
            else
            if (obj is PlayerNode)
            {
                (obj as PlayerNode).healthComponent.isDead.subscribers += OnPlayerDead;
            }
        }

        public void Remove(INode obj)
        {
            if (obj is PlayerGunNode)
                _playerGunNode = null;
            else
            if (obj is PlayerTargetNode)
            {
                var targetNode = obj as PlayerTargetNode;

                targetNode.healthComponent.isDead.subscribers -= OnTargetDead;

                _playerTargets.Remove(targetNode.ID);
            }
            else
            if (obj is PlayerNode)
            {
                (obj as PlayerNode).healthComponent.isDead.subscribers -= OnPlayerDead;
            }
        }

        private void OnPlayerDead(int ID, int arg2)
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

            playerGunComponent.timer = 0;

            if (Physics.Raycast(playerGunComponent.shootRay, out shootHit, playerGunComponent.range, SHOOTABLE_MASK | ENEMY_MASK))
            {
                var hitGO = shootHit.collider.gameObject;

                PlayerTargetNode targetComponent = null;
                if (hitGO.layer == ENEMY_LAYER && _playerTargets.TryGetValue(hitGO.GetInstanceID(), out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, shootHit.point);
                    targetComponent.damageEventComponent.damageReceived.Dispatch(ref damageInfo);

                    playerGunComponent.lastTargetPosition = shootHit.point;
                    playerGunComponent.targetHit.value = true;

                    return;
                }
            }

            playerGunComponent.targetHit.value = false;
        }

        void OnTargetDead(int senderHealth, int targetID)
        {
            var playerTarget = _playerTargets[targetID];
            var targetType = playerTarget.targetTypeComponent.targetType;

            _enemyKilledObservable.Dispatch(ref targetType);
        }

        readonly Type[] _acceptedNodes = { typeof(PlayerTargetNode), typeof(PlayerGunNode), typeof(PlayerNode) };

        PlayerGunNode                                   _playerGunNode;
        Dictionary<int, PlayerTargetNode>               _playerTargets = new Dictionary<int, PlayerTargetNode>();

        EnemyKilledObservable _enemyKilledObservable;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
    }
}
