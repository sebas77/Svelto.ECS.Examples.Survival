using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Components.Enemies;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyHealthImplementor : MonoBehaviour, IImplementor, IDestroyComponent, IHealthComponent
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.

        int   IHealthComponent.currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

        void Awake ()
        {
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
