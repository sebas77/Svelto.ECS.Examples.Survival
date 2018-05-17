namespace Svelto.ECS.Example.Survive.Camera
{
    public struct CameraTargetEntityView: IEntityData
    {
        public ICameraTargetComponent targetComponent;
        public EGID ID { get; set; }
    }
}