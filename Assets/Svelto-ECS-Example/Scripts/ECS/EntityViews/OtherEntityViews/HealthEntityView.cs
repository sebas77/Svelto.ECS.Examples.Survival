namespace Svelto.ECS.Example.Survive
{
    public struct HealthEntityView: IEntityData
    {
        public IHealthComponent healthComponent;
        public EGID ID { get; set; }
    }
}
