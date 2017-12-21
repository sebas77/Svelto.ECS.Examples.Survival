using System;

namespace Svelto.ECS.EntityViewSchedulers
{
    public abstract class EntityViewSubmissionScheduler
    {
        abstract public void Schedule(Action submitEntityViews);
    }
}