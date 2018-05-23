namespace Svelto.ECS.Example.Survive.HUD
{
	public struct HUDDamageEntityView: IEntityView
    {
        public IHealthComponent        healthComponent;
        public EGID ID { get; set; }
    }
}
