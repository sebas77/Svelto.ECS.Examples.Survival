using System;
using Svelto.ES;
using UnityEngine;
using GunComponents;
using Svelto.Context;
using System.Collections;
using Svelto.Tasks;

namespace PlayerEngines
{
    public class PlayerShootingFXsEngine : INodeEngine
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public PlayerShootingFXsEngine()
        {
            _taskRoutine = TaskRunner.Instance.CreateTask(DisableFXAfterTime);
        }

        public void Add(INode obj)
        {
            _playerGunNode = obj as PlayerGunNode;

            _playerGunNode.gunComponent.targetHit.observers += Shoot;
        }

        public void Remove(INode obj)
        {
             _playerGunNode.gunComponent.targetHit.observers -= Shoot;

             _playerGunNode = null;
        }

        private void Shoot(IGunComponent gunComponent, bool targetHasBeenHit)
        {
            var gunFXComponent = _playerGunNode.gunFXComponent;
            // Play the gun shot audioclip.
            gunFXComponent.audio.Play ();

            // Enable the light.
            gunFXComponent.light.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunFXComponent.particles.Stop();
            gunFXComponent.particles.Play();

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

        Type[] _acceptedNodes = new Type[1] { typeof(PlayerGunNode) };

        PlayerGunNode _playerGunNode;

        TaskRoutine _taskRoutine;
    }
}
