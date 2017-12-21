using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        /// I decided to leave this one for compatibility, probably some ugly animation callback.
        /// </summary>
        public void RestartLevel()
        {
            TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks(); //Tasks like not ending loops must be cleared manually with this function

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        int                 _currentHealth;
        AudioSource         _playerAudio;                                    // Reference to the AudioSource component.
    }
}
