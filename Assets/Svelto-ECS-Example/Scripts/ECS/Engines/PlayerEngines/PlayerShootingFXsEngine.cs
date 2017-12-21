using UnityEngine;
using System.Collections;
using Svelto.Tasks;
using Svelto.ECS.Example.Survive.EntityViews.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingFXsEngine : SingleEntityViewEngine<GunEntityView>, IQueryingEntityViewEngine
    {
        public IEngineEntityViewDB entityViewsDB { set; private get; }

        public void Ready()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumeratorProvider(DisableFXAfterTime);
        }

        protected override void Add(GunEntityView playerGunEntityView)
        {
            _playerGunEntityView = playerGunEntityView;
            playerGunEntityView.gunHitTargetComponent.targetHit.NotifyOnValueSet(Shoot);
            _waitForSeconds = new WaitForSeconds(_playerGunEntityView.gunComponent.timeBetweenBullets * _playerGunEntityView.gunFXComponent.effectsDisplayTime);
        }

        protected override void Remove(GunEntityView playerGunEntityView)
        {
            _playerGunEntityView = null;
        }

        void Shoot(int ID, bool targetHasBeenHit)
        {
            var playerGunEntityView = entityViewsDB.QueryEntityView<GunEntityView>(ID);

            var gunFXComponent = playerGunEntityView.gunFXComponent;

            // Play the gun shot audioclip.
            gunFXComponent.audio.Play ();

            // Enable the light.
            gunFXComponent.light.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunFXComponent.particles.Stop();
            gunFXComponent.particles.Play();

            var gunComponent = playerGunEntityView.gunComponent;

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
            var fxComponent = _playerGunEntityView.gunFXComponent;
            // Disable the line renderer and the light.
            fxComponent.line.enabled = false;
            fxComponent.light.enabled = false;
        }

        ITaskRoutine   _taskRoutine;
        GunEntityView        _playerGunEntityView;
        WaitForSeconds _waitForSeconds;
    }
}
