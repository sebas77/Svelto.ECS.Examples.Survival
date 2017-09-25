namespace Svelto.ECS.Example.Flock
{
    public class BoidNode : NodeWithID
    {
        public ITransformComponent transform;
        public IBoidComponent boid;
        public IThreadSafeTransformComponent transformTS;
    }
}