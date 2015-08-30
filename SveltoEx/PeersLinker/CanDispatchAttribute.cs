using System;

namespace Svelto.PeersLinker
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CanDispatchAttribute : Attribute
    {
        public Type notificationType { get; private set; }

        public CanDispatchAttribute(Type notificationClass)
        {
            notificationType = notificationClass;
        }
    }
}
