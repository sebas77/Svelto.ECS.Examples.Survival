namespace Svelto.ECS.Example.Survive
{
    public struct HealthEntityView: IEntityView
    {
        public IHealthComponent healthComponent;
        public EGID ID { get; set; }
    }
}
