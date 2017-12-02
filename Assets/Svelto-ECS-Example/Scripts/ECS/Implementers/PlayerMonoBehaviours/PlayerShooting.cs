using Svelto.ECS.Example.Survive.Components.Gun;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerShooting : MonoBehaviour, IGunAttributesComponent, IGunFXComponent, IGunHitTargetComponent
    {
        public int DamagePerShot = 20;                  // The damage inflicted by each bullet.
        public float TimeBetweenBullets = 0.15f;        // The time between each shot.
        public float Range = 100f;                      // The distance the gun can fire.

        public float timeBetweenBullets { get { return TimeBetweenBullets; } }
        public float range { get { return Range; } }
        public int damagePerShot { get { return DamagePerShot; } }
        public DispatchOnSet<bool> targetHit { get { return _targetHit; } }
        public Vector3 lastTargetPosition { set { _lastTargetPosition = value; } get { return _lastTargetPosition; } }
        public float timer { get; set; }
        public Ray shootRay
        {
            get
            {
                return new Ray(_transform.position, _transform.forward);
            }
        }

        public ParticleSystem particles { get { return _gunParticles; } }
        public LineRenderer line { get { return _gunLine; } }
        new public AudioSource audio { get { return _gunAudio; } }
        new public Light light { get { return _gunLight; }}

        public float effectsDisplayTime { get { return _effectsDisplayTime; } }

        void Awake ()
        {
            _transform = transform;

            // Set up the references.
            _gunParticles = GetComponent<ParticleSystem> ();
            _gunLine = GetComponent <LineRenderer> ();
            _gunAudio = GetComponent<AudioSource> ();
            _gunLight = GetComponent<Light> ();

            _targetHit = new DispatchOnSet<bool>(gameObject.GetInstanceID());
        }

        Transform       _transform;
        ParticleSystem  _gunParticles;                      // Reference to the particle system.
        LineRenderer    _gunLine;                           // Reference to the line renderer.
        AudioSource     _gunAudio;                          // Reference to the audio source.
        Light           _gunLight;                          // Reference to the light component.
        float           _effectsDisplayTime = 0.2f;         // The proportion of the timeBetweenBullets that the effects will display for.
        Vector3         _lastTargetPosition;

        DispatchOnSet<bool> _targetHit;
    }
}
