namespace Svelto.ECS.Example.Survive.HUD
{
	public struct HUDDamageEntityView: IEntityData
    {
        public IHealthComponent        healthComponent;
        public EGID ID { get; set; }
    }
}
