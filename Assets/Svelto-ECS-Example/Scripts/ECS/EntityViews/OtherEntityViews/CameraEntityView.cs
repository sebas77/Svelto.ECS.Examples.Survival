namespace Svelto.ECS.Example.Survive.Camera
{
    public struct CameraEntityView: IEntityView
    {
        public ITransformComponent transformComponent;
        public IPositionComponent  positionComponent;
        
        public EGID ID { get; set; }
    }
}