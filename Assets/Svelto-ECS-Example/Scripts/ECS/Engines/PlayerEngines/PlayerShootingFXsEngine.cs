using UnityEngine;
using System.Collections;
using Svelto.Tasks;
using Svelto.ECS.Example.Survive.Nodes.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingFXsEngine : SingleNodeEngine<GunNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public PlayerGunShootingFXsEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumeratorProvider(DisableFXAfterTime);
        }

        override protected void Add(GunNode playerGunNode)
        {
            _playerGunNode = playerGunNode;
            playerGunNode.gunHitTargetComponent.targetHit.NotifyOnValueSet(Shoot);
            _waitForSeconds = new WaitForSeconds(_playerGunNode.gunComponent.timeBetweenBullets * _playerGunNode.gunFXComponent.effectsDisplayTime);
        }

        override protected  void Remove(GunNode playerGunNode)
        {
            _playerGunNode = null;
        }

        void Shoot(int ID, bool targetHasBeenHit)
        {
            var playerGunNode = nodesDB.QueryNode<GunNode>(ID);

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

            _taskRoutine.Start();
        }

        IEnumerator DisableFXAfterTime()
        {           
            yield return _waitForSeconds;

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

        ITaskRoutine   _taskRoutine;
        GunNode        _playerGunNode;
        WaitForSeconds _waitForSeconds;
    }
}
