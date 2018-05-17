namespace Svelto.ECS.Example.Survive.Camera
{
    public struct CameraEntityView: IEntityData
    {
        public ITransformComponent transformComponent;
        public IPositionComponent  positionComponent;
    }
}