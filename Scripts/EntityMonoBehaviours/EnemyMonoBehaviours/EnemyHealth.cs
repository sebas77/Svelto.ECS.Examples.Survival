using Components.Base;
using Components.Damageable;
using Components.Enemy;
using Components.Player;
using UnityEngine;

namespace Implementators.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealthComponent, IAnimationComponent, IEnemyVFXComponent, IDamageSoundComponent, IDamageEventComponent, ITargetTypeComponent
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        public AudioClip damageClip;                 // The sound to play when the enemy dies.
        public PlayerTargetType targetType;

        Dispatcher<int, DamageInfo>  IDamageEventComponent.damageReceived { get { return _damageReceived; } }

        int   IHealthComponent.currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
        bool  IHealthComponent.hasBeenDamaged { get; set; }

        Dispatcher<int, int>           IHealthComponent.isDead { get { return _isDead; } }
        Dispatcher<int, DamageInfo>    IHealthComponent.isDamaged { get { return _isDamaged; } }

        Animator IAnimationComponent.animation { get { return _anim; } }

        ParticleSystem IEnemyVFXComponent.hitParticles { get { return _hitParticles; } }

        AudioSource IDamageSoundComponent.audioSource { get { return _enemyAudio; } }
        AudioClip IDamageSoundComponent.death { get { return deathClip; } }
        AudioClip IDamageSoundComponent.damage { get { return damageClip; } }

        PlayerTargetType ITargetTypeComponent.targetType { get { return targetType; } }

        void Awake ()
        {
            // Setting up the references.
            _anim = GetComponent <Animator> ();
            _enemyAudio = GetComponent <AudioSource> ();
            _hitParticles = GetComponentInChildren <ParticleSystem> ();

            // Setting the current health when the enemy first spawns.
            _currentHealth = startingHealth;

            _damageReceived = new Dispatcher<int, DamageInfo>(this.GetInstanceID());
            _isDead = new Dispatcher<int, int>(this.GetInstanceID());
            _isDamaged = new Dispatcher<int, DamageInfo>(this.GetInstanceID());
        }

        Dispatcher<int, DamageInfo>     _damageReceived;
        Dispatcher<int, int>                 _isDead;
        Dispatcher<int, DamageInfo>          _isDamaged;

        Animator _anim;                       // Reference to the animator.
        AudioSource _enemyAudio;              // Reference to the audio source.
        ParticleSystem _hitParticles;         // Reference to the particle system that plays when the enemy is damaged.
        int _currentHealth;                   // The current health the enemy has.
    }
}
