namespace Svelto.ECS.Example.Survive.Sound
{
    public struct DamageSoundEntityView: IEntityData
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
        public EGID ID { get; set; }
    }
}
