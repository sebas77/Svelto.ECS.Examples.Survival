using Components.Gun;
using System;
using UnityEngine;

namespace Implementators.Player
{
    public class PlayerShooting : MonoBehaviour, IGunComponent, IGunFXComponent
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float range = 100f;                      // The distance the gun can fire.

        float IGunComponent.timeBetweenBullets { get { return timeBetweenBullets; } }
        float IGunComponent.range { get { return range; } }
        int IGunComponent.damagePerShot { get { return damagePerShot; } }
        DispatcherOnSet<int, bool> IGunComponent.targetHit { get { return _targetHit; } }
        Vector3 IGunComponent.lastTargetPosition { set { _lastTargetPosition = value; } get { return _lastTargetPosition; } }
        float IGunComponent.timer { get; set; }
        Ray IGunComponent.shootRay
        {
            get
            {
                _shootRay.origin = _transform.position;
                _shootRay.direction = _transform.forward;

                return _shootRay;
            }
        }

        ParticleSystem IGunFXComponent.particles { get { return _gunParticles; } }
        LineRenderer IGunFXComponent.line { get { return _gunLine; } }
        AudioSource IGunFXComponent.audio { get { return _gunAudio; } }
        Light IGunFXComponent.light { get { return _gunLight; }}

        float IGunFXComponent.effectsDisplayTime { get { return _effectsDisplayTime; } }

        void Awake ()
        {
            _transform = transform;

            // Set up the references.
            _gunParticles = GetComponent<ParticleSystem> ();
            _gunLine = GetComponent <LineRenderer> ();
            _gunAudio = GetComponent<AudioSource> ();
            _gunLight = GetComponent<Light> ();

            _targetHit = new DispatcherOnSet<int, bool>(this.GetInstanceID());
        }

        Transform       _transform;
        ParticleSystem  _gunParticles;                    // Reference to the particle system.
        LineRenderer    _gunLine;                           // Reference to the line renderer.
        AudioSource     _gunAudio;                           // Reference to the audio source.
        Light           _gunLight;                                 // Reference to the light component.
        float           _effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        Ray             _shootRay;
        Vector3         _lastTargetPosition;

        DispatcherOnSet<int, bool> _targetHit;
    }
}
