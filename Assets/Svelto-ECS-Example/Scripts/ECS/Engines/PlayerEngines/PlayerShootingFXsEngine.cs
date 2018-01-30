using UnityEngine;
using System.Collections;
using Svelto.Tasks;
using Svelto.ECS.Example.Survive.EntityViews.Gun;

namespace Svelto.ECS.Example.Survive.Engines.Player.Gun
{
    public class PlayerGunShootingFXsEngine : SingleEntityViewEngine<GunEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            //In this case a taskroutine is used because we want to have control over when it starts
            //and we want to reuse it.
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumeratorProvider(DisableFXAfterTime);
        }

        /// <summary>
        /// Using the Add/Remove method to hold a local reference of an entity
        /// is not necessary. Do it only if you find covenient, otherwise
        /// querying is always cleaner.
        /// </summary>
        /// <param name="playerGunEntityView"></param>
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
            gunFXComponent.playAudio = true;

            // Enable the light.
            gunFXComponent.lightEnabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunFXComponent.play = false;
            gunFXComponent.play = true;

            var gunComponent = playerGunEntityView.gunComponent;
            var shootRay = gunComponent.shootRay;

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunFXComponent.lineEnabled = true;
            gunFXComponent.lineStartPosition = shootRay.origin;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (targetHasBeenHit == true)
            {
                // Set the second position of the line renderer to the point the raycast hit.
                gunFXComponent.lineEndPosition = gunComponent.lastTargetPosition;
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                 gunFXComponent.lineEndPosition = shootRay.origin + shootRay.direction * gunComponent.range;
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
            fxComponent.lineEnabled = false;
            fxComponent.lightEnabled = false;
        }

        ITaskRoutine   _taskRoutine;
        GunEntityView  _playerGunEntityView;
        WaitForSeconds _waitForSeconds;
    }
}
