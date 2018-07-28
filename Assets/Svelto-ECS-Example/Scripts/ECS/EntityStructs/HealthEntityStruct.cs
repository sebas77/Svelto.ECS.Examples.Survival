namespace Svelto.ECS.Example.Survive.Characters.Player
{
    public struct HealthEntityStruct : IEntityStruct
    {
        public int currentHealth;
        
        public EGID ID { get; set; }
    }
}