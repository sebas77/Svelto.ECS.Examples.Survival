namespace Svelto.ECS.Internal
{
    sealed class RemoveEntityImplementor : IRemoveEntityComponent
    {
        IEntityDescriptor _entityTemplate;
        int groupID;

        public RemoveEntityImplementor(IEntityDescriptor template, int groupID) : this(template)
        {
            _entityTemplate = template;
            this.groupID = groupID;
        }

        internal RemoveEntityImplementor(IEntityDescriptor template)
        {
            _entityTemplate = template;
        }

        public IEntityDescriptor entityDescriptor
        {
            get { return _entityTemplate; }
        }
    }
}

namespace Svelto.ECS
{
    public interface IRemoveEntityComponent
    {
        IEntityDescriptor entityDescriptor { get; }
    }
}
