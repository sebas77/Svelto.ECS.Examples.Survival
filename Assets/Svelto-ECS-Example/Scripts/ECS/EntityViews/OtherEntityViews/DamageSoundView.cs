namespace Svelto.ECS.Example.Survive.Sound
{
    public struct DamageSoundEntityView: IEntityView
    {
        public IDamageSoundComponent    audioComponent;
        public EGID ID { get; set; }
    }
}
