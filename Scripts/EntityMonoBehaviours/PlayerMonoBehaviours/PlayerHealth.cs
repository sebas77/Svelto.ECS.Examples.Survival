using SharedComponents;
using UnityEngine;

namespace CompleteProject
{
    public class PlayerHealth : MonoBehaviour, IHealthComponent, IDamageSoundComponent, IDamageEventComponent
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public AudioClip damageClip;                                 // The audio clip to play when the player dies.

        int  IHealthComponent.currentHealth   { get { return _currentHealth; } set { _currentHealth = value; } }
        bool IHealthComponent.hasBeenDamaged  { get; set; }

        Dispatcher<IDamageEventComponent, DamageInfo>    IDamageEventComponent.damageReceived { get { return _damageReceived; } }
        Dispatcher<IHealthComponent, GameObject>          IHealthComponent.isDead         { get { return _isDead; } }
        Dispatcher<IHealthComponent, DamageInfo>  IHealthComponent.isDamaged { get { return _isDamaged; } }

        AudioSource IDamageSoundComponent.audioSource { get { return _playerAudio; } }
        AudioClip   IDamageSoundComponent.death       { get { return deathClip; } }
        AudioClip   IDamageSoundComponent.damage { get { return damageClip; } }

        void Awake()
        {
            _playerAudio = GetComponent<AudioSource> ();

            // Set the initial health of the player.
            _currentHealth = startingHealth;

            _isDead = new DispatcherOnChange<IHealthComponent, GameObject>(this);
            _isDamaged = new DispatcherOnChange<IHealthComponent, DamageInfo>(this);
            _damageReceived = new Dispatcher<IDamageEventComponent, DamageInfo>(this);
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

        Dispatcher<IHealthComponent, GameObject>            _isDead;
        Dispatcher<IHealthComponent, DamageInfo>            _isDamaged;
        Dispatcher<IDamageEventComponent, DamageInfo>      _damageReceived;
    }
}
