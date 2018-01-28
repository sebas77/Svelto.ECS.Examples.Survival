using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Components.Enemies;
using Svelto.ECS.Example.Survive.Components.Player;
using Svelto.ECS.Example.Survive.EntityViews.Enemies;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyHealthImplementor : MonoBehaviour, IImplementor, 
        IHealthComponent, IAnimationComponent, IEnemyVFXComponent, IDamageSoundComponent, ITargetTypeComponent, IDestroyComponent
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        public AudioClip damageClip;                 // The sound to play when the enemy dies.
        public PlayerTargetType targetType;

        int   IHealthComponent.currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

        ParticleSystem IEnemyVFXComponent.hitParticles { get { return _hitParticles; } }

        AudioSource IDamageSoundComponent.audioSource { get { return _enemyAudio; } }
        AudioClip IDamageSoundComponent.death { get { return deathClip; } }
        AudioClip IDamageSoundComponent.damage { get { return damageClip; } }

        PlayerTargetType ITargetTypeComponent.targetType { get { return targetType; } }
        
        public void setBool(string name, bool value)
        {
            _anim.SetBool(name, value);
        }

        public void setTrigger(string name)
        {
            _anim.SetTrigger(name);
        }

        void Awake ()
        {
            // Setting up the references.
            _anim = GetComponent <Animator> ();
            _enemyAudio = GetComponent <AudioSource> ();
            _hitParticles = GetComponentInChildren <ParticleSystem> ();

            // Setting the current health when the enemy first spawns.
            _currentHealth = startingHealth;
            destroyed = new DispatchOnChange<bool>(GetInstanceID());
            destroyed.NotifyOnValueSet(OnDestroyed);
        }

        void OnDestroyed(int sender, bool isDestroyed)
        {
            Destroy(this.gameObject);
        }

        public DispatchOnChange<bool> destroyed { get; private set; }

        Animator        _anim;                 // Reference to the animator.
        AudioSource     _enemyAudio;           // Reference to the audio source.
        ParticleSystem  _hitParticles;         // Reference to the particle system that plays when the enemy is damaged.
        int             _currentHealth;        // The current health the enemy has.
    }
}
