using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Enemies;
using Svelto.ECS.Example.Survive.Components.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyAudioImplementor : MonoBehaviour, IImplementor, 
        IAnimationComponent, IEnemyVFXComponent, IDamageSoundComponent, ITargetTypeComponent
    {
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        public AudioClip damageClip;                 // The sound to play when the enemy dies.
        public PlayerTargetType targetType;

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
        }

        Animator        _anim;                 // Reference to the animator.
        AudioSource     _enemyAudio;           // Reference to the audio source.
        ParticleSystem  _hitParticles;         // Reference to the particle system that plays when the enemy is damaged.
    }
}
