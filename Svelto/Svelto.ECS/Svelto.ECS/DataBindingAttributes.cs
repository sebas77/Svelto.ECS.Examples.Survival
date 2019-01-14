using System;

namespace Svelto.ECS
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataBindsToAttribute : Attribute
    {
        public DataBindsToAttribute(object type)
        {
            
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PushDataAttribute : Attribute
    {
        public PushDataAttribute(Type type)
        {
            
        }
    }
    
    public struct DataBind<T> where T:struct
    {
        DataBind(T value)
        {}

        public static implicit operator DataBind<T>(T value)
        {
            return new DataBind<T>(value);
        }
    }
}