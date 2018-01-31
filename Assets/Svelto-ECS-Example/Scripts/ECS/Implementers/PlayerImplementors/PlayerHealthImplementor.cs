using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Implementors.Player
{
    public class PlayerHealthImplementor : IImplementor, IHealthComponent
    {
        public int currentHealth { get; set; }

        public PlayerHealthImplementor(int startingHealth)
        {
            // Set the initial health of the player.
            currentHealth = startingHealth;
        }
    }
}