namespace Svelto.ECS.Example.Survive.Sound
{
    public struct DamageSoundEntityView: IEntityViewStruct
    {
        public IDamageSoundComponent    audioComponent;
        public EGID ID { get; set; }
    }
}
