using Svelto.ECS.Internal;

namespace Svelto.ECS.Internal
{
    public abstract class MultiEntityViewsEngine<T>:IHandleEntityViewEngine where T:EntityView
    {
        protected abstract void Add(T entityView);
        protected abstract void Remove(T entityView);
        
        public virtual void Add(EntityView entityView)
        {
            Add((T) entityView);
        }

        public virtual void Remove(EntityView entityView)
        {
            Remove((T) entityView);
        }
    }
}

namespace Svelto.ECS
{
    public abstract class MultiEntityViewsEngine<T, U> : MultiEntityViewsEngine<T>
        where T:EntityView where U:EntityView
    {
        protected abstract void Add(U entityView);
        protected abstract void Remove(U entityView);

        public override void Add(EntityView entityView)
        {
            var castedEntityView = entityView as U;
            if (castedEntityView != null)
            {
                Add(castedEntityView);
            }
            else
            {
                base.Add(entityView);
            }
        }

        public override void Remove(EntityView entityView)
        {
            if (entityView is U)
            {
                Remove((U) entityView);
            }
            else
            {
                base.Remove(entityView);
            }
        }
    }

    public abstract class MultiEntityViewsEngine<T, U, V> : MultiEntityViewsEngine<T, U> 
        where T: EntityView where U : EntityView where V:EntityView
    {
        protected abstract void Add(V entityView);
        protected abstract void Remove(V entityView);

        public override void Add(EntityView entityView)
        {
            var castedEntityView = entityView as V;
            if (castedEntityView != null)
            {
                Add(castedEntityView);
            }
            else
                base.Add(entityView);
        }

        public override void Remove(EntityView entityView)
        {
            var castedEntityView = entityView as V;
            if (castedEntityView != null)
            {
                Remove(castedEntityView);
            }
            else
                base.Remove(entityView);
        }
    }
}