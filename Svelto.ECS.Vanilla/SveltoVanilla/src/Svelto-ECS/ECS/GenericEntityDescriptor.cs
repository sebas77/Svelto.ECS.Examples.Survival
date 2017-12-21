namespace Svelto.ECS
{
    public class GenericEntityDescriptor<T>:IEntityDescriptor where T : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T()};
        }
        
        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }

    public class GenericEntityDescriptor<T, U> : IEntityDescriptor where T : class, IEntityViewBuilder, new() 
                                                                       where U : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T(), new U()};
        }

        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }

    public class GenericEntityDescriptor<T, U, V> : IEntityDescriptor where T : class, IEntityViewBuilder, new()
                                                                          where U : class, IEntityViewBuilder, new()
                                                                          where V : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T(), new U(), new V()};
        }

        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }

    public class GenericEntityDescriptor<T, U, V, W> : IEntityDescriptor where T : class, IEntityViewBuilder, new()
                                                                             where U : class, IEntityViewBuilder, new()
                                                                             where V : class, IEntityViewBuilder, new()
                                                                             where W : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T(), new U(), new V(), new W()};
        }

        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }

    public class GenericEntityDescriptor<T, U, V, W, X> : IEntityDescriptor where T : class, IEntityViewBuilder, new()
                                                                                where U : class, IEntityViewBuilder, new()
                                                                                where V : class, IEntityViewBuilder, new()
                                                                                where W : class, IEntityViewBuilder, new()
                                                                                where X : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T(), new U(), new V(), new W(), new X()};
        }

        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }

    public class GenericEntityDescriptor<T, U, V, W, X, Y> : IEntityDescriptor where T : class, IEntityViewBuilder, new()
                                                                                   where U : class, IEntityViewBuilder, new()
                                                                                   where V : class, IEntityViewBuilder, new()
                                                                                   where W : class, IEntityViewBuilder, new()
                                                                                   where X : class, IEntityViewBuilder, new()
                                                                                   where Y : class, IEntityViewBuilder, new()
    {
        static GenericEntityDescriptor()
        {
            _entityViewsToBuild = new IEntityViewBuilder[] {new T(), new U(), new V(), new W(), new X(), new Y()};
        }

        public IEntityViewBuilder[] entityViewsToBuild
        {
            get { return _entityViewsToBuild; }
        }       

        static readonly IEntityViewBuilder[] _entityViewsToBuild;
    }
}
