using System;
using System.Collections.Generic;
using System.Reflection;
using Svelto.DataStructures;
using Svelto.Utilities;

namespace Svelto.ECS
{   
    public interface IEntityView
    {
        int ID { get; }
    }
    
    public interface IEntityStruct:IEntityView
    {
        new int ID { set; }
    }

    public class EntityView: IEntityView
    {
        public int ID { get { return _ID; } }
        
        internal static TEntityViewType BuildEntityView<TEntityViewType>(int ID) where TEntityViewType: EntityView, new() 
        {
            return new TEntityViewType { _ID = ID };
        }

        internal static KeyValuePair<Type, Action<TEntityViewType, object>>[] 
            EntityViewBlazingFastReflection<TEntityViewType>(TEntityViewType view, out int count) where TEntityViewType: EntityView 
        {
            var type = view.GetType();

            if (FieldCache<TEntityViewType>.list.Count == 0)
            {
                var fields = type.GetFields(BindingFlags.Public |
                                            BindingFlags.Instance);

                for (int i = fields.Length - 1; i >= 0; --i)
                {
                    var field = fields[i];

                    Action<TEntityViewType, object> setter = FastInvoke<TEntityViewType>.MakeSetter(field);

                    FieldCache<TEntityViewType>.Add(new KeyValuePair<Type, Action<TEntityViewType, object>>(field.FieldType, setter));
                }
            }
            
            return FasterList<KeyValuePair<Type, Action<TEntityViewType, object>>>.NoVirt.ToArrayFast(FieldCache<TEntityViewType>.list, out count);
        }

        int _ID;

        static class FieldCache<T> where T : class
        {
            internal static void Add(KeyValuePair<Type, Action<T, object>> setter)
            {
                list.Add(setter);
            }
            
            internal static readonly FasterList<KeyValuePair<Type, Action<T, object>>> list = new FasterList<KeyValuePair<Type, Action<T, object>>>();
        }
    }
}

