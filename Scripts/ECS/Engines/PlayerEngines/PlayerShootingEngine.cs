using System;
using Svelto.ES;
using Svelto.Ticker;
using UnityEngine;
using System.Collections.Generic;
using SharedComponents;
using EnemyObservables;

namespace PlayerEngines
{
    public class PlayerShootingEngine : INodeEngine, ITickable
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

                targetNode.healthComponent.isDead.observers += OnTargetDead;

                _playerTargets[targetNode.ID] = targetNode;
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

                targetNode.healthComponent.isDead.observers -= OnTargetDead;

                _playerTargets.Remove(targetNode.ID);
            }
        }

        public void Tick(float deltaSec)
        {
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
                if (hitGO.layer == ENEMY_LAYER && _playerTargets.TryGetValue(hitGO, out targetComponent))
                {
                    targetComponent.damageEventComponent.damageReceived.Dispatch(new DamageInfo(playerGunComponent.damagePerShot, shootHit.point));
                    playerGunComponent.lastTargetPosition = shootHit.point;
                    playerGunComponent.targetHit.Dispatch(true);

                    return;
                }
            }
            
            playerGunComponent.targetHit.Dispatch(false);
        }

        void OnTargetDead(IHealthComponent arg1, GameObject target)
        {
            var playerTarget = _playerTargets[target];

            _enemyKilledObservable.Dispatch(playerTarget.targetTypeComponent.targetType);
        }

        Type[] _acceptedNodes = new Type[2] { typeof(PlayerTargetNode), typeof(PlayerGunNode) };

        PlayerGunNode                                   _playerGunNode;
        Dictionary<GameObject, PlayerTargetNode>        _playerTargets = new Dictionary<GameObject, PlayerTargetNode>();

        EnemyKilledObservable _enemyKilledObservable;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
    }
}
