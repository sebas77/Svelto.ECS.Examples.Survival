namespace Svelto.ECS.Example.Survive.Player
{
    public struct HealthEntityStruct : IEntityData
    {
        public int currentHealth;
        public EGID ID { get; set; }
    }
}