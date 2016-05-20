using System;
using Svelto.ES;
using UnityEngine;
using System.Collections;
using Svelto.Tasks;
using Nodes.Player;

namespace Engines.Player
{
    public class PlayerShootingFXsEngine : SingleNodeEngine<PlayerGunNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public PlayerShootingFXsEngine()
        {
            _taskRoutine = TaskRunner.Instance.CreateTask(DisableFXAfterTime);
        }

        override protected void Add(PlayerGunNode playerGunNode)
        {
            _playerGunNode = playerGunNode;
            playerGunNode.gunComponent.targetHit.subscribers += Shoot;
        }

        override protected  void Remove(PlayerGunNode playerGunNode)
        {
            _playerGunNode = null;
            playerGunNode.gunComponent.targetHit.subscribers -= Shoot;
        }

        private void Shoot(int ID, bool targetHasBeenHit)
        {
            var playerGunNode = nodesDB.QueryNode<PlayerGunNode>(ID);

            var gunFXComponent = playerGunNode.gunFXComponent;
            // Play the gun shot audioclip.
            gunFXComponent.audio.Play ();

            // Enable the light.
            gunFXComponent.light.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunFXComponent.particles.Stop();
            gunFXComponent.particles.Play();

            var gunComponent = playerGunNode.gunComponent;

            var shootRay = gunComponent.shootRay;

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunFXComponent.line.enabled = true;
            gunFXComponent.line.SetPosition(0, shootRay.origin);

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (targetHasBeenHit == true)
            {
                // Set the second position of the line renderer to the point the raycast hit.
                gunFXComponent.line.SetPosition (1, gunComponent.lastTargetPosition);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                 gunFXComponent.line.SetPosition(1, shootRay.origin + shootRay.direction * gunComponent.range);
            }

            _taskRoutine.Start(true);
        }

        IEnumerator DisableFXAfterTime()
        {
            yield return new WaitForSeconds(_playerGunNode.gunComponent.timeBetweenBullets * _playerGunNode.gunFXComponent.effectsDisplayTime);

            // ... disable the effects.
            DisableEffects();
        }

        void DisableEffects ()
        {
            var fxComponent = _playerGunNode.gunFXComponent;
            // Disable the line renderer and the light.
            fxComponent.line.enabled = false;
            fxComponent.light.enabled = false;
        }

        TaskRoutine     _taskRoutine;
        PlayerGunNode   _playerGunNode;
    }
}
