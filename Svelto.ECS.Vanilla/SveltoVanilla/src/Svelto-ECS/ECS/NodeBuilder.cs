using System;
using Svelto.DataStructures;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public interface IEntityViewBuilder
    {
        IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID);

        Type GetEntityViewType();
    }

    public class EntityViewWithBuilder<EntityViewType> : IEntityViewBuilder where EntityViewType : EntityView, new()
    {
        public IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForClasses<EntityViewType>();

            var castedList = list as TypeSafeFasterListForECSForClasses<EntityViewType>;

            var entityView = EntityView.BuildEntityView<EntityViewType>(entityID);

            castedList.Add(entityView);

            return entityView;
        }

        public Type GetEntityViewType()
        {
            return _entityViewType;
        }

        readonly Type _entityViewType = typeof(EntityViewType);
    }

    public class EntityStructWithBuilder<EntityViewType> : IEntityViewBuilder where EntityViewType : struct, IEntityStruct
    {
        public IEntityView BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID)
        {
            var entityView = default(EntityViewType);
            entityView.ID = entityID;
            
            if (list == null)
                list = new TypeSafeFasterListForECSForStructs<EntityViewType>();

            var castedList = list as TypeSafeFasterListForECSForStructs<EntityViewType>;

            castedList.Add(entityView);

            return null;
        }

        public Type GetEntityViewType()
        {
            return _entityViewType;
        }

        readonly Type _entityViewType = typeof(EntityViewType);
    }    
}