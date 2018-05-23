namespace Svelto.ECS.Example.Survive.Camera
{
    public struct CameraTargetEntityView: IEntityView
    {
        public ICameraTargetComponent targetComponent;
        public EGID ID { get; set; }
    }
}