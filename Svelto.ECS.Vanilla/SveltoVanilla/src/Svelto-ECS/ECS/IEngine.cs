namespace Svelto.ECS.Internal
{
    public interface IHandleEntityViewEngine : IEngine
    {
        void Add(EntityView entityView);
        void Remove(EntityView entityView);
    }
}

namespace Svelto.ECS
{
    public interface IEngine
    {}
#if EXPERIMENTAL
    public interface IHandleActivableEntityEngine : IEngine
    {
        void Enable(EntityView entityView);
        void Disable(EntityView entityView);
    }
#endif
    public interface IQueryingEntityViewEngine : IEngine
    {
        IEngineEntityViewDB entityViewsDB { set; }

        void Ready();
    }
}
