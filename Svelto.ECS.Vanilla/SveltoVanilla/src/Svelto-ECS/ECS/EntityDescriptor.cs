using System;
using System.Collections.Generic;

namespace Svelto.ECS
{
    public interface IEntityDescriptor
    {
        IEntityViewBuilder[] entityViewsToBuild { get; }
    }
}

namespace Svelto.ECS.Internal
{
    static class EntityDescriptor<TType> where TType : IEntityDescriptor, new()
    {
        static readonly TType Default = new TType();

        public static IEntityViewBuilder[] EntityViewsToBuild
        {
            get { return Default.entityViewsToBuild; }
        }
    }

    static class EntityFactory
    {
        internal static void BuildGroupedEntityViews(int entityID, int groupID,
                                                     Dictionary<int, Dictionary<Type, ITypeSafeList>>
                                                     groupEntityViewsByType,
                                                     IEntityViewBuilder[] entityViewsToBuild,
                                                     object[] implementors)
        {
            int count = entityViewsToBuild.Length;

            for (int index = 0; index < count; index++)
            {
                var entityViewBuilder = entityViewsToBuild[index];
                var entityViewType = entityViewBuilder.GetEntityViewType();

                Dictionary<Type, ITypeSafeList> groupedEntityViewsTyped;

                if (groupEntityViewsByType.TryGetValue(groupID, out groupedEntityViewsTyped) == false)
                {
                    groupedEntityViewsTyped = new Dictionary<Type, ITypeSafeList>();

                    groupEntityViewsByType.Add(groupID, groupedEntityViewsTyped);
                }

                var entityViewObjectToFill =
                    BuildEntityView(entityID, groupedEntityViewsTyped, entityViewType, entityViewBuilder);

                //the semantic of this code must still be improved
                //but only classes can be filled, so I am aware
                //it's a EntityViewWithID
                if (entityViewObjectToFill != null)
                    FillEntityView(entityViewObjectToFill as EntityView, implementors);
            }
        }

        internal static void BuildEntityViews(int entityID, Dictionary<Type, ITypeSafeList> entityViewsByType,
                                              IEntityViewBuilder[] entityViewsToBuild,
                                              object[] implementors)
        {
            int count = entityViewsToBuild.Length;

            for (int index = 0; index < count; index++)
            {
                var entityViewBuilder = entityViewsToBuild[index];
                var entityViewType = entityViewBuilder.GetEntityViewType();

                var entityViewObjectToFill =
                    BuildEntityView(entityID, entityViewsByType, entityViewType, entityViewBuilder);

                //the semantic of this code must still be improved
                //but only classes can be filled, so I am aware
                //it's a EntityViewWithID
                if (entityViewObjectToFill != null)
                    FillEntityView(entityViewObjectToFill as EntityView, implementors);
            }
        }

        static IEntityView BuildEntityView(int entityID, Dictionary<Type, ITypeSafeList> groupedEntityViewsTyped,
                                                 Type entityViewType, IEntityViewBuilder entityViewBuilderId)
        {
            ITypeSafeList entityViews;

            var entityViewsPoolWillBeCreated =
                groupedEntityViewsTyped.TryGetValue(entityViewType, out entityViews) == false;
            var entityViewObjectToFill = entityViewBuilderId.BuildEntityViewAndAddToList(ref entityViews, entityID);

            if (entityViewsPoolWillBeCreated)
                groupedEntityViewsTyped.Add(entityViewType, entityViews);

            return entityViewObjectToFill;
        }

        //this is used to avoid newing a dictionary every time, but it's used locally only
        static readonly Dictionary<Type, object> implementorsByType = new Dictionary<Type, object>();

        static void FillEntityView<TEntityViewType>(TEntityViewType entityView, 
                                                    object[] implementors)
            where TEntityViewType : EntityView
        {
            for (int index = 0; index < implementors.Length; index++)
            {
                var implementor = implementors[index];

                if (implementor != null)
                {
                    var type = implementor.GetType();

                    Type[] interfaces;
                    if (_cachedTypes.TryGetValue(type, out interfaces) == false)
                        interfaces = _cachedTypes[type] = type.GetInterfaces(); 

                    for (int iindex = 0; iindex < interfaces.Length; iindex++)
                    {
                        var componentType = interfaces[iindex];
#if DEBUG && !PROFILER
                        if (implementorsByType.ContainsKey(componentType) == true)
                        {
                            Utility.Console.LogError(DUPLICATE_IMPLEMENTOR_ERROR.FastConcat(
                                                         "component: ", componentType.ToString(), " implementor: ",
                                                         implementor.ToString()));

                        }
#endif

                        implementorsByType[componentType] = implementor;
                    }
                }
#if DEBUG && !PROFILER
                else
                    Utility.Console.LogError(NULL_IMPLEMENTOR_ERROR.FastConcat(entityView.ToString()));
#endif
            }

            int count;

            KeyValuePair<Type, Action<TEntityViewType, object>>[] setters =
                EntityView.EntityViewBlazingFastReflection(entityView, out count);

            for (int i = 0; i < count; i++)
            {
                object component;

                var keyValuePair = setters[i];
                Type fieldType = keyValuePair.Key;

                if (implementorsByType.TryGetValue(fieldType, out component) == false)
                {
                    Exception e = new Exception(NOT_FOUND_EXCEPTION + fieldType.Name + " - EntityView: " +
                                                entityView.GetType().Name + " - EntityDescriptor " /* + this*/);

                    throw e;
                }

                keyValuePair.Value(entityView, component);

            }

            implementorsByType.Clear();
        }

        static Dictionary<Type, Type[]> _cachedTypes = new Dictionary<Type, Type[]>();

        const string DUPLICATE_IMPLEMENTOR_ERROR =
            "the same component is implemented with more than one implementor. This is considered an error and MUST be fixed. ";

        const string NULL_IMPLEMENTOR_ERROR =
            "Null implementor, are you using a wild GetComponents<Monobehaviour> to fetch it? ";

        const string NOT_FOUND_EXCEPTION = "Svelto.ECS: Implementor not found for a EntityView. Implementor Type: ";
    }
}

