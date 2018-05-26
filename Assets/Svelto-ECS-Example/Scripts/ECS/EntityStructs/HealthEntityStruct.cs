namespace Svelto.ECS.Example.Survive.Player
{
    public struct HealthEntityStruct : IImplementor, IEntityData
    {
        public int currentHealth { get; set; }

        public HealthEntityStruct(int startingHealth) : this()
        {
            // Set the initial health of the player.
            currentHealth = startingHealth;
        }

        public EGID ID { get; set; }
    }
}