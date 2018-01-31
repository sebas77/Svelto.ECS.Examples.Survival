using Svelto.ECS.Example.Survive.Components.Damageable;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerHealthImplementor : MonoBehaviour, IImplementor, IHealthComponent
    {
        public int       startingHealth = 100; // The amount of health the player starts the game with.

        public int currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

        void Awake()
        {
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

        int         _currentHealth;
    }
}