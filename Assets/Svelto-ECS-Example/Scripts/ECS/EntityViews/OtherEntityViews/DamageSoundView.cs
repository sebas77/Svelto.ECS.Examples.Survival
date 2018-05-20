namespace Svelto.ECS.Example.Survive.Sound
{
    public struct DamageSoundEntityView: IEntityData
    {
        public IDamageSoundComponent    audioComponent;
        public EGID ID { get; set; }
    }
}
