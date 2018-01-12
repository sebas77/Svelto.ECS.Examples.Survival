using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerHealthImplementor : MonoBehaviour, IImplementor, IHealthComponent, IDamageSoundComponent
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public AudioClip damageClip;                                 // The audio clip to play when the player dies.

        int  IHealthComponent.currentHealth   { get { return _currentHealth; } set { _currentHealth = value; } }

        AudioSource IDamageSoundComponent.audioSource   { get { return _playerAudio; } }
        AudioClip   IDamageSoundComponent.death         { get { return deathClip; } }
        AudioClip   IDamageSoundComponent.damage        { get { return damageClip; } }

        void Awake()
        {
            _playerAudio = GetComponent<AudioSource> ();

            // Set the initial health of the player.
            _currentHealth = startingHealth;
        }

        /// <summary>
        /// I have no clue how this animation even is called, I spent 30 minutes 
        /// to figure out how to look up for an AnimationEvent. Eventually
        /// I gave up. Don't remove it, unity is looking for this function
        /// magically.
        /// </summary>
        public void RestartLevel()
        {}

        int                 _currentHealth;
        AudioSource         _playerAudio;                                    // Reference to the AudioSource component.
    }
}
