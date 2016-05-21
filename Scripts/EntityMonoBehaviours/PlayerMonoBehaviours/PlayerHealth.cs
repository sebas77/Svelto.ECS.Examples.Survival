using Components.Base;
using Components.Damageable;
using UnityEngine;

namespace Implementators.Player
{
    public class PlayerHealth : MonoBehaviour, IHealthComponent, IDamageSoundComponent, IDamageEventComponent
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public AudioClip damageClip;                                 // The audio clip to play when the player dies.

        int  IHealthComponent.currentHealth   { get { return _currentHealth; } set { _currentHealth = value; } }
        bool IHealthComponent.hasBeenDamaged  { get; set; }

        Dispatcher<int, DamageInfo>    IDamageEventComponent.damageReceived { get { return _damageReceived; } }
        Dispatcher<int>                IHealthComponent.isDead              { get { return _isDead; } }
        Dispatcher<int, DamageInfo>    IHealthComponent.isDamaged           { get { return _isDamaged; } }

        AudioSource IDamageSoundComponent.audioSource   { get { return _playerAudio; } }
        AudioClip   IDamageSoundComponent.death         { get { return deathClip; } }
        AudioClip   IDamageSoundComponent.damage        { get { return damageClip; } }

        void Awake()
        {
            _playerAudio = GetComponent<AudioSource> ();

            // Set the initial health of the player.
            _currentHealth = startingHealth;

            _isDead = new Dispatcher<int>(gameObject.GetInstanceID());
            _isDamaged = new DispatcherOnChange<int, DamageInfo>(gameObject.GetInstanceID());
            _damageReceived = new Dispatcher<int, DamageInfo>(gameObject.GetInstanceID());
        }

        /// <summary>
        /// I decided to leave this one for compatibility, probably some ugly animation callback.
        /// </summary>
        public void RestartLevel()
        {
            Application.LoadLevel (Application.loadedLevel);
        }

        int                 _currentHealth;
        AudioSource         _playerAudio;                                    // Reference to the AudioSource component.

        Dispatcher<int>              _isDead;
        Dispatcher<int, DamageInfo>  _isDamaged;
        Dispatcher<int, DamageInfo>  _damageReceived;
    }
}
